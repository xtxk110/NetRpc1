﻿using System;
using System.Threading.Tasks;
using DataContract;
using Microsoft.Extensions.Hosting;
using NetRpc;
using NetRpc.Grpc;

namespace Service
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var host = NManager.CreateHost(50001, null, new ContractParam<IServiceAsync, ServiceAsync>());
            await host.RunAsync();
        }
    }

    public class ServiceAsync : IServiceAsync
    {
        public async Task CallAsync(string s)
        {
            Console.WriteLine($"Receive: {s}");
        }
    }
}