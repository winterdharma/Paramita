using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Paramita.GameLogic.Data
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
            var resourceNames = GetType().Assembly.GetManifestResourceNames();
            var stream = GetType().Assembly.GetManifestResourceStream(resourcePath);
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
