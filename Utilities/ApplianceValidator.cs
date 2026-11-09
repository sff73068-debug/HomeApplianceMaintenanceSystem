using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Text.RegularExpressions;

namespace HomeApplianceMaintenanceSystem.Utilities
{
    /// <summary>
    /// Static utility class providing validation rules for appliance and customer inputs.
    /// Fulfils the Static Classes & Members requirement.
    /// </summary>
    public static class ApplianceValidator
    {
        // Validates serial numbers following standard format: XXX-YYYY-NNNN (e.g., REF-2026-1001)
        private static readonly Regex SerialRegex = new Regex(@"^[A-Z]{3}-\d{4}-\d{4}$");

        // Validates Syrian/international mobile numbers (10 digits starting with 09)
        private static readonly Regex PhoneRegex = new Regex(@"^09\d{8}$");

        public static bool IsValidSerialNumber(string serialNumber)
        {
            return !string.IsNullOrWhiteSpace(serialNumber) && SerialRegex.IsMatch(serialNumber.Trim());
        }

        public static bool IsValidReceivedDate(DateTime receivedDate)
        {
            // Received date cannot be in the future or older than 5 years
            return receivedDate.Date <= DateTime.Now.Date && receivedDate.Year >= 2021;
        }

        public static bool IsValidDeliveryDate(DateTime receivedDate, DateTime expectedDeliveryDate)
        {
            return expectedDeliveryDate.Date >= receivedDate.Date;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && PhoneRegex.IsMatch(phoneNumber.Trim());
        }

        public static bool IsValidCost(decimal amount)
        {
            return amount >= 0m;
        }

        public static bool IsValidText(string value, int minLength = 2)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Trim().Length >= minLength;
        }
    }
}