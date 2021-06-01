using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Interfaces;
using GraphQL.Types;
using GraphQL;

namespace GNews.Models
{
    public class NewsQuery : ObjectGraphType
    {
        public NewsQuery(INewsRepository newsRepository)
        {
            Field<ListGraphType<NewsType>>(
                "news",
                resolve: context => newsRepository.GetAllNewsAsync()
            );

            Field<ListGraphType<NewsType>>(
                "newsById",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id"}),
                 resolve: context => newsRepository.GetNewsById(context.GetArgument<string>("id"))
            );
        }
    }
}
