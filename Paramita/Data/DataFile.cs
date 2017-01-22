using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Paramita.Data
{
    public class DataFile
    {
        protected List<string> txtFileLines = new List<string>();



        public DataFile(string resourcePath)
        {
            txtFileLines = LoadLinesFrom(resourcePath);
        }



        public List<string> LoadLinesFrom(string resourcePath)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
            TextReader reader = new StreamReader(stream);

            List<string> lines = new List<string>();
            string dataLine;
            while ((dataLine = reader.ReadLine()) != null)
            {
                lines.Add(dataLine);
            }
            return lines;
        }
    }
}
