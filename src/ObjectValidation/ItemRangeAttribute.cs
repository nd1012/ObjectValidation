using System.ComponentModel.DataAnnotations;

//TODO .NET 8: Minimum/MaximumIsExclusive
//TODO .NET 8: (Item)LengthAttribute
//TODO .NET 8: (Item)Base64StringAttribute

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
    }
}
