using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arithmetic.Evaluation.Client
{
    public class ArithmeticEvaluationClient
    {
        public ArithmeticEvaluationClient()
        {
            
        }

        public async Task Start()
        {
            while (true)
            {
                try
                {
                    using (var client = new TcpClient("127.0.0.1", 1337))
                    {
                        await SendRequest(client);
                        await Task.Delay(500);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occurred: {e}");
                    Console.WriteLine("Waiting 5 seconds to attempt to connect again.");
                    await Task.Delay(5000);
                }
                
            }
        }

        private Task SendRequest(TcpClient client)
        {
            return Task.Run(async () =>
            {
                string randomExpression = ExpressionFactory.Create();

                using (var networkStream = client.GetStream())
                {
                    Console.WriteLine($"Sending: {randomExpression}");
                    byte[] writeBuffer = Encoding.ASCII.GetBytes(randomExpression);
                    await networkStream.WriteAsync(writeBuffer, 0, writeBuffer.Length);

                    var readBuffer = new byte[4096];
                    var byteCount = await networkStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                    var result = Encoding.UTF8.GetString(readBuffer, 0, byteCount);
                    Console.WriteLine($"Result: {result}");
                }
            });
        }
    }
}
