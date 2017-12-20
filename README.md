# GraphQL-Dot-Net-Demo
Graph QL function demonstration, based on http://asp.net-hacker.rocks/2017/05/29/graphql-and-aspnetcore.html blog

#Extentiosn
Added new query types to extent the original.
          {
            authors{
              id,
              name,
              books{
                isbn,
                name
              }
            }
          }
          -----------------------------------
          and
     {
      publishers{
        id,
        name,
        books{
          isbn,
          name
        }
      }
    }
    
 Authors and Publsihers can be quried separatly.
 

