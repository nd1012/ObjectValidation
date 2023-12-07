using System.ComponentModel.DataAnnotations;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Item base64 string validation attribute
    /// </summary>
    public sealed class ItemBase64StringAttribute : ItemValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemBase64StringAttribute() : base(new Base64StringAttribute()) { }
    }
}
