using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer.Utilities
{
    public static class Utils
    {
        public static string GetFormattedDateString(DateTime date)
        {
            return $"{date.Year:0000}{date.Month:00}{date.Day:00}";
        }

        public static IConfigurationRoot CreateConfiguration(IApplicationEnvironment appEnv)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }

}
