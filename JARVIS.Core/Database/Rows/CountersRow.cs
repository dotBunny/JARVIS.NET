using System;
namespace JARVIS.Core.Database.Rows
{
    /// <summary>
    /// A counters row.
    /// </summary>
    public class CountersRow
    {
        /// <summary>
        /// The name field.
        /// </summary>
        public string Name;

        /// <summary>
        /// The value field.
        /// </summary>
        public int Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Rows.CountersRow"/> class.
        /// </summary>
        public CountersRow()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Rows.CountersRow"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="newValue">Value</param>
        public CountersRow(string name, int newValue)
        {
            Name = name;
            Value = newValue;
        }
    }
}
