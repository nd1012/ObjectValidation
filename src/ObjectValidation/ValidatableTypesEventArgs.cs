namespace wan24.ObjectValidation
{
    /// <summary>
    /// Event arguments for the <see cref="ValidatableTypes.OnIsTypeValidatable"/> event
    /// </summary>
    public sealed class ValidatableTypesEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        public ValidatableTypesEventArgs(Type type) : base() => Type = type;

        /// <summary>
        /// Type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Is validatable?
        /// </summary>
        public bool IsValidatable { get; set; }
    }
}
