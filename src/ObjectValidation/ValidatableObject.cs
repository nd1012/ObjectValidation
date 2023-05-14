namespace wan24.ObjectValidation
{
    /// <summary>
    /// Base class for a validatable object
    /// </summary>
    [Obsolete("Use ValidatableObjectBase instead")]
    public abstract class ValidatableObject : ValidatableObjectBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ValidatableObject() : base() { }
    }
}
