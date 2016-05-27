using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer.Utilities
{
    public static class Utils
    {
        public static string GetJsonFormattedDateString(DateTime date)
        {
            return $"{date.Year:0000}{date.Month:00}{date.Day:00}";
        }

        public static string GetClientFormattedDateString(DateTime date)
        {
            return GetClientFormattedDateString(date, "-");
        }

        public static string GetClientFormattedDateString(DateTime date, string delimiter)
        {
            return $"{date.Year:0000}{delimiter}{date.Month:00}{delimiter}{date.Day:00}";
        }

        public static IConfigurationRoot CreateConfiguration(IApplicationEnvironment appEnv, string fileName)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile(fileName)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public static ICollection<string> GetListFromConfiguration(IConfiguration configuration, string path)
        {
            ICollection<string> list = new List<string>();

            int i = 0;
            string entry = configuration[$"{path}:{i}"];
            while (entry != null)
            {
                list.Add(entry);
                i++;
                entry = configuration[$"{path}:{i}"];
            }

            return list;
        } 
    }

}
