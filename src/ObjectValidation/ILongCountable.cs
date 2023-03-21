namespace wan24.ObjectValidation
{
    /// <summary>
    /// Long countable
    /// </summary>
    public interface ILongCountable : ICountable
    {
        /// <summary>
        /// Count
        /// </summary>
        long LongCount { get; }
    }
}
