using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using XeModelTest;

namespace CurrencyExplorer.Models.Localizations
{
    public class EnglishLocalization : AbstractLocalization
    {
        public EnglishLocalization(IApplicationEnvironment appEnv)
        {
            InitializeConfiguration("en.json", appEnv);
        }
    }
}
