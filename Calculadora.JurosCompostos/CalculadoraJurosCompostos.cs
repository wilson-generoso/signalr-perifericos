using SignalR.Calculadora.Comum;
using SignalR.Calculadora.Hub;
using SignalR.Comum;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.JurosCompostos
{
    [Export(typeof(ICalculadora))]
    public class CalculadoraJurosCompostos : PerifericoControllerBase<HubCalculadoraController>, ICalculadora
    {
        private const double IOF_FIXO = 0.0038;
        private const double IOF_DIARIO = 0.000082;

        public override string Modelo => "JurosCompostos";

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
                var valorTotal = 0D;
                var juros = 0D;

                for (int j = 1; j <= qtde; j++)
                {
                    var taxaPeriodo = (((qtde - j + 1) * 30) * taxaDiaria);
                    var jurosParcela = parcela * (taxaPeriodo / 100);

                    var valorParcelaPeriodo = parcelaComIof + jurosParcela;
                    juros += jurosParcela;
                    valorTotal += valorParcelaPeriodo;
                }

                var valorParcela = Math.Round(valorTotal / qtde, 2);

                var resultadoParcela = new ResultadoSimulacao(qtde, valorParcela, Math.Round(valorTotal,2), Math.Round(juros,2), Math.Round(iofNoPeriodo*100,2));
                await EnviarResultado(resultadoParcela.ToParameterValue(value.Context.ConnectionId));
            }

            await SinalizarFim(new ParameterValue(value.Context.ConnectionId));
        }

        public async Task SinalizarFim(ParameterValue value)
        {
            await Controller.SinalizarFim(value);
        }
    }
}

