using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using GraphQL.Types;


namespace GNews.Models
{
    public class NewsType : ObjectGraphType<NewsDTO>
    {
        public NewsType()
        {
            Field(x => x.ID, true);
            Field(x => x.Author, true);
            Field(x => x.Title);
            Field(x => x.Url);
            Field(x => x.Description, true);
            Field(x => x.DateOfPublication, true);
            Field(x => x.Like, nullable: true);
            Field(x => x.Comments, type: typeof(ListGraphType<CommentType>) );
        }
    }
}
