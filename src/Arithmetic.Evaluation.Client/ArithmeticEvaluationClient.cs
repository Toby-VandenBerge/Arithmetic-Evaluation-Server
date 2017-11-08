using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Arithmetic.Evaluation.Client
{
    /// <summary>
    /// Client application to send randomly generated arithmetic expressions to at TCP server and get the result back as a response
    /// </summary>
    public class ArithmeticEvaluationClient
    {
        private readonly Serilog.ILogger logger = Log.ForContext<ArithmeticEvaluationClient>();
        private bool continueRunning = true;
        private string evaluationServerIp;
        private int evaluationServerPort;

        public ArithmeticEvaluationClient(string evaluationServerIp, int evaluationServerPort)
        {
            this.evaluationServerIp = evaluationServerIp;
            this.evaluationServerPort = evaluationServerPort;
            // Listen for (Ctrl + c) or (Ctrl + break) to gracefully stop sending new requests
            Console.CancelKeyPress += delegate
            {
                continueRunning = false;
            };
        }

        /// <summary>
        /// Start sending requests until a CancelKeyPress event is fired. 
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            logger.Information("Starting test client");
            while (continueRunning)
            {
                try
                {
                    using (var client = new TcpClient(evaluationServerIp, evaluationServerPort))
                    {
                        await Send(client, ExpressionFactory.CreateRandomExpression());
                        await Task.Delay(500);
                    }
                }
                catch (Exception e)
                {
                    logger.Error($"Error occurred: {e.Message}");
                    logger.Debug("Waiting 5 seconds to attempt to connect again.");
                    await Task.Delay(5000);
                }
            }
        }

        /// <summary>
        /// Asynchronously send arithmetic expressions to be evaluated by the server. Awaits for the response
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns>Evaluation response</returns>
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
