using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Comum
{
    public abstract class HubControllerBase<THub, TPeriferico> : IHubController 
        where THub : Microsoft.AspNetCore.SignalR.Hub
        where TPeriferico : IPeriferico
    {
        protected IHubContext<THub> Context { get; private set; }

        protected Dictionary<string, TPeriferico> Perifericos { get; private set; }

        public abstract string Route { get; }

        public virtual Task Initialize(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            Context = serviceProvider.GetRequiredService<IHubContext<THub>>();

            var importPlugin = new ImportPlugin<TPeriferico>(configuration["ControladorFolder"]);

            Perifericos = new Dictionary<string, TPeriferico>();

            foreach(var plugin in importPlugin.Plugins)
            {
                var periferico = plugin.Value;

                if(periferico is IPerifericoController)
                    ((IPerifericoController)periferico).SetHubController(this);

                if (!Perifericos.TryAdd(periferico.Modelo, (TPeriferico)periferico))
                {
                    // TODO: Log
                }
            }

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var periferico in Perifericos?.Values)
                await periferico.DisposeAsync();
        }

        protected string ObterModelo(string modelo)
        {
            if (string.IsNullOrEmpty(modelo))
                return Perifericos.First().Value?.Modelo;
            else
                return modelo;
        }

        public void ConfigureRoute(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapHub<THub>(Route);
        }
    }
}
