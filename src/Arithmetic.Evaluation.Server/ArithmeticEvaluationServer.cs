using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arithmetic.Evaluation.Server
{
    public class ArithmeticEvaluationServer
    {
        private TcpListener listener;

        public ArithmeticEvaluationServer(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
        }

        public async Task StartAsync()
        {
            Console.WriteLine($"Starting listening on {listener.LocalEndpoint}");
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");
                await ProcessConnection(client);

                client.Close();
            }
        }

        private Task ProcessConnection(TcpClient tcpClient)
        {
            return Task.Run(async () =>
            {
                using (var networkStream = tcpClient.GetStream())
                {
                    byte[] readBuffer = new byte[1024];
                    int byteCount = await networkStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    string request = Encoding.UTF8.GetString(readBuffer, 0, byteCount);
                    Console.WriteLine($"Request: {request}");

                    string formattedEvaluation = String.Format("{0:0.##}", Evaluate(request));
                    byte[] writeBuffer = Encoding.UTF8.GetBytes(formattedEvaluation);
                    await networkStream.WriteAsync(writeBuffer, 0, writeBuffer.Length);
                    Console.WriteLine($"{request} = {formattedEvaluation}");
                }
            });
        }

        public string Evaluate(string expression)
        {
            try
            {
                DataTable table = new DataTable();
                return table.Compute(expression, string.Empty).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Double.NaN.ToString();
            }
        }
    }
}
