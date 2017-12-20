using GraphQL.Types;
using Ql.Data;

namespace Ql.Middleware.GraphQlTypes
{
    public class PublisherQuery : ObjectGraphType
    {

        public PublisherQuery(IBookRepository bookRepository)
        {
            Field<ListGraphType<PublisherType>>("publishers",
               resolve: context =>
               {
                   return bookRepository.AllPublishers();
               });
        }
       
    }
}
