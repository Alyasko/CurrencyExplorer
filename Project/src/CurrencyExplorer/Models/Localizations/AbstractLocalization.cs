using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CurrencyExplorer.Models.Contracts;
using CurrencyExplorer.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace CurrencyExplorer.Models.Localizations
{
    public abstract class AbstractLocalization : ILocalization
    {
        protected IConfiguration Configuration;

        protected AbstractLocalization(string fileName, IApplicationEnvironment appEnv)
        {
            Configuration = Utils.CreateConfiguration(appEnv, $"Localizations//{fileName}");
        }

        protected string GetValueByPropertyName([CallerMemberName] string name = "")
        {
            return Configuration[name];
        }

        public virtual string MenuSettings
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string MenuExplore
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string MenuHelp
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string MenuAbout
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string SettingsTitle
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string SettingsInterfaceLanguage
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string SettingsHowMany
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string SettingsIn
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string SettingsApply
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string TodayToday
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string TodayCosts
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string TodayActualCurrencyOn
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string ExplorerBeginDate
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string ExplorerEndDate
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string ExplorerUpdateChart
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string ExportAsImage
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string ExportAsTable
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string AboutTheProject
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string Help
        {
            get { return GetValueByPropertyName(); }
        }

        public virtual string FooterAllRightsReserved
        {
            get { return GetValueByPropertyName(); }
        }
    }
}
