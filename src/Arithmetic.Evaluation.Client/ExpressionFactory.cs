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

        /// <summary>
        /// Create a random arithmetic expression.
        /// </summary>
        /// <returns>string in the format ## OPERAND ##</returns>
        public static string CreateRandomExpression()
        {
            return $"{random.Next(0, 101)} {operators[random.Next(0, operators.Count)]} {random.Next(0, 101)}";
        }
    }
}
