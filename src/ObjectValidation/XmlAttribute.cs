using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XML validation attribute
    /// </summary>
    public class XmlAttribute : ValidationAttributeBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XmlAttribute() : base() { }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string xml) return this.CreateValidationResult($"XML value as {typeof(string)} expected", validationContext);
            try
            {
                new XmlDocument().LoadXml(xml);
            }
            catch(Exception ex)
            {
                return this.CreateValidationResult($"Invalid XML value: {ex.Message}", validationContext);
            }
            return null;
        }
    }
}
