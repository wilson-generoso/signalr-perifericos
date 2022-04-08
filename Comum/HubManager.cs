using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Comum;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public sealed class HubManager : IAsyncDisposable
    {
        private ConcurrentDictionary<Type, IHubController> controllers = new ConcurrentDictionary<Type, IHubController>();

        public void Initialize(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var plugins = new ImportPlugin<IHubController>(configuration["HubFolder"]);

            var taskList = new List<Task>();

            foreach (var plugin in plugins.Plugins)
                taskList.Add(InitializeController(plugin.Value, configuration, serviceProvider));

            Task.WhenAll(taskList).Wait();
        }

        public void LoadRoutes(IEndpointRouteBuilder routeBuilder)
        {
            foreach(var controller in controllers.Values)
                controller.ConfigureRoute(routeBuilder);
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var controller in controllers.Values)
                await controller.DisposeAsync();
        }

        internal TController GetController<TController>() where TController : IHubController
        {
            if (controllers.TryGetValue(typeof(TController), out var controller))
                return (TController)controller;
            else
                return default(TController);
        }

        private async Task InitializeController(IHubController controller, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            await controller.Initialize(configuration, serviceProvider);

            while (!controllers.TryAdd(controller.GetType(), controller))
                await Task.Delay(1000);
        }
    }
}
