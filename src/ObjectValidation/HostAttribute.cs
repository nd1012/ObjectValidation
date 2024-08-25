using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace wan24.ObjectValidation
{
    /// <summary>
    /// Host name or IP address validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    public class HostAttribute() : ValidationAttributeBase()
    {
        /// <summary>
        /// If IPv4 addresses are allowed
        /// </summary>
        public bool AllowIPv4 { get; set; } = true;

        /// <summary>
        /// If IPv6 addresses are allowed
        /// </summary>
        public bool AllowIPv6 { get; set; } = true;

        /// <summary>
        /// Check if the hostname (DNS lookup) or IP address (ICMP) exists
        /// </summary>
        public bool CheckIfExists { get; set; }

        /// <summary>
        /// Ping timeout in ms
        /// </summary>
        public int PingTimeout { get; set; } = 300;

        /// <inheritdoc/>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;
            if (value is not string str) return this.CreateValidationResult($"Hostname or IP address value as {typeof(string)} expected", validationContext);
            UriHostNameType type = Uri.CheckHostName(str);
            IPAddress? ip;
            switch (type)
            {
                case UriHostNameType.Dns:
                    if (!CheckIfExists) return null;
                    try
                    {
                        Dns.GetHostEntry(str);
                        return null;
                    }
                    catch (Exception ex)
                    {
                        return this.CreateValidationResult($"Hostname DNS lookup failed: {ex.Message ?? ex.GetType().ToString()}", validationContext);
                    }
                case UriHostNameType.IPv4:
                    if(!IPAddress.TryParse(str, out ip)) return this.CreateValidationResult($"Host IPv4 address parsing failed", validationContext);
                    if (ip.AddressFamily != AddressFamily.InterNetwork) return this.CreateValidationResult($"Detected host IPv4 address parsed to IPv6", validationContext);
                    break;
                case UriHostNameType.IPv6:
                    if (!IPAddress.TryParse(str, out ip)) return this.CreateValidationResult($"Host IPv6 address parsing failed", validationContext);
                    if (ip.AddressFamily != AddressFamily.InterNetworkV6) return this.CreateValidationResult($"Detected host IPv6 address parsed to IPv4", validationContext);
                    break;
                default:
                    return this.CreateValidationResult($"Hostname or IP address value invalid ({type})", validationContext);
            }
            if (!CheckIfExists) return null;
            using Ping ping = new();
            try
            {
                PingReply pong = ping.Send(ip, PingTimeout, RandomNumberGenerator.GetBytes(count: 32), new()
                {
                    DontFragment = true
                });
                return pong.Status == IPStatus.Success
                    ? null
                    : this.CreateValidationResult($"Ping to {ip} failed: {pong.Status}", validationContext);
            }
            catch(Exception ex)
            {
                return this.CreateValidationResult($"Ping to {ip} failed exceptional: {ex.Message ?? ex.GetType().ToString()}", validationContext);
            }
        }
    }
}
