using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using HomeApplianceMaintenanceSystem.Enums;
using HomeApplianceMaintenanceSystem.Models;
using HomeApplianceMaintenanceSystem.Utilities;

namespace HomeApplianceMaintenanceSystem.Services
{
    /// <summary>
    /// Main manager class holding a polymorphic List<Appliance> and subscribing to maintenance events.
    /// Demonstrates Polymorphism, Encapsulation, and Event Handling.
    /// </summary>
    public class MaintenanceCenter
    {
        private readonly List<Appliance> _appliances;
        private readonly List<string> _eventAuditLog;
        private string _centerName = string.Empty;

        public string CenterName
        {
            get => _centerName;
            set
            {
                if (!ApplianceValidator.IsValidText(value, 3))
                    throw new ArgumentException("Center name must contain at least 3 characters.");
                _centerName = value.Trim();
            }
        }

        public IReadOnlyList<Appliance> Appliances => _appliances.AsReadOnly();
        public IReadOnlyList<string> EventAuditLog => _eventAuditLog.AsReadOnly();

        public MaintenanceCenter(string centerName)
        {
            CenterName = centerName;
            _appliances = new List<Appliance>();
            _eventAuditLog = new List<string>();
        }

        // Registers a derived appliance, prevents duplicate serials, and subscribes to its events
        public void RegisterAppliance(Appliance appliance)
        {
            if (appliance == null)
                throw new ArgumentNullException(nameof(appliance));

            foreach (Appliance existing in _appliances)
            {
                if (existing.SerialNumber.Equals(appliance.SerialNumber, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException(
                        $"Duplicate Serial Number '{appliance.SerialNumber}' is already registered.");
            }

            // Subscribe to appliance events (Delegates & Events Requirement)
            appliance.StatusChanged += OnApplianceStatusChanged;
            appliance.DeliveryDelayed += OnApplianceDeliveryDelayed;

            _appliances.Add(appliance);
            Console.WriteLine($"  [OK] Registered: [{appliance.SerialNumber}] {appliance.Brand} {appliance.Model} (Owner: {appliance.OwnerName})");
        }

        // Polymorphic calculation of total expected repair revenue across all appliance types
        public decimal CalculateTotalExpectedRevenue()
        {
            decimal totalRevenue = 0m;
            foreach (Appliance appliance in _appliances)
            {
                // Calls overridden CalculateRepairCost() dynamically based on runtime type
                totalRevenue += appliance.CalculateRepairCost();
            }
            return totalRevenue;
        }

        // Filters appliances by their current MaintenanceStatus
        public List<Appliance> GetAppliancesByStatus(MaintenanceStatus targetStatus)
        {
            List<Appliance> result = new List<Appliance>();
            foreach (Appliance appliance in _appliances)
            {
                if (appliance.Status == targetStatus)
                    result.Add(appliance);
            }
            return result;
        }

        // Checks all registered appliances against a reference date to detect delivery delays
        public void CheckAllForDelays(DateTime referenceDate)
        {
            foreach (Appliance appliance in _appliances)
            {
                appliance.CheckDeliveryDelay(referenceDate);
            }
        }

        // Polymorphic display of all appliances and their specialized technical diagnoses
        public void DisplayAllMaintenanceReports()
        {
            int index = 1;
            foreach (Appliance appliance in _appliances)
            {
                Console.WriteLine($"  {index}. {appliance.GetMaintenanceSummary()}");
                Console.WriteLine($"     Diagnosis: {appliance.DiagnoseIssue()}");
                index++;
            }
        }

        // Event Handler 1: Triggered automatically when any appliance changes status
        private void OnApplianceStatusChanged(
            Appliance sender,
            MaintenanceStatus oldStatus,
            MaintenanceStatus newStatus,
            string notificationMessage)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm}] {notificationMessage}";
            _eventAuditLog.Add(logEntry);

            Console.WriteLine($"  >> [EVENT: STATUS CHANGED] Device: {sender.SerialNumber} | {oldStatus} -> {newStatus}");

            // If repair is completed, send immediate pickup & invoice notification to customer
            if (newStatus == MaintenanceStatus.ReadyForDelivery)
            {
                decimal finalCost = sender.CalculateRepairCost();
                Console.WriteLine($"  >> [CUSTOMER SMS ALERT] Dear {sender.OwnerName} ({sender.OwnerPhone}): " +
                                  $"Your {sender.GetType().Name} is READY!");
                Console.WriteLine($"     Total Repair Cost Due: {finalCost:N0} SYP. Please visit the center for pickup.");
            }
        }

        // Event Handler 2: Triggered automatically when an appliance exceeds ExpectedDeliveryDate
        private void OnApplianceDeliveryDelayed(
            Appliance sender,
            int daysDelayed,
            string alertMessage)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm}] DELAY ALERT: {alertMessage}";
            _eventAuditLog.Add(logEntry);

            Console.WriteLine($"  >> [EVENT: DELIVERY DELAYED!] {alertMessage}");
            Console.WriteLine($"     Status automatically updated to [{sender.Status}].");
        }
    }
}