namespace POC_GraphQL
{
    using GraphiQl;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using POC_GraphQL.Schemas;
    using GraphQL;
    using GraphQL.Types;
    using POC_GraphQL.Models;
    using POC_GraphQL.Queries;
    using POC_GraphQL.Repositories;
    using System.Linq;

    public class Startup
    {

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html</param>
        /// <param name="hostingEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default. See http://docs.asp.net/en/latest/fundamentals/environments.html</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this._configuration = configuration;
            this._hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var assembly = typeof(IHumanRepository).Assembly; // I actually use Assembly.LoadFile with well-known names 
            RegisterRepositories(services, assembly);
            RegisterGraphQLTypes(services, assembly);

            services.AddSingleton<RootQuery>();
            services.AddSingleton<RootMutation>();
            services.AddSingleton<RootSubscription>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new MainSchema(new FuncDependencyResolver(type => sp.GetService(type))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseGraphiQl();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static void RegisterGraphQLTypes(IServiceCollection services, System.Reflection.Assembly assembly)
        {
            var types = assembly.ExportedTypes
               // filter types that are unrelated
               .Where(x => x.IsClass && x.IsPublic && (
               x.Name.EndsWith("InputObject") ||
               x.Name.EndsWith("CreatedEvent") ||
               x.Name.EndsWith("Interface") ||
               x.Name.EndsWith("GType")));

            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
        }

        private static void RegisterRepositories(IServiceCollection services, System.Reflection.Assembly assembly)
        {
            var types = assembly.ExportedTypes
               // filter types that are unrelated
               .Where(x => x.IsClass && x.IsPublic && (
               x.Name.EndsWith("Repository")));

            foreach (var type in types)
            {
                services.AddSingleton(type.GetInterface($"I{type.Name}"), type);
            }
        }

    }
}
