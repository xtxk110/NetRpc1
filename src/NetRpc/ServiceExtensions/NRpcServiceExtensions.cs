﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetRpc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NRpcServiceExtensions
    {
        #region Service

        public static IServiceCollection AddNRpcService(this IServiceCollection services)
        {
            services.TryAddSingleton<BusyFlag>();
            services.TryAddSingleton<RequestHandler>();
            services.TryAddSingleton<IActionExecutingContextAccessor, ActionExecutingContextAccessor>();
            return services;
        }

        public static IServiceCollection AddNRpcServiceContract(this IServiceCollection services, Type serviceType, Type implementationType,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            services.Configure<ContractOptions>(i => i.Contracts.Add(new Contract(serviceType, implementationType)));

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton(serviceType, implementationType);
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped(serviceType, implementationType);
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            return services;
        }

        public static IServiceCollection AddNRpcServiceContract(this IServiceCollection services, Type serviceType,
            Func<IServiceProvider, object> implementationFactory, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            services.Configure<ContractOptions>(i => i.Contracts.Add(new Contract(serviceType, implementationFactory)));

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton(serviceType, implementationFactory);
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped(serviceType, implementationFactory);
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient(serviceType, implementationFactory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            return services;
        }

        public static IServiceCollection AddNRpcServiceContract(this IServiceCollection services, Type serviceType, object implementationInstance)
        {
            services.Configure<ContractOptions>(i => i.Contracts.Add(new Contract(serviceType, implementationInstance.GetType())));
            services.AddSingleton(serviceType, implementationInstance);
            return services;
        }

        public static IServiceCollection AddNRpcServiceContract<TService, TImplementation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where TService : class
            where TImplementation : class, TService
        {
            services.AddNRpcServiceContract(typeof(TService), typeof(TImplementation), serviceLifetime);
            return services;
        }

        internal static List<Instance> GetContractInstances(this IServiceProvider serviceProvider, ContractOptions options)
        {
            return options.Contracts.ConvertAll(i => new Instance(i, serviceProvider.GetRequiredService(i.ContractInfo.Type)));
        }

        #endregion

        #region Service Middleware

        public static IServiceCollection AddNRpcMiddleware(this IServiceCollection services, Action<MiddlewareOptions> configureOptions)
        {
            if (configureOptions != null)
                services.Configure(configureOptions);
            return services;
        }

        public static IServiceCollection AddNRpcCallbackThrottling(this IServiceCollection services, int callbackThrottlingInterval)
        {
            services.Configure<MiddlewareOptions>(i => i.UseCallbackThrottling(callbackThrottlingInterval));
            return services;
        }

        public static IServiceCollection AddNRpcStreamCallBack(this IServiceCollection services, int progressCount)
        {
            services.Configure<MiddlewareOptions>(i => i.UseStreamCallBack(progressCount));
            return services;
        }

        #endregion

        #region Client

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static IServiceCollection AddNRpcClient(this IServiceCollection services, Action<NClientOption>? configureOptions = null,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            if (configureOptions != null)
                services.Configure(configureOptions);

            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IClientProxyFactory, ClientProxyFactory>();
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IClientProxyFactory, ClientProxyFactory>();
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient<IClientProxyFactory, ClientProxyFactory>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            services.TryAddSingleton<IActionExecutingContextAccessor, ActionExecutingContextAccessor>();
            services.TryAddSingleton<IOrphanClientProxyFactory, OrphanClientProxyFactory>();

            return services;
        }

        public static IServiceCollection AddNRpcClientContract<TService>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where TService : class
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IClientProxy<TService>, ClientProxy<TService>>();
                    services.TryAddSingleton(typeof(TService), p => p.GetService<IClientProxy<TService>>().Proxy);
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IClientProxy<TService>, ClientProxy<TService>>();
                    services.TryAddScoped(typeof(TService), p => p.GetService<IClientProxy<TService>>().Proxy);
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient<IClientProxy<TService>, ClientProxy<TService>>();
                    services.TryAddTransient(typeof(TService), p => p.GetService<IClientProxy<TService>>().Proxy);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            return services;
        }

        public static IServiceCollection AddNRpcClientContract<TService>(this IServiceCollection services, string optionsName,
            ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where TService : class
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton(typeof(IClientProxy<TService>), p => p.GetService<IClientProxyFactory>().CreateProxy<TService>(optionsName));
                    services.TryAddSingleton(typeof(TService), p => p.GetService<IClientProxyFactory>().CreateProxy<TService>(optionsName)!.Proxy);
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped(typeof(IClientProxy<TService>), p => p.GetService<IClientProxyFactory>().CreateProxy<TService>(optionsName));
                    services.TryAddScoped(typeof(TService), p => p.GetService<IClientProxyFactory>().CreateProxy<TService>(optionsName)!.Proxy);
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient(typeof(IClientProxy<TService>), p => p.GetService<IClientProxyFactory>().CreateProxy<TService>(optionsName));
                    services.TryAddTransient(typeof(TService), p => p.GetService<IClientProxyFactory>().CreateProxy<TService>(optionsName)!.Proxy);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            return services;
        }

        public static IServiceCollection AddNRpcClientByOnceCallFactory<TOnceCallFactoryImplementation>(this IServiceCollection services,
            Action<NClientOption>? configureOptions = null, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TOnceCallFactoryImplementation : class, IOnceCallFactory
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IOnceCallFactory, TOnceCallFactoryImplementation>();
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IOnceCallFactory, TOnceCallFactoryImplementation>();
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient<IOnceCallFactory, TOnceCallFactoryImplementation>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            services.AddNRpcClient(configureOptions, serviceLifetime);
            return services;
        }

        public static IServiceCollection AddNRpcClientByClientConnectionFactory<TClientConnectionFactoryImplementation>(this IServiceCollection services,
            Action<NClientOption>? configureOptions = null, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TClientConnectionFactoryImplementation : class, IClientConnectionFactory
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton<IClientConnectionFactory, TClientConnectionFactoryImplementation>();
                    break;
                case ServiceLifetime.Scoped:
                    services.TryAddScoped<IClientConnectionFactory, TClientConnectionFactoryImplementation>();
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient<IClientConnectionFactory, TClientConnectionFactoryImplementation>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime, null);
            }

            services.AddNRpcClient(configureOptions, serviceLifetime);
            return services;
        }

        #endregion
    }
}