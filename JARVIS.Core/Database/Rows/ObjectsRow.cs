using System;
namespace JARVIS.Core.Database.Rows
{
    /// <summary>
    /// A settings row
    /// </summary>
    public class ObjectsRow
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
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Rows.ObjectsRow"/> class.
        /// </summary>
        public ObjectsRow()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Rows.ObjectsRow"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="newValue">Value</param>
        public ObjectsRow(string name, string newValue)
        {
            Name = name;
            Value = newValue;
        }
    }
}
