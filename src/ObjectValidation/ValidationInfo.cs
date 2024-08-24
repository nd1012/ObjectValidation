namespace wan24.ObjectValidation
{
    /// <summary>
    /// Validation depth
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="seen">Seen objects</param>
    internal sealed class ValidationInfo(HashSet<object>? seen = null) : IValidationInfo
    {
        /// <summary>
        /// Current validation depth
        /// </summary>
        private int _CurrentDepth = -1;

        /// <inheritdoc/>
        public HashSet<object> Seen { get; } = seen ?? [];

        /// <inheritdoc/>
        public int CurrentDepth
        {
            get => _CurrentDepth;
            set
            {
                if (ValidationExtensions.MaxDepth > 0 && value > ValidationExtensions.MaxDepth)
                    throw new ObjectValidationException($"Max. validation depth of {ValidationExtensions.MaxDepth}´exceeded");
                _CurrentDepth = value;
            }
        }

        /// <inheritdoc/>
        public int ArrayLevel { get; set; }

        /// <inheritdoc/>
        public ValidationInfo GetClone() => new(Seen)
        {
            CurrentDepth = CurrentDepth,
            ArrayLevel = ArrayLevel
        };

        /// <inheritdoc/>
        IValidationInfo IValidationInfo.GetClone() => GetClone();
    }
}
