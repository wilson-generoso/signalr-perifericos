using Microsoft.AspNetCore.SignalR;
using SignalR.Calculadora.Comum;
using SignalR.Comum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.Hub
{
    public class HubCalculadora : HubBase<HubCalculadoraController>
    {
        public HubCalculadora(HubManager manager) : base(manager)
        {
        }

        public async Task Simular(string modelo, double valor, double taxaJuros)
        {
            var simulacao = new Simulacao(modelo, valor, taxaJuros);

            await Controller.Simular(simulacao.ToParameterValue(Context.ConnectionId));
        }
    }
}
