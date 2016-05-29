using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyExplorer.Models.Contracts
{
    public interface ILocalization
    {
        string MenuSettings { get; }
        string MenuExplore { get; }
        string MenuHelp { get; }
        string MenuAbout { get; }
        string SettingsTitle { get; }
        string SettingsInterfaceLanguage { get; }
        string SettingsHowMany { get; }
        string SettingsIn { get; }
        string SettingsApply { get; }
        string TodayToday { get; }
        string TodayCosts { get; }
        string TodayActualCurrencyOn { get; }
        string ExplorerBeginDate { get; }
        string ExplorerEndDate { get; }
        string ExplorerUpdateChart{ get; }
        string ExportAsImage { get; }
        string ExportAsTable { get; }
        string AboutTheProject { get; }
        string AboutTheProjectTitle { get; }
        string Help { get; }
        string HelpTitle { get; }
        string FooterAllRightsReserved { get; }
    }
}
