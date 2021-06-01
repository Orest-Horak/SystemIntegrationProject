using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using GraphQL.Types;

namespace GNews.Models
{
    public class CommentType : ObjectGraphType<CommentDTO>
    {
        public CommentType()
        {
            Field(x => x.ID, true);
            Field(x => x.Author);
            Field(x => x.Text);
            Field(x => x.DateOfPost);
        }
    }
}
