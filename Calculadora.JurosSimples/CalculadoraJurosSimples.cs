using SignalR.Calculadora.Comum;
using SignalR.Calculadora.Hub;
using SignalR.Comum;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.JurosSimples
{
    [Export(typeof(IPeriferico))]
    public class CalculadoraJurosSimples : PerifericoControllerBase<HubCalculadoraController>, ICalculadora
    {
        private const double IOF_FIXO = 0.0038;
        private const double IOF_DIARIO = 0.000082;

        public override string Modelo => "JurosSimples";

        public async Task EnviarResultado(ParameterValue<ResultadoSimulacao> value)
        {
            await Controller.EnviarResultado(value);
        }

        public async Task Simular(ParameterValue<Simulacao> value)
        {
            var simulacao = value.Value;

            for (int qtde = 1; qtde <= 12; qtde++)
            {
                var iofNoPeriodo = ((qtde * 30) * IOF_DIARIO) + IOF_FIXO;
                var valorComIof = simulacao.Valor / (1 - iofNoPeriodo);
                var parcelaComIof = valorComIof / qtde;
                var parcela = simulacao.Valor / qtde;
                var taxaDiaria = simulacao.TaxaJuros / 30;

                var valorParcela = Math.Round((((taxaDiaria * (qtde * 30)) / 100) * parcela) + parcelaComIof, 2);
                var valorTotal = Math.Round(valorParcela * qtde, 2);
                var juros = Math.Round(valorTotal - simulacao.Valor, 2);
                var resultadoSimulacao = new ResultadoSimulacao(qtde, valorParcela, valorTotal, juros, Math.Round(iofNoPeriodo*100, 2));

                await EnviarResultado(resultadoSimulacao.ToParameterValue(value.Context.ConnectionId));
            }

            await SinalizarFim(new ParameterValue(value.Context.ConnectionId));
        }

        public async Task SinalizarFim(ParameterValue value)
        {
            await Controller.SinalizarFim(value);
        }
    }
}
