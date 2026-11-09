using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace HomeApplianceMaintenanceSystem.Models
{
    /// <summary>
    /// Concrete subclass representing a Washing Machine appliance (غسالة).
    /// Inherits from Appliance and provides custom drum, bearing, and pump repair cost logic.
    /// </summary>
    public class WashingMachine : Appliance
    {
        private double _drumCapacityKg;
        private bool _isFrontLoad;
        private bool _needsBearingReplacement;
        private bool _drainPumpReplaced;

        public double DrumCapacityKg
        {
            get => _drumCapacityKg;
            set
            {
                if (value < 3.0 || value > 25.0)
                    throw new ArgumentException("Drum capacity must be between 3 and 25 Kg.");
                _drumCapacityKg = value;
            }
        }

        public bool IsFrontLoad
        {
            get => _isFrontLoad;
            set => _isFrontLoad = value;
        }

        public bool NeedsBearingReplacement
        {
            get => _needsBearingReplacement;
            set => _needsBearingReplacement = value;
        }

        public bool DrainPumpReplaced
        {
            get => _drainPumpReplaced;
            set => _drainPumpReplaced = value;
        }

        public WashingMachine(
            string serialNumber,
            string brand,
            string model,
            string ownerName,
            string ownerPhone,
            DateTime receivedDate,
            DateTime expectedDeliveryDate,
            decimal baseInspectionFee,
            double drumCapacityKg,
            bool isFrontLoad,
            bool needsBearingReplacement,
            bool drainPumpReplaced)
            : base(serialNumber, brand, model, ownerName, ownerPhone,
                   receivedDate, expectedDeliveryDate, baseInspectionFee)
        {
            DrumCapacityKg = drumCapacityKg;
            IsFrontLoad = isFrontLoad;
            NeedsBearingReplacement = needsBearingReplacement;
            DrainPumpReplaced = drainPumpReplaced;
        }

        public override decimal CalculateRepairCost()
        {
            decimal totalCost = BaseInspectionFee;

            if (NeedsBearingReplacement)
            {
                decimal bearingCost = 140000m;
                // Front-load or heavy-duty drums (> 8Kg) require complex spider shaft disassembly
                if (IsFrontLoad || DrumCapacityKg > 8.0)
                    bearingCost += 35000m;

                totalCost += bearingCost;
            }

            if (DrainPumpReplaced)
                totalCost += 85000m;

            return totalCost;
        }

        public override string DiagnoseIssue()
        {
            string loadType = IsFrontLoad ? "Front-Load" : "Top-Load";
            return $"Drum: {DrumCapacityKg}Kg ({loadType}) | Bearings Replaced: {(NeedsBearingReplacement ? "Yes" : "No")} | " +
                   $"Drain Pump Replaced: {(DrainPumpReplaced ? "Yes" : "No")}";
        }
    }
}