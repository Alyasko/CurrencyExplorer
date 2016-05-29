using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CurrencyExplorer.Utilities;


namespace CurrencyExplorer.Models.Exporters
{
    public class JpegCurrencyExporter : AbstractCurrencyExporter<string>
    {
        public override void Export()
        {
            string outputFileName = Constants.TempImageFileName;

            //FileStream outputStream = File.OpenWrite(outputFileName); 

            Result = outputFileName;
        }
    }
}
