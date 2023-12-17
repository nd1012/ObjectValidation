using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item range attribute
    /// </summary>
    public class ItemRangeAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        /// <param name="target">Validation target</param>
        public ItemRangeAttribute(double min, double max, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new RangeAttribute(min, max)) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        /// <param name="target">Validation target</param>
        public ItemRangeAttribute(int min, int max, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new RangeAttribute(min, max)) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="min">Minimum</param>
        /// <param name="max">Maximum</param>
        /// <param name="target">Validation target</param>
        public ItemRangeAttribute(Type type, string min, string max, ItemValidationTargets target = ItemValidationTargets.Item) : base(target, new RangeAttribute(type, min, max)) { }

        /// <summary>
        /// Range attribute
        /// </summary>
        private RangeAttribute RangeAttribute => ValidationAttribute as RangeAttribute ?? throw new InvalidProgramException();

        /// <summary>
        /// Specifies whether validation should fail for values that are equal to <see cref="RangeAttribute.Maximum" />.
        /// </summary>
        public bool MaximumIsExclusive
        {
            get => RangeAttribute.MaximumIsExclusive;
            set => RangeAttribute.MaximumIsExclusive = value;
        }

        /// <summary>
        /// Specifies whether validation should fail for values that are equal to <see cref="RangeAttribute.Minimum" />.
        /// </summary>
        public bool MinimumIsExclusive
        {
            get => RangeAttribute.MinimumIsExclusive;
            set => RangeAttribute.MinimumIsExclusive = value;
        }

        /// <summary>
        /// Determines whether string values for <see cref="RangeAttribute.Minimum" /> and <see cref="RangeAttribute.Maximum" /> are parsed using the invariant culture rather than the current culture.
        /// </summary>
        public bool ParseLimitsInInvariantCulture
        {
            get => RangeAttribute.ParseLimitsInInvariantCulture;
            set => RangeAttribute.ParseLimitsInInvariantCulture = value;
        }

        /// <summary>
        /// Determines whether any conversions of the value being validated to <see cref="System.ComponentModel.DataAnnotations.RangeAttribute.OperandType" /> as set by the type parameter of the 
        /// <see cref="RangeAttribute" /> constructor use the invariant culture or the current culture.
        /// </summary>
        public bool ConvertValueInInvariantCulture
        {
            get => RangeAttribute.ConvertValueInInvariantCulture;
            set => RangeAttribute.ConvertValueInInvariantCulture = value;
        }
    }
}
