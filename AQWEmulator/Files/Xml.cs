using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Utils.Exceptions;

namespace Utils.Files
{
    public static class Xml
    {
        public static bool IsValidXml(string xmlString)
        {
            try
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static IEnumerable<XElement> Values(string filePath, string descendants)
        {
            if (!File.Exists(filePath)) throw new FileNotFound("Configuration file not found.");
            var data = File.ReadAllText(filePath);
            if (!IsValidXml(data)) throw new FileNotFound("Configuration file not found.");
            return XDocument.Load(filePath).Descendants(descendants);
        }

        public static XmlDocument Load(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFound("Configuration file not found.");
            var doc = new XmlDocument();
            doc.Load(filePath);
            return doc;
        }

        public static XmlNode SelectSingleNode(string xml, string nodes)
        {
            var _xml = new XmlDocument();
            _xml.LoadXml(xml);
            return _xml.SelectSingleNode(nodes);
        }

        public static XmlNodeList SelectNodes(string xml, string nodes)
        {
            var _xml = new XmlDocument();
            _xml.LoadXml(xml);
            return _xml.SelectNodes(nodes);
        }

        public static string ToString(XmlNodeList nodeList)
        {
            var data = "";
            foreach (XmlNode xn in nodeList) data += xn.InnerText;
            return data;
        }
    }
}