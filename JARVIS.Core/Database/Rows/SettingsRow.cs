using System;
namespace JARVIS.Core.Database.Rows
{
    /// <summary>
    /// A settings row
    /// </summary>
    public class SettingsRow
    {
        /// <summary>
        /// The name field.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// The value field.
        /// </summary>
        public string Value = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Tables.Settings.SettingsRow"/> class.
        /// </summary>
        public SettingsRow()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Tables.Settings.SettingsRow"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="newValue">Value</param>
        public SettingsRow(string name, string newValue)
        {
            Name = name;
            Value = newValue;
        }
    }
}
