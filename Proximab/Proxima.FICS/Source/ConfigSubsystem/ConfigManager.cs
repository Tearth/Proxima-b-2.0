using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Proxima.FICS.Source.ConfigSubsystem
{
    public class ConfigManager
    {
        private ConfigContainer _configContainer;

        public ConfigManager(string filename)
        {
            var xmlReader = XmlReader.Create(filename);
            var xmlSerializer = new XmlSerializer(typeof(ConfigContainer), new XmlRootAttribute("FICSConfig"));

            _configContainer = (ConfigContainer)xmlSerializer.Deserialize(xmlReader);
        }

        public T GetValue<T>(string key)
        {
            var item = _configContainer.Items.FirstOrDefault(p => p.Key == key);
            if(item == null)
            {
                throw new KeyNotFoundException();
            }

            return (T)Convert.ChangeType(item.Value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
