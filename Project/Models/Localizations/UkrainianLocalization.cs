﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer.Models.Localizations
{
    public class UkrainianLocalization : AbstractLocalization
    {
        public UkrainianLocalization(IApplicationEnvironment appEnv)
        {
            InitializeConfiguration("ua.json", appEnv);
        }
    }
}
