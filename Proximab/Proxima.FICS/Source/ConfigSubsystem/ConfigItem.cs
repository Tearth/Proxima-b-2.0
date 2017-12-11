using System.Xml.Serialization;

namespace Proxima.FICS.Source.ConfigSubsystem
{
    /// <summary>
    /// Represents a single config item.
    /// </summary>
    public class ConfigItem
    {
        /// <summary>
        /// Gets or sets the config key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the config value.
        /// </summary>
        public string Value { get; set; }
    }
}
