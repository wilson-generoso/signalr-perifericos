using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SignalR.Calculadora.Comum;
using SignalR.Comum;

namespace SignalR.Calculadora.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");

            var threadState = new ManualResetEvent(false);

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/calculadora")
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .WithAutomaticReconnect()
                .Build();

            await connection.StartAsync();

            Console.WriteLine("Starting connection. Press Ctrl-C to close.");
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, a) =>
            {
                a.Cancel = true;
                cts.Cancel();
                threadState.Set();
            };

            connection.Closed += e =>
            {
                Console.WriteLine("Connection closed with error: {0}", e);

                cts.Cancel();
                return Task.CompletedTask;
            };

            connection.On<string>("resultadoSimulado", (content) =>
            {
                var resultado = JsonConvert.DeserializeObject<ResultadoSimulacao>(content);
                Console.WriteLine($"Parcelas: {resultado.Parcelas}, Valor da parcela: {resultado.ValorParcela}, Valor total: {resultado.ValorTotal}, Juros: {resultado.Juros}, IOF: {resultado.Iof}% ");
            });

            connection.On("sinalizarFim", () =>
            {
                threadState.Set();
            });

            try
            {
                while (!cts.IsCancellationRequested)
                {
                    threadState.Reset();

                    var modelo = await ObterModelo(cts);
                    var valor = await ObterValorFinanciamento(cts);
                    var taxa = await ObterTaxaJuros(cts);

                    if (!cts.IsCancellationRequested)
                        await connection.InvokeAsync("Simular", modelo, valor, taxa, CancellationToken.None);

                    if(!cts.IsCancellationRequested)
                        threadState.WaitOne();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static Task<string> ObterModelo(CancellationTokenSource cts)
        {
            bool valid;
            var modelo = "";

            if (cts.IsCancellationRequested)
                return Task.FromResult("");

            do
            {
                valid = true;

                Console.WriteLine("Digite o tipo de juros que deseja simular? (S = Simples, C = Composto)");
                modelo = Console.ReadLine();

                if (cts.IsCancellationRequested)
                    return Task.FromResult("");

                if (modelo.Trim().Length == 0)
                {
                    Console.WriteLine("Valor inválido");
                    valid = false;
                }
                else
                {
                    if ("SCsc".IndexOf(modelo[0]) < 0)
                    {
                        Console.WriteLine("Digite C ou S");
                        valid = false;
                    }
                    else
                        modelo = modelo.ToUpper()[0] == 'C' ? "JurosCompostos" : "JurosSimples";
                }

            }
            while (!valid);

            return Task.FromResult(modelo);
        }

        private static Task<double> ObterValorFinanciamento(CancellationTokenSource cts)
        {
            bool valid;
            var valorFinanciamento = 0D;

            if (cts.IsCancellationRequested)
                return Task.FromResult(0D);

            do
            {
                valid = true;

                Console.WriteLine("Digite o valor do financiamento: (Decimal com ponto. Ex: 1500.00)");
                var valor = Console.ReadLine();

                if (cts.IsCancellationRequested)
                    return Task.FromResult(0D);

                if (valor.Trim().Length == 0 || !double.TryParse(valor, out valorFinanciamento))
                {
                    Console.WriteLine("Valor inválido");
                    valid = false;
                }
            }
            while (!valid);

            return Task.FromResult(valorFinanciamento);
        }

        private static Task<double> ObterTaxaJuros(CancellationTokenSource cts)
        {
            bool valid;
            var juros = 0D;

            if (cts.IsCancellationRequested)
                return Task.FromResult(0D);

            do
            {
                valid = true;

                Console.WriteLine("Digite a taxa de juros: (Decimal com ponto. Ex: 1500.00)");
                var valor = Console.ReadLine();

                if (cts.IsCancellationRequested)
                    return Task.FromResult(0D);

                if (valor.Trim().Length == 0 || !double.TryParse(valor, out juros))
                {
                    Console.WriteLine("Valor inválido");
                    valid = false;
                }
            }
            while (!valid);

            return Task.FromResult(juros);
        }
    }
}
