﻿using UniCtor.Services;

namespace Sources.Extensions.IServiceCollections
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAsScoped<TService1, TService2, TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2 =>
            services.RegisterAsScoped<TImplementation>()
                .RegisterAsScoped<TService1, TImplementation>()
                .RegisterAsScoped<TService2, TImplementation>();

        public static IServiceCollection RegisterAsScoped<TService1, TService2, TService3, TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3 =>
            services.RegisterAsScoped<TImplementation>()
                .RegisterAsScoped<TService1, TImplementation>()
                .RegisterAsScoped<TService2, TImplementation>()
                .RegisterAsScoped<TService3, TImplementation>();

        public static IServiceCollection RegisterAsScoped<TService1, TService2, TService3, TService4, TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TImplementation : class, TService1, TService2, TService3, TService4 =>
            services.RegisterAsScoped<TImplementation>()
                .RegisterAsScoped<TService1, TImplementation>()
                .RegisterAsScoped<TService2, TImplementation>()
                .RegisterAsScoped<TService3, TImplementation>()
                .RegisterAsScoped<TService4, TImplementation>();

        public static IServiceCollection RegisterAsScoped<TService1, TService2, TService3, TService4, TService5,
            TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TService5 : class
            where TImplementation : class, TService1, TService2, TService3, TService4, TService5 =>
            services.RegisterAsScoped<TImplementation>()
                .RegisterAsScoped<TService1, TImplementation>()
                .RegisterAsScoped<TService2, TImplementation>()
                .RegisterAsScoped<TService3, TImplementation>()
                .RegisterAsScoped<TService4, TImplementation>()
                .RegisterAsScoped<TService5, TImplementation>();
    }
}