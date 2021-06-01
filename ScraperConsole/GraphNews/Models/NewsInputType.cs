﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using DTO;
using GraphQL.Types;

namespace GNews.Models
{
    //public class NewsInputType : ObjectGraphType<NewsEntity>
    //{
    //    public NewsInputType()
    //    {
    //        Field(x => x.ExternalId, true);
    //        Field(x => x.Author, true);
    //        Field(x => x.Title);
    //        Field(x => x.Url);
    //        Field(x => x.Description, true);
    //        Field(x => x.DateOfPublication, true);
    //        Field(x => x.Like, nullable: true);
    //        Field(x => x.Comments, type: typeof(ListGraphType<CommentType>));
    //    }
    //}

    public class NewsInputType : InputObjectGraphType
    {
        public NewsInputType()
        {
            Name = "NewsInput";
            Field<NonNullGraphType<StringGraphType>>("author");
            Field<StringGraphType>("title");
            Field<StringGraphType>("url");
            Field<StringGraphType>("description");
            Field<DateGraphType>("dateOfPublication");
        }
    }
}
