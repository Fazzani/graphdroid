namespace POC_GraphQL
{
    using global::Common;
    using GraphQL;
    using GraphQL.Conventions;
    using GraphQL.DataLoader;
    using GraphQL.Http;
    using GraphQL.Relay.Types;
    using GraphQL.Server;
    using GraphQL.Server.Ui.Playground;
    using GraphQL.Server.Ui.Voyager;
    using GraphQL.Types;
    using GraphQL.Types.Relay;
    using GraphQL.Validation;
    using IdentityServer4.AccessTokenValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using POC_GraphQL.Common;
    using POC_GraphQL.Queries;
    using POC_GraphQL.Repositories;
    using POC_GraphQL.Schemas;
    using POC_GraphQL.ValidationRules;
    using System.Linq;

    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html</param>
        /// <param name="hostingEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default. See http://docs.asp.net/en/latest/fundamentals/environments.html</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<GraphQLOptions>(Configuration.GetSection(nameof(GraphQLOptions)));
            services.AddSingleton<IDependencyInjector, Injector>();
            var assembly = typeof(IHumanRepository).Assembly;
            RegisterRepositories(services, assembly);
            RegisterGraphQLTypes(services, assembly);
            services.AddScoped<IUserContext, GraphQLUserContext>();
            //Adding GraphQL Relay services
            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            services.AddTransient<NodeInterface>();
            services.AddTransient<PageInfoType>();
            //Adding GraphQL services
            services.AddSingleton<RootQuery>();
            services.AddSingleton<RootMutation>();
            services.AddSingleton<RootSubscription>();
            //Custom validation rules
            services.AddSingleton<IValidationRule, DebugValidationRule>();
            services.AddSingleton<IValidationRule, RequiresAuthValidationRule>();
            //Adding DataLoader
            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<DataLoaderDocumentListener>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            var sp = services.BuildServiceProvider();
            services.AddSingleton(new MainSchema(new FuncDependencyResolver(type => sp.GetService(type))));
            services.AddSingleton<ISchema>(new MainSchema(new FuncDependencyResolver(type => sp.GetService(type))));

            //Auth
            // extension method defined in this project
            services.AddGraphQLAuth(_ =>
            {
                _.AddPolicy(Constants.Policies.AdminPolicy, p => p.RequireClaim("role", Constants.Permissions.ADMIN));
                _.AddPolicy(Constants.Policies.ViewerPolicy, p => p.RequireClaim("role", Constants.Permissions.READ_ONLY));
            });

            //GraphQL Configuration
            var graphqlOptions = sp.GetService<IOptionsMonitor<GraphQLOptions>>();
            services
                .AddGraphQL(graphqlOptions.CurrentValue)
                .AddUserContextBuilder(context => sp.GetService<IUserContext>())
                .AddWebSockets() // Add required services for web socket support
                .AddDataLoader(); // Add required services for DataLoader support;

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
           .AddIdentityServerAuthentication(
                opt =>
                {
                    opt.Authority = "http://identityserver";
                    opt.RequireHttpsMetadata = false;
                    opt.ApiName = "graphqlApi";
                });

            services.AddCors();
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

            app.UseCors(
                builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:53374", "https://localhost:44363"));

            // this is required for websockets support
            app.UseWebSockets();

            // use websocket middleware for ChatSchema at path /graphql
            app.UseGraphQLWebSockets<MainSchema>("/graphql");

            // use HTTP middleware for MainSchema at path /graphql
            //app.UseGraphQL<MainSchema>("/graphql");

            // use graphiQL middleware at default url /graphiql
            //app.UseGraphiQLServer(new GraphiQLOptions());

            // use graphql-playground middleware at default url /ui/playground
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());

            // use GraphQLI middleware at default url /graphql
            //app.UseGraphiQl();

            // use voyager middleware at default url /ui/voyager
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions());

            app.UseHttpsRedirection();
            app.UseAuthentication();

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
