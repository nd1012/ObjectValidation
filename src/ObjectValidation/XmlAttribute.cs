using System.ComponentModel.DataAnnotations;
using System.Xml;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// XML validation attribute
    /// </summary>
    public class XmlAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public XmlAttribute() : base() { }

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return null;
            if (value is not string xml)
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"XML value as {typeof(string)} expected" : $"{validationContext.MemberName}: XML value as {typeof(string)} expected"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            try
            {
                new XmlDocument().LoadXml(xml);
            }
            catch(Exception ex)
            {
                return new(
                    ErrorMessage ?? (validationContext.MemberName == null ? $"Invalid XML value: {ex.Message}" : $"{validationContext.MemberName}: Invalid XML value: {ex.Message}"),
                    validationContext.MemberName == null ? null : new string[] { validationContext.MemberName }
                    );
            }
            return null;
        }
    }
}
