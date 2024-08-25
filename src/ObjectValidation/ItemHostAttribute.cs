namespace wan24.ObjectValidation
{
    /// <summary>
    /// Host name or IP address validation attribute
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="target">Validation target</param>
    public class ItemHostAttribute(ItemValidationTargets target = ItemValidationTargets.Item) : ItemValidationAttribute(target, new HostAttribute())
    {
        /// <summary>
        /// If IPv4 addresses are allowed
        /// </summary>
        public bool AllowIPv4
        {
            get => ((HostAttribute)ValidationAttribute).AllowIPv4;
            set => ((HostAttribute)ValidationAttribute).AllowIPv4 = value;
        }

        /// <summary>
        /// If IPv6 addresses are allowed
        /// </summary>
        public bool AllowIPv6
        {
            get => ((HostAttribute)ValidationAttribute).AllowIPv6;
            set => ((HostAttribute)ValidationAttribute).AllowIPv6 = value;
        }

        /// <summary>
        /// Check if the hostname (DNS lookup) or IP address (ICMP) exists
        /// </summary>
        public bool CheckIfExists
        {
            get => ((HostAttribute)ValidationAttribute).CheckIfExists;
            set => ((HostAttribute)ValidationAttribute).CheckIfExists = value;
        }

        /// <summary>
        /// Ping timeout in ms
        /// </summary>
        public int PingTimeout
        {
            get => ((HostAttribute)ValidationAttribute).PingTimeout;
            set => ((HostAttribute)ValidationAttribute).PingTimeout = value;
        }
    }
}
