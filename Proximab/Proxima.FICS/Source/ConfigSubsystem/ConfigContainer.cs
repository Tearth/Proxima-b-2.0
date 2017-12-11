using System.Collections.Generic;
using System.Xml.Serialization;

namespace Proxima.FICS.Source.ConfigSubsystem
{
    public class ConfigContainer
    {
        [XmlElement("Item")]
        public List<ConfigItem> Items { get; set; }
    }
}
