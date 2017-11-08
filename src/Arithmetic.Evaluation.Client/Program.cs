using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Arithmetic.Evaluation.Shared.Extensions;

namespace Arithmetic.Evaluation.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = ConfigurationRootFactory.Create();
            SerilogExtensions.InitializeSerilog(configuration);

            ArithmeticEvaluationClient client = new ArithmeticEvaluationClient();
            client.Start().Wait();
        }
    }
}
