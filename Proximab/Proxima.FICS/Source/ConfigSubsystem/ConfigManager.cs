using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Proxima.FICS.Source.ConfigSubsystem
{
    /// <summary>
    /// Represents a set of methods to manage project configuration.
    /// </summary>
    public class ConfigManager
    {
        private ConfigContainer _configContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManager"/> class.
        /// </summary>
        /// <param name="filename">The filename with configuration.</param>
        public ConfigManager(string filename)
        {
            var xmlReader = XmlReader.Create(filename);
            var xmlSerializer = new XmlSerializer(typeof(ConfigContainer), new XmlRootAttribute("FICSConfig"));

            _configContainer = (ConfigContainer)xmlSerializer.Deserialize(xmlReader);
        }

        /// <summary>
        /// Gets the value of the config item with the specified key.
        /// </summary>
        /// <typeparam name="T">The value type.</typeparam>
        /// <param name="key">The key of the config item.</param>
        /// <returns>The value of the config item.</returns>
        public T GetValue<T>(string key)
        {
            var item = _configContainer.Items.FirstOrDefault(p => p.Key == key);
            if (item == null)
            {
                throw new KeyNotFoundException();
            }

            return (T)Convert.ChangeType(item.Value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
