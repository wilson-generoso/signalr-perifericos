using SignalR.Comum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.Comum
{
    public interface ICalculadora : IPeriferico
    {
        Task Simular(ParameterValue<Simulacao> value);

        Task EnviarResultado(ParameterValue<ResultadoSimulacao> value);

        Task SinalizarFim(ParameterValue value);
    }
}
