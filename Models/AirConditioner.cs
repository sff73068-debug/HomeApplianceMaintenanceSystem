using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace HomeApplianceMaintenanceSystem.Models
{
    /// <summary>
    /// Concrete subclass representing an Air Conditioner appliance (مكيف هوائي).
    /// Inherits from Appliance and calculates repair cost based on BTU capacity and inverter electronics.
    /// </summary>
    public class AirConditioner : Appliance
    {
        private int _btuCapacity;
        private bool _isInverter;
        private bool _needsChemicalWash;
        private bool _capacitorReplaced;

        public int BtuCapacity
        {
            get => _btuCapacity;
            set
            {
                if (value < 9000 || value > 60000)
                    throw new ArgumentException("BTU capacity must be between 9,000 and 60,000 BTU.");
                _btuCapacity = value;
            }
        }

        public bool IsInverter
        {
            get => _isInverter;
            set => _isInverter = value;
        }

        public bool NeedsChemicalWash
        {
            get => _needsChemicalWash;
            set => _needsChemicalWash = value;
        }

        public bool CapacitorReplaced
        {
            get => _capacitorReplaced;
            set => _capacitorReplaced = value;
        }

        public AirConditioner(
            string serialNumber,
            string brand,
            string model,
            string ownerName,
            string ownerPhone,
            DateTime receivedDate,
            DateTime expectedDeliveryDate,
            decimal baseInspectionFee,
            int btuCapacity,
            bool isInverter,
            bool needsChemicalWash,
            bool capacitorReplaced)
            : base(serialNumber, brand, model, ownerName, ownerPhone,
                   receivedDate, expectedDeliveryDate, baseInspectionFee)
        {
            BtuCapacity = btuCapacity;
            IsInverter = isInverter;
            NeedsChemicalWash = needsChemicalWash;
            CapacitorReplaced = capacitorReplaced;
        }

        public override decimal CalculateRepairCost()
        {
            decimal totalCost = BaseInspectionFee;

            if (NeedsChemicalWash)
                totalCost += 90000m;

            if (CapacitorReplaced)
                totalCost += 75000m;

            if (IsInverter)
                totalCost += 55000m; // Inverter board calibration & diagnostics surcharge

            if (BtuCapacity >= 24000)
                totalCost += 30000m; // High-capacity outdoor unit handling fee

            return totalCost;
        }

        public override string DiagnoseIssue()
        {
            string tech = IsInverter ? "Inverter" : "Standard";
            return $"Capacity: {BtuCapacity} BTU ({tech}) | Chemical Wash: {(NeedsChemicalWash ? "Yes" : "No")} | " +
                   $"Capacitor Replaced: {(CapacitorReplaced ? "Yes" : "No")}";
        }
    }
}