using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;

namespace GNews.Models
{
    public class NewsSchema : Schema
    {
        public NewsSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<NewsQuery>();
            Mutation = provider.GetRequiredService<NewsMutation>();
        }
    }
}
