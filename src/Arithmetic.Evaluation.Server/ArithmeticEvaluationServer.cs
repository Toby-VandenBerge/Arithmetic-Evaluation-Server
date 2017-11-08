using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Arithmetic.Evaluation.Server
{
    public class ArithmeticEvaluationServer
    {
        private readonly Serilog.ILogger logger = Log.ForContext<ArithmeticEvaluationServer>();

        private TcpListener listener;

        public ArithmeticEvaluationServer(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
        }

        public async Task StartAsync()
        {
            logger.Information($"Starting listening on {listener.LocalEndpoint}");
            listener.Start();

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                logger.Debug($"Client connected: {client.Client.RemoteEndPoint}");
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
                    logger.Debug($"Request: {request}");

                    string evaluation = Evaluate(request);
                    byte[] writeBuffer = Encoding.UTF8.GetBytes(evaluation);
                    await networkStream.WriteAsync(writeBuffer, 0, writeBuffer.Length);
                    logger.Debug($"{request} = {evaluation}");
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
                logger.Error(e.Message);
                return Double.NaN.ToString();
            }
        }
    }
}
