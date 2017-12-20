using GraphQL.Types;
using Ql.Data;

namespace Ql.Middleware.GraphQlTypes
{
    public class AuthorQuery : ObjectGraphType
    {
        public AuthorQuery(IBookRepository bookRepository)
        {
            Field<ListGraphType<AuthorType>>("authors",
                resolve: context =>
                {
                    return bookRepository.AllAuthors();
                });


        }
    }
}
