using GraphQL.Types;
using Ql.Data;
using Ql.Models;

namespace Ql.Middleware.GraphQlTypes
{
    public class PublisherType : ObjectGraphType<Publisher>
    {
        public PublisherType()
        {
            Field(x => x.Id).Description("The id of the publisher.");
            Field(x => x.Name).Description("The name of the publisher.");
            Field<BookType>("book");
            Field<ListGraphType<BookType>>("books",
               resolve: context => {

                   IBookRepository bookRepository = new BookRepository();


                   return bookRepository.PublishedBooks(context.Source.Id);

               });
            Field<ListGraphType<AuthorType>>("authors",
                resolve: context => new Author[] { });
        }
    }
}
