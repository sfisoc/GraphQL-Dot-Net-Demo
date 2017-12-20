using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL;
using System.IO;
using System.Collections.Generic;
using Ql.Data;
using Ql.Middleware.GraphQlTypes;

namespace Ql.Middleware
{
    public class GraphQlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IBookRepository _bookRepository;

        public GraphQlMiddleware(RequestDelegate next, IBookRepository bookRepository)
        {
            _next = next;
            _bookRepository = bookRepository;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sent = false;
            if (httpContext.Request.Path.StartsWithSegments("/graph"))
            {
                using (var sr = new StreamReader(httpContext.Request.Body))
                {
                    var query = await sr.ReadToEndAsync();
                    if (!String.IsNullOrWhiteSpace(query))
                    {
                        var schema = this.SchemaResolve(query);
                       
                        var result = await new DocumentExecuter()
                            .ExecuteAsync(options =>
                            {
                                options.Schema = schema;
                                options.Query = query;
                            }).ConfigureAwait(false);

                        CheckForErrors(result);

                        await WriteResult(httpContext, result);

                        sent = true;
                    }
                }
            }
            if (!sent)
            {
                await _next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, ExecutionResult result)
        {
            var json = new DocumentWriter(indent: true).Write(result);

            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(json);
        }

        
        private Schema SchemaResolve(String rootQuery)
        {
  
            

            if (rootQuery.Contains("publishers"))
            {
                var pub = new Schema { Query = new PublisherQuery(_bookRepository) };

                return pub;
            }
            else if(rootQuery.Contains("authors"))
            {
                var aut = new Schema { Query = new AuthorQuery(_bookRepository) };
                return aut;
            }
           
            var book = new Schema { Query = new BooksQuery(_bookRepository) };

            return book;
            


        }

        private void CheckForErrors(ExecutionResult result)
        {
            if (result.Errors?.Count > 0)
            {
                var errors = new List<Exception>();
                foreach (var error in result.Errors)
                {
                    var ex = new Exception(error.Message);
                    if (error.InnerException != null)
                    {
                        ex = new Exception(error.Message, error.InnerException);
                    }
                    errors.Add(ex);
                }
                throw new AggregateException(errors);
            }
        }
    }
}
