namespace wan24.ObjectValidation
{
    /// <summary>
    /// Base class for a validatable object
    /// </summary>
    [Obsolete("Use ValidatableObjectBase instead (ValidatableObject will be removed in v2!)")]
    public abstract class ValidatableObject : ValidatableObjectBase //TODO Remove in v2
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected ValidatableObject() : base() { }
    }
}
