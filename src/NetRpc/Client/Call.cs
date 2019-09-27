﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace NetRpc
{
    internal sealed class Call : ICall
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ContractInfo _contract;
        private readonly IOnceCallFactory _factory;
        private volatile int _timeoutInterval;
        private readonly ClientMiddlewareBuilder _middlewareBuilder;

        public Call(IServiceProvider serviceProvider, ContractInfo contract, IOnceCallFactory factory, int timeoutInterval)
        {
            _serviceProvider = serviceProvider;
            _contract = contract;
            _factory = factory;
            _timeoutInterval = timeoutInterval;

            if (serviceProvider != null)
            {
                var middlewareOptions = _serviceProvider.GetService<IOptions<ClientMiddlewareOptions>>().Value;
                _middlewareBuilder = new ClientMiddlewareBuilder(middlewareOptions, serviceProvider);
            }
        }

        public void Config(int timeoutInterval)
        {
            _timeoutInterval = timeoutInterval;
        }

        public Dictionary<string, object> AdditionHeader { get; set; }

        public async Task<object> CallAsync(MethodInfo methodInfo, Action<object> callback, CancellationToken token, Stream stream, params object[] args)
        {
            var call = _factory.Create(_contract, _timeoutInterval);
            await call.StartAsync();

            ClientContext context = new ClientContext(_serviceProvider, call,  methodInfo, callback, token, stream, args);

            //header
            var header = AdditionHeader;
            if (header != null && header.Count > 0)
            {
                foreach (var key in header.Keys) 
                    context.Header.Add(key, header[key]);
            }

            if (_middlewareBuilder != null)
            {
                await _middlewareBuilder.InvokeAsync(context);
                return context.Result;
            }

            //onceTransfer will dispose after stream translate finished in OnceCall.
            return await call.CallAsync(header, methodInfo, callback, token, stream, args);
        }
    }
}