﻿namespace POC_GraphQL.Common
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using GraphQL.Authorization;
    using GraphQL.Validation;

    public static class GraphQLAuthExtensions
    {
        public static IServiceCollection AddGraphQLAuth(this IServiceCollection services, Action<AuthorizationSettings> configure)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();
                configure(authSettings);
                return authSettings;
            });
            return services;
        }
    }
}
