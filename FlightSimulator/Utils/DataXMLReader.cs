using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.Utils
{
    static class DataXMLReader
    {
        public static IList<String> getPlaneProperties()
        {
            StreamReader sr = new StreamReader("generic_small.xml");
            IList<String> planeProperties = new List<String>();
            String line = sr.ReadLine();
            while (line != null)
            {
                line = line.Trim();
                if (line.StartsWith("<name>") && line.EndsWith("</name>")) // name should be in one row.
                {
                    planeProperties.Add(line.Substring("<name>".Length, line.Length - "<name>".Length - "</name>".Length));
                }
                line = sr.ReadLine();
            }

            return planeProperties;
        }

        public static IList<String> getPlanePropertiesPaths()
        {
            StreamReader sr = new StreamReader("generic_small.xml");
            IList<String> planeProperties = new List<String>();
            String line = sr.ReadLine();
            while (line != null)
            {
                line = line.Trim();
                if (line.StartsWith("<node>") && line.EndsWith("</node>")) // name should be in one row.
                {
                    planeProperties.Add(line.Substring("<node>".Length, line.Length - "<node>".Length - "</node>".Length));
                }
                line = sr.ReadLine();
            }

            return planeProperties;
        }
    }
}
