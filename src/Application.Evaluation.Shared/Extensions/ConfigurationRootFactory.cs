using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Arithmetic.Evaluation.Shared.Extensions
{
    public static class ConfigurationRootFactory
    {
        public static IConfigurationRoot Create()
        {
            return new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
