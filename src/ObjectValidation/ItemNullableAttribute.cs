namespace wan24.ObjectValidation
{
    /// <summary>
    /// Attribute for nullable items
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ItemNullableAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemNullableAttribute() : base() { }

        /// <summary>
        /// Target array level
        /// </summary>
        public int ArrayLevel { get; set; }
    }
}
