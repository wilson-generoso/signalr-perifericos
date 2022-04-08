using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SignalR.Calculadora.Comum;
using SignalR.Comum;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.Hub
{
    [Export(typeof(IHubController))]
    public class HubCalculadoraController : HubControllerBase<HubCalculadora, ICalculadora>, ICalculadora
    {
        public override string Route => "/calculadora";

        public string Modelo => "";

        public async Task EnviarResultado(ParameterValue<ResultadoSimulacao> value)
        {
            await Context.Clients.Client(value.Context.ConnectionId).SendAsync("resultadoSimulado", JsonConvert.SerializeObject(value.Value));
        }

        public async Task Simular(ParameterValue<Simulacao> value)
        {
            var modelo = ObterModelo(value.Value.Modelo);

            await base.Perifericos[modelo].Simular(value);
        }

        public async Task SinalizarFim(ParameterValue value)
        {
            await Context.Clients.Client(value.Context.ConnectionId).SendAsync("sinalizarFim");
        }
    }
}
