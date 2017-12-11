using System.Collections.Generic;
using System.Xml.Serialization;

namespace Proxima.FICS.Source.ConfigSubsystem
{
    /// <summary>
    /// Represents a container for the config items.
    /// </summary>
    public class ConfigContainer
    {
        /// <summary>
        /// Gets or sets the config items list.
        /// </summary>
        [XmlElement("Item")]
        public List<ConfigItem> Items { get; set; }
    }
}
