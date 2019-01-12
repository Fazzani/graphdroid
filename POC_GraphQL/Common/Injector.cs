﻿namespace POC_GraphQL.Common
{
    using GraphQL.Conventions;
    using System;
    using System.Reflection;

    internal sealed class Injector : IDependencyInjector
    {
        private readonly IServiceProvider _provider;

        public Injector(IServiceProvider provider)
        {
            _provider = provider;
        }

        public object Resolve(TypeInfo typeInfo) => _provider.GetService(typeInfo);
        public object Resolve(Type type) => _provider.GetService(type);
    }
}
