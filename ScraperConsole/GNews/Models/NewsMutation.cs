using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Interfaces;
using GraphQL.Types;
using GraphQL;
using DTO;

namespace GNews.Models
{
    public class NewsMutation : ObjectGraphType
    {
        public NewsMutation(INewsRepository newsRepository)
        {
            Field<NewsType>(
                "addNews",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<NewsInputType>> { Name = "news"}
                    ),
                resolve: context => 
                {
                    var n = context.GetArgument<NewsDTO>("news");
                    return newsRepository.AddNews(n);
                });

            //Field<NewsType>(
            //    "feedback",
            //    arguments: new QueryArguments(
            //        new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id"},
            //         new QueryArgument<NonNullGraphType<BooleanGraphType>> { Name = "like" }
            //         ),
            //    resolve: context =>
            //    {
            //        var externalId = context.GetArgument<string>("id");
            //        var like = context.GetArgument<bool>("like");
            //        var n = newsRepository.GetNewsById(externalId).Result;
            //    });
        }
    }
}
