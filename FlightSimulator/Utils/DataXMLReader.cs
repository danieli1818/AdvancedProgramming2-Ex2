using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The FlightSimulator.Utils Namespace of the Utils.
/// </summary>
namespace FlightSimulator.Utils
{
    /// <summary>
    /// The DataXMLReader static class which reads the generic_small.xml file.
    /// </summary>
    static class DataXMLReader
    {
        /// <summary>
        /// The getPlaneProperties function reads from the generic_small.xml
        /// file and returns a List of all the plane properties names in the order of them
        /// in the file.
        /// <retValue>Ordered List of the plane names from the generic_small.xml file.</retValue>
        /// </summary>
        public static IList<String> getPlaneProperties()
        {
            String path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Utils\Files\generic_small.xml");
            StreamReader sr = new StreamReader(path);
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

        /// <summary>
        /// The getPlanePropertiesPaths function
        /// reads the generic_small.xml file and returns a List of the Plane Properties Paths
        /// in the order like in the file.
        /// <retValue>Ordered List of the Plane Properties Paths like in the generic_small.xml file.</retValue>
        /// </summary>
        public static IList<String> getPlanePropertiesPaths()
        {
            String path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Utils\Files\generic_small.xml");
            StreamReader sr = new StreamReader(path);
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
