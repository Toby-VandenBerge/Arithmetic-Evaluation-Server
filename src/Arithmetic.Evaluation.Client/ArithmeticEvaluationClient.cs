using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Arithmetic.Evaluation.Client
{
    public class ArithmeticEvaluationClient
    {
        private readonly Serilog.ILogger logger = Log.ForContext<ArithmeticEvaluationClient>();

        public ArithmeticEvaluationClient()
        {
            
        }

        public async Task Start()
        {
            logger.Information("Starting test client");
            while (true)
            {
                try
                {
                    using (var client = new TcpClient("127.0.0.1", 1337))
                    {
                        await Send(client, ExpressionFactory.CreateRandomExpression());
                        await Task.Delay(500);
                    }
                }
                catch (Exception e)
                {
                    logger.Error($"Error occurred: {e}");
                    logger.Debug("Waiting 5 seconds to attempt to connect again.");
                    await Task.Delay(5000);
                }
                
            }
        }

        public Task<string> Send(TcpClient client, string message)
        {
            return Task<string>.Run(async () =>
            {
                string response = string.Empty;
                using (var networkStream = client.GetStream())
                {
                    logger.Debug($"Sending: {message}");
                    byte[] writeBuffer = Encoding.ASCII.GetBytes(message);
                    await networkStream.WriteAsync(writeBuffer, 0, writeBuffer.Length);

                    var readBuffer = new byte[1024];
                    var byteCount = await networkStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    response = Encoding.UTF8.GetString(readBuffer, 0, byteCount);
                    logger.Debug($"Result: {response}");
                }
                return response;
            });
        }
    }
}
