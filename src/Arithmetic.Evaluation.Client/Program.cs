using System;
using Microsoft.Extensions.Configuration;
using Arithmetic.Evaluation.Shared.Extensions;

namespace Arithmetic.Evaluation.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = ConfigurationRootFactory.Create();
            string evaluationServerIp = configuration.GetSection("EvaluationServerIp").Value;
            int evaluationServerPort = Convert.ToInt32(configuration.GetSection("EvaluationServerPort").Value);
            SerilogExtensions.InitializeSerilog(configuration);

            ArithmeticEvaluationClient client = new ArithmeticEvaluationClient(evaluationServerIp, evaluationServerPort);
            client.Start().Wait();
        }
    }
}
