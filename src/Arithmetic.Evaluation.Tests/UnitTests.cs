using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Arithmetic.Evaluation.Client;
using Arithmetic.Evaluation.Server;
using NUnit.Framework;

namespace Arithmetic.Evaluation.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private const string localhost = "127.0.0.1";
        private const int testPort = 1337;

        [SetUp]
        public void Setup()
        {
            var server = new ArithmeticEvaluationServer(IPAddress.Parse(localhost), testPort);
            server.StartAsync();
        }

        [Test]
        public async Task AdditionTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "1+1";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                Assert.AreEqual("2", response);
            }
        }

        [Test]
        public async Task SubtractionTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "1-1";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                Assert.AreEqual("0", response);
            }
        }

        [Test]
        public async Task MultiplicationTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "2*2";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                Assert.AreEqual("4", response);
            }
        }

        [Test]
        public async Task DivisionTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "9/3";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                Assert.AreEqual("3", response);
            }
        }

        [Test]
        public async Task ModulusTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "3%2";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                Assert.AreEqual("1", response);
            }
        }

        [Test]
        public async Task InfinityTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "1/0";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                double responseDouble = Double.Parse(response);
                Assert.IsTrue(double.IsInfinity(responseDouble));
            }
        }

        [Test]
        public async Task DivideByZeroTest()
        {
            var client = new ArithmeticEvaluationClient(localhost, testPort);

            string message = "1.0/0";
            using (TcpClient tcpClient = new TcpClient(localhost, testPort))
            {
                string response = await client.Send(tcpClient, message);
                double responseDouble = Double.Parse(response);
                Assert.IsNaN(responseDouble);
            }
        }
    }
}
