using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Arithmetic.Evaluation.Server
{
    /// <summary>
    /// Handles asynchronous connections from n number of TcpClients. Processess the request to evaluate an arithmetic expression and return the result.
    /// </summary>
    public class ArithmeticEvaluationServer
    {
        private readonly Serilog.ILogger logger = Log.ForContext<ArithmeticEvaluationServer>();

        private TcpListener listener;

        private bool continueListening = true;

        public ArithmeticEvaluationServer(IPAddress address, int port)
        {
            listener = new TcpListener(address, port);
            Console.CancelKeyPress += delegate
            {
                Stop();
            };
        }


        /// <summary>
        /// Start listening on the specified address and port and handle new connections as the arrive.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            logger.Information($"Starting listening on {listener.LocalEndpoint}");
            listener.Start();

            while (continueListening)
            {
                var client = await listener.AcceptTcpClientAsync();
                logger.Debug($"Client connected: {client.Client.RemoteEndPoint}");
                await ProcessConnection(client);
                //logger.Debug($"Finished processing {client.Client.RemoteEndPoint}");
                client.Close();
            }
        }

        /// <summary>
        /// Stops the listening loop and stops the TcpListener object
        /// </summary>
        public void Stop()
        {
            continueListening = false;
            listener.Stop();
        }

        /// <summary>
        /// When a connection is made, asynchronously process the request and send the response.
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        private Task ProcessConnection(TcpClient tcpClient)
        {
            // Async lambda to take advantage of the async APIs to be able to await the reading and writing between the connected client
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

        /// <summary>
        /// Utilize the DataTable.Compute method to evaluate the expression and return the result.
        /// If an exception occurs, log out and return Double.NaN
        /// </summary>
        /// <remarks> 
        /// </remarks>
        /// <param name="expression"></param>
        /// <returns>Result of the expression as a string</returns>
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
