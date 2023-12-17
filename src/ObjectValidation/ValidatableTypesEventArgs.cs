namespace wan24.ObjectValidation
{
    /// <summary>
    /// Event arguments for the <see cref="ValidatableTypes.OnIsTypeValidatable"/> event
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="type">Type</param>
    public sealed class ValidatableTypesEventArgs(Type type) : EventArgs()
    {
        /// <summary>
        /// Type
        /// </summary>
        public Type Type { get; } = type;

        /// <summary>
        /// Is validatable?
        /// </summary>
        public bool IsValidatable { get; set; }
    }
}
