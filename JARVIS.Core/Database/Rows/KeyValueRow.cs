namespace JARVIS.Core.Database.Rows
{
    /// <summary>
    /// A generic KeyValue row.
    /// </summary>
    public class KeyValueRow<T>
    {
        /// <summary>
        /// The name field.
        /// </summary>
        public string Name;

        /// <summary>
        /// The value field.
        /// </summary>
        public T Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Rows.KeyValueRow"/> class.
        /// </summary>
        public KeyValueRow()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:JARVIS.Core.Database.Rows.KeyValueRow"/> class.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="newValue">Value</param>
        public KeyValueRow(string name, T newValue)
        {
            Name = name;
            Value = newValue;
        }
    }
}
