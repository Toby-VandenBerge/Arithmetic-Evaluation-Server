using System;
using System.Collections.Generic;
using System.Text;

namespace Arithmetic.Evaluation.Client
{
    public static class ExpressionFactory
    {
        private static Random random = new Random();
        private static List<string> operators = new List<string>()
        {
            "+",
            "-",
            "*",
            "/",
            "%"
        };

        public static string CreateRandomExpression()
        {
            return $"{random.Next(0, 101)} {operators[random.Next(0, operators.Count)]} {random.Next(0, 101)}";
        }
    }
}
