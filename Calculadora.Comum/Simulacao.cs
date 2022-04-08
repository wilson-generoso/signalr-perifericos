using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Calculadora.Comum
{
    public struct Simulacao
    {
        public Simulacao(string modelo, double valor, double taxaJuros)
        {
            Modelo = modelo;
            Valor = valor;
            TaxaJuros = taxaJuros;
        }

        public string Modelo { get; }

        public double Valor { get; }

        public double TaxaJuros { get; }
    }
}
