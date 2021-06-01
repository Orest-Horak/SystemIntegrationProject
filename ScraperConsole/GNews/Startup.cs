using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Configuration;
using DAL.Interfaces;
using DAL.MappingProfiles;
using DAL;
using GraphQL;
using GNews.Models;
using AutoMapper;
using GraphQL.Types;
using Newtonsoft.Json;
using GraphQL.Server.Transports.WebSockets;
using GraphQL.Server;
using GraphQL.Server.Ui.Altair;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;

namespace GNews
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllersWithViews();
            services.AddControllers();
            var dbConfig = new MongoDbConfig
            {
                ConnectionString = "mongodb://localhost:27017",
                Database = "NewsCrawl"
            };
            Configuration.Bind("MongoConnection", dbConfig);

            services.AddHttpContextAccessor();

            //services.AddAutoMapper();
            //services.AddAutoMapper(typeof(INewsRepository).Assembly);
            MapperConfiguration mapperConfiguration = MappingProfile.InitializeAutoMapper();
            IMapper mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            //services.AddSingleton<IMapper>(s => (IMapper)MappingProfile.InitializeAutoMapper());

            services.AddSingleton(dbConfig);
            //services.AddScoped<INewsRepository, NewsRepository>();
            //services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            //services.AddScoped<NewsQuery>();
            //services.AddScoped<NewsMutation>();
            //services.AddScoped<CommentType>();
            //services.AddScoped<NewsType>();
            //services.AddScoped<NewsInputType>();
            //services.AddScoped<ISchema, NewsSchema>();
            services.AddSingleton<INewsRepository, NewsRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<NewsSchema>();
            services.AddSingleton<NewsQuery>();
            services.AddSingleton<NewsMutation>();
            services.AddSingleton<CommentType>();
            services.AddSingleton<NewsType>();
            services.AddSingleton<NewsInputType>();
            //services.AddSingleton<ISchema, NewsSchema>();
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllersWithViews().AddNewtonsoftJson();
            //services.AddRazorPages().AddNewtonsoftJson();
            //services.AddNewston
            //services.AddMvc().AddNewtonsoftJson();
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //
            //services.AddSystemTextJson(); // For .NET Core 3+
            //services.AddMvc().AddNewtonsoftJson(); // For everything else
            //services.AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = Environment.IsDevelopment());
            //services.AddWebSockets(); // Add required services for web socket support
            //services.AddDataLoader(); // Add required services for DataLoader support
            //services.AddGraphTypes(typeof(NewsSchema)); // Add all IGraphType implementors in assembly which ChatSchema exists 
            //
            services
                .AddGraphQL((options, provider) =>
       {
           options.EnableMetrics = Environment.IsDevelopment();
       }).AddSystemTextJson() // For .NET Core 3+ 
       .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = Environment.IsDevelopment())
       .AddWebSockets() // Add required services for web socket support
       .AddDataLoader() // Add required services for DataLoader support
       .AddGraphTypes(typeof(NewsSchema)); // Add all IGraphType implementors in assembly which ChatSchema exists 

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseGraphQL<NewsSchema>();
            //app.UseGraphQLGraphiQL("/graphiql"/*, "/graphql"*/);
            //app.UseGraphQLPlayground();
            //app.UseGraphQLAltair();
            //app.UseGraphQLVoyager();


            // use websocket middleware for ChatSchema at default path /graphql
            app.UseGraphQLWebSockets<NewsSchema>();

            // use HTTP middleware for ChatSchema at default path /graphql
            app.UseGraphQL<NewsSchema>();

            // use GraphiQL middleware at default path /ui/graphiql with default options
            app.UseGraphQLGraphiQL();

            // use GraphQL Playground middleware at default path /ui/playground with default options
            app.UseGraphQLPlayground();

            // use Altair middleware at default path /ui/altair with default options
            app.UseGraphQLAltair();

            // use Voyager middleware at default path /ui/voyager with default options
            app.UseGraphQLVoyager();

            //app.UseGraphiQl("/graphiql", "/graphql");
            //app.UseGraphQL<NewsSchema>("/graphql");
            //app.UseGraphQLWebSockets<NewsSchema>();

            app.UseWebSockets();
            app.UseHttpsRedirection();
            app.UseCors();
            //app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGraphQLWebSockets<NewsSchema>();
                //endpoints.MapGraphQL<ChatSchema, GraphQLHttpMiddlewareWithLogs<ChatSchema>>();
                //endpoints.MapGraphQLWebSockets<NewsSchema>();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Main}");
            });
        }
    }
}
