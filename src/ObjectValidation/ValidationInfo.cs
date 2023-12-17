namespace wan24.ObjectValidation
{
    /// <summary>
    /// Validation depth
    /// </summary>
    internal sealed class ValidationInfo : IValidationInfo
    {
        /// <summary>
        /// Current validation depth
        /// </summary>
        private int _CurrentDepth = -1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="seen">Seen objects</param>
        internal ValidationInfo(List<object>? seen = null) => Seen = seen ?? [];

        /// <inheritdoc/>
        public List<object> Seen { get; }

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
