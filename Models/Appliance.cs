using System;
using System.Collections.Generic;
using System.Text;

using System;
using HomeApplianceMaintenanceSystem.Enums;
using HomeApplianceMaintenanceSystem.Events;
using HomeApplianceMaintenanceSystem.Interfaces;
using HomeApplianceMaintenanceSystem.Utilities;

namespace HomeApplianceMaintenanceSystem.Models
{
    /// <summary>
    /// Abstract base class representing a generic home appliance in the maintenance center.
    /// Implements IMaintainable and enforces strict Encapsulation, Abstraction, Events & Static counters.
    /// </summary>
    public abstract class Appliance : IMaintainable
    {
        // 1. Private Encapsulated Fields (Read-Only fields cannot be modified after construction)
        private readonly string _serialNumber;
        private readonly DateTime _receivedDate;
        private string _brand;
        private string _model;
        private string _ownerName;
        private string _ownerPhone;
        private DateTime _expectedDeliveryDate;
        private decimal _baseInspectionFee;
        private MaintenanceStatus _status;

        // 2. Static Fields for Global Tracking
        private static int _totalAppliancesCount = 0;
        private static int _totalCompletedRepairsCount = 0;

        // 3. Events (Delegates & Events Requirement)
        public event ApplianceStatusChangedHandler StatusChanged;
        public event MaintenanceAlertHandler DeliveryDelayed;

        // 4. Static Read-Only Properties
        public static int TotalAppliancesCount => _totalAppliancesCount;
        public static int TotalCompletedRepairsCount => _totalCompletedRepairsCount;

        // 5. Public Encapsulated Properties
        // Read-Only Property after creation: SerialNumber cannot be altered once constructed
        public string SerialNumber => _serialNumber;

        // Read-Only Property after creation: ReceivedDate is fixed upon intake
        public DateTime ReceivedDate => _receivedDate;

        // Controlled Read-Only Property: Status can only be changed via UpdateStatus()
        public MaintenanceStatus Status => _status;

        public string Brand
        {
            get => _brand;
            set
            {
                if (!ApplianceValidator.IsValidText(value))
                    throw new ArgumentException("Brand name must contain at least 2 characters.");
                _brand = value.Trim();
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                if (!ApplianceValidator.IsValidText(value))
                    throw new ArgumentException("Model name must contain at least 2 characters.");
                _model = value.Trim();
            }
        }

        public string OwnerName
        {
            get => _ownerName;
            set
            {
                if (!ApplianceValidator.IsValidText(value, 3))
                    throw new ArgumentException("Owner name must contain at least 3 characters.");
                _ownerName = value.Trim();
            }
        }

        public string OwnerPhone
        {
            get => _ownerPhone;
            set
            {
                if (!ApplianceValidator.IsValidPhoneNumber(value))
                    throw new ArgumentException($"Invalid phone number '{value}'. Must match 09XXXXXXXX.");
                _ownerPhone = value.Trim();
            }
        }

        public DateTime ExpectedDeliveryDate
        {
            get => _expectedDeliveryDate;
            set
            {
                if (!ApplianceValidator.IsValidDeliveryDate(_receivedDate, value))
                    throw new ArgumentException("Expected delivery date cannot precede received date.");
                _expectedDeliveryDate = value;
            }
        }

        public decimal BaseInspectionFee
        {
            get => _baseInspectionFee;
            protected set
            {
                if (!ApplianceValidator.IsValidCost(value))
                    throw new ArgumentException("Base inspection fee cannot be negative.");
                _baseInspectionFee = value;
            }
        }

        // Constructor validating all sensitive fields before initializing the object
        protected Appliance(
            string serialNumber,
            string brand,
            string model,
            string ownerName,
            string ownerPhone,
            DateTime receivedDate,
            DateTime expectedDeliveryDate,
            decimal baseInspectionFee)
        {
            if (!ApplianceValidator.IsValidSerialNumber(serialNumber))
                throw new ArgumentException(
                    $"Serial Number '{serialNumber}' is invalid! Must match XXX-YYYY-NNNN.");

            if (!ApplianceValidator.IsValidReceivedDate(receivedDate))
                throw new ArgumentException("Received date is invalid or in the future.");

            _serialNumber = serialNumber.Trim();
            _receivedDate = receivedDate;

            Brand = brand;
            Model = model;
            OwnerName = ownerName;
            OwnerPhone = ownerPhone;
            ExpectedDeliveryDate = expectedDeliveryDate;
            BaseInspectionFee = baseInspectionFee;
            _status = MaintenanceStatus.Received;

            // Increment static counter of registered appliances
            _totalAppliancesCount++;
        }

        // Abstract method that every derived appliance class MUST override to compute repair cost
        public abstract decimal CalculateRepairCost();

        // Virtual diagnosis method overridden by subclasses
        public virtual string DiagnoseIssue()
        {
            return $"General inspection queued for {Brand} {Model} (SN: {SerialNumber}).";
        }

        // Updates appliance status safely and fires the StatusChanged event
        public void UpdateStatus(MaintenanceStatus newStatus)
        {
            if (_status == MaintenanceStatus.Delivered)
                throw new InvalidOperationException("Cannot modify status of an already delivered appliance.");

            if (_status == newStatus)
                return;

            MaintenanceStatus previousStatus = _status;
            _status = newStatus;

            // Track static count of completed repairs when reaching ReadyForDelivery
            if (newStatus == MaintenanceStatus.ReadyForDelivery &&
                previousStatus != MaintenanceStatus.ReadyForDelivery)
            {
                _totalCompletedRepairsCount++;
            }

            string msg = $"Device [{SerialNumber}] transitioned from [{previousStatus}] to [{newStatus}].";
            StatusChanged?.Invoke(this, previousStatus, newStatus, msg);
        }

        // Checks if the appliance repair is overdue and fires the DeliveryDelayed event
        public bool CheckDeliveryDelay(DateTime currentDate)
        {
            bool isFinished = (_status == MaintenanceStatus.ReadyForDelivery ||
                               _status == MaintenanceStatus.Delivered);

            if (!isFinished && currentDate.Date > _expectedDeliveryDate.Date)
            {
                int daysLate = (currentDate.Date - _expectedDeliveryDate.Date).Days;
                _status = MaintenanceStatus.Delayed;

                string alert = $"ALERT! Device {SerialNumber} (Owner: {OwnerName}) is OVERDUE by {daysLate} days!";
                DeliveryDelayed?.Invoke(this, daysLate, alert);
                return true;
            }
            return false;
        }

        public virtual string GetMaintenanceSummary()
        {
            return $"[{GetType().Name,-14}] SN: {SerialNumber} | {Brand} {Model,-15} | " +
                   $"Owner: {OwnerName,-14} | Status: {Status,-16} | Cost: {CalculateRepairCost(),9:N0} SYP";
        }
    }
}