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
            if (value is not string ipStr) return this.CreateValidationResult("IP address value is not a {typeof(string)}", validationContext);
            if (!IPAddress.TryParse(ipStr, out IPAddress? ip)) return this.CreateValidationResult("Invalid IP address", validationContext);
            if (ip.AddressFamily != AddressFamily) return this.CreateValidationResult($"Invalid IP address family ({AddressFamily} expected)", validationContext);
            return null;
        }
    }
}
