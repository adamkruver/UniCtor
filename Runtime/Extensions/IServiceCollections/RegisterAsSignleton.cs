using UniCtor.Services;

namespace Sources.Extensions.IServiceCollections
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAsSingleton<TService1, TService2, TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TImplementation : class, TService1, TService2 =>
            services
                .RegisterAsSingleton<TImplementation>()
                .RegisterAsSingleton<TService1>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService2>(provider => provider.GetService<TImplementation>());

        public static IServiceCollection RegisterAsSingleton<TService1, TService2, TService3, TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TImplementation : class, TService1, TService2, TService3 =>
            services
                .RegisterAsSingleton<TImplementation>()
                .RegisterAsSingleton<TService1>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService2>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService3>(provider => provider.GetService<TImplementation>());

        public static IServiceCollection RegisterAsSingleton<TService1, TService2, TService3, TService4,
            TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TImplementation : class, TService1, TService2, TService3, TService4 =>
            services
                .RegisterAsSingleton<TImplementation>()
                .RegisterAsSingleton<TService1>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService2>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService3>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService4>(provider => provider.GetService<TImplementation>());

        public static IServiceCollection RegisterAsSingleton<TService1, TService2, TService3, TService4, TService5,
            TImplementation>(
            this IServiceCollection services
        )
            where TService1 : class
            where TService2 : class
            where TService3 : class
            where TService4 : class
            where TService5 : class
            where TImplementation : class, TService1, TService2, TService3, TService4, TService5 =>
            services
                .RegisterAsSingleton<TImplementation>()
                .RegisterAsSingleton<TService1>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService2>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService3>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService4>(provider => provider.GetService<TImplementation>())
                .RegisterAsSingleton<TService5>(provider => provider.GetService<TImplementation>());
    }
}