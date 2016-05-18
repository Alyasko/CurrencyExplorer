using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer.Models.Localizations
{
    public class RussianLocalization : AbstractLocalization
    {
        public RussianLocalization(IApplicationEnvironment appEnv)
        {
            InitializeConfiguration("ru.json", appEnv);
        }
    }
}
