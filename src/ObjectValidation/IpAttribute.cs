using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// IP address validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="family">Address family</param>
    public class IpAttribute(AddressFamily family = AddressFamily.InterNetwork) : ValidationAttribute
    {
        /// <summary>
        /// Address family
        /// </summary>
        public AddressFamily AddressFamily { get; } = family;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string ipStr)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"IP address value is not a {typeof(string)}" : $"{validationContext.MemberName}: IP address value is not a {typeof(string)}"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (!IPAddress.TryParse(ipStr, out IPAddress? ip))
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid IP address" : $"{validationContext.MemberName}: Invalid IP address"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            if (ip.AddressFamily != AddressFamily)
                return new(
                    ErrorMessage ?? (validationContext.MemberName is null ? $"Invalid IP address family ({AddressFamily} expected)" : $"{validationContext.MemberName}: Invalid IP address family ({AddressFamily} expected)"),
                    validationContext.MemberName is null ? null : new string[] { validationContext.MemberName }
                    );
            return null;
        }
    }
}
