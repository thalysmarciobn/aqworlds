using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AQWEmulator.Xml
{
    public class XmlSettingsSerializer<T>
    {
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(T));

        public T Configuration { get; private set; }

        public bool Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                    using (var file = File.Create(path))
                    {
                        _xmlSerializer.Serialize(file, Activator.CreateInstance(typeof(T)));
                    }

                using (var reader = XmlReader.Create(path))
                {
                    Configuration = (T) _xmlSerializer.Deserialize(reader);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}