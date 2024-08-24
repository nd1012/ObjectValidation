namespace wan24.ObjectValidation
{
    /// <summary>
    /// Interface for the validation information object
    /// </summary>
    public interface IValidationInfo
    {
        /// <summary>
        /// Seen objects
        /// </summary>
        HashSet<object> Seen { get; }
        /// <summary>
        /// Current validation depth
        /// </summary>
        int CurrentDepth { get; set; }
        /// <summary>
        /// Array level
        /// </summary>
        int ArrayLevel { get; set; }
        /// <summary>
        /// Get a clone
        /// </summary>
        /// <returns>Clone</returns>
        IValidationInfo GetClone();
    }
}
