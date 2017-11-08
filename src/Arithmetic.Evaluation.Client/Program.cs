using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Arithmetic.Evaluation.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ArithmeticEvaluationClient client = new ArithmeticEvaluationClient();
            client.Start().Wait();
        }
    }
}
