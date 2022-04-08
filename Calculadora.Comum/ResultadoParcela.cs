using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.Comum
{
    public struct ResultadoSimulacao
    {
        public ResultadoSimulacao(int parcelas, double valorParcela, double valorTotal, double juros, double iof)
        {
            Parcelas = parcelas;
            ValorParcela = valorParcela;
            ValorTotal = valorTotal;
            Juros = juros;
            Iof = iof;
        }

        public double ValorParcela { get; }

        public int Parcelas { get; }

        public double ValorTotal { get; }

        public double Juros { get; }

        public double Iof { get; }
    }

}
