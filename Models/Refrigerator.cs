using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace HomeApplianceMaintenanceSystem.Models
{
    /// <summary>
    /// Concrete subclass representing a Refrigerator appliance (ثلاجة).
    /// Inherits from Appliance and overrides CalculateRepairCost() and DiagnoseIssue().
    /// </summary>
    public class Refrigerator : Appliance
    {
        private int _coolingVolumeLiters;
        private bool _needsGasRefill;
        private bool _compressorReplaced;
        private bool _thermostatDefective;

        public int CoolingVolumeLiters
        {
            get => _coolingVolumeLiters;
            set
            {
                if (value < 50 || value > 1200)
                    throw new ArgumentException("Cooling volume must be between 50 and 1200 liters.");
                _coolingVolumeLiters = value;
            }
        }

        public bool NeedsGasRefill
        {
            get => _needsGasRefill;
            set => _needsGasRefill = value;
        }

        public bool CompressorReplaced
        {
            get => _compressorReplaced;
            set => _compressorReplaced = value;
        }

        public bool ThermostatDefective
        {
            get => _thermostatDefective;
            set => _thermostatDefective = value;
        }

        public Refrigerator(
            string serialNumber,
            string brand,
            string model,
            string ownerName,
            string ownerPhone,
            DateTime receivedDate,
            DateTime expectedDeliveryDate,
            decimal baseInspectionFee,
            int coolingVolumeLiters,
            bool needsGasRefill,
            bool compressorReplaced,
            bool thermostatDefective)
            : base(serialNumber, brand, model, ownerName, ownerPhone,
                   receivedDate, expectedDeliveryDate, baseInspectionFee)
        {
            CoolingVolumeLiters = coolingVolumeLiters;
            NeedsGasRefill = needsGasRefill;
            CompressorReplaced = compressorReplaced;
            ThermostatDefective = thermostatDefective;
        }

        public override decimal CalculateRepairCost()
        {
            decimal totalCost = BaseInspectionFee;

            if (NeedsGasRefill)
            {
                decimal gasCost = 125000m;
                // Large refrigerators (> 500L) require 20% extra refrigerant charge
                if (CoolingVolumeLiters > 500)
                    gasCost *= 1.20m;

                totalCost += gasCost;
            }

            if (CompressorReplaced)
                totalCost += 325000m;

            if (ThermostatDefective)
                totalCost += 65000m;

            return totalCost;
        }

        public override string DiagnoseIssue()
        {
            return $"Cooling Volume: {CoolingVolumeLiters}L | Gas Refill: {(NeedsGasRefill ? "Yes" : "No")} | " +
                   $"Compressor Replaced: {(CompressorReplaced ? "Yes" : "No")} | " +
                   $"Thermostat Replaced: {(ThermostatDefective ? "Yes" : "No")}";
        }
    }
}