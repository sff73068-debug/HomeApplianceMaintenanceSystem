using System;
using HomeApplianceMaintenanceSystem.Enums;
using HomeApplianceMaintenanceSystem.Models;
using HomeApplianceMaintenanceSystem.Services;
using HomeApplianceMaintenanceSystem.Utilities;

namespace HomeApplianceMaintenanceSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine("   AL-SHAM HOME APPLIANCE MAINTENANCE & REPAIR CENTER SYSTEM (TIC_IPG203_F25)   ");
            Console.WriteLine("================================================================================");

            MaintenanceCenter center = new MaintenanceCenter("Al-Sham Authorized Maintenance Center");

            Console.WriteLine("[INIT] Registering Appliances & Subscribing to Maintenance Events...");

            // 1. Create a Refrigerator instance
            Refrigerator samsungFridge = new Refrigerator(
                serialNumber: "REF-2026-1001",
                brand: "Samsung",
                model: "TwinCool",
                ownerName: "Ahmad Al-Ali",
                ownerPhone: "0944112233",
                receivedDate: new DateTime(2026, 11, 1),
                expectedDeliveryDate: new DateTime(2026, 11, 6),
                baseInspectionFee: 50000m,
                coolingVolumeLiters: 550,
                needsGasRefill: true,
                compressorReplaced: true,
                thermostatDefective: false
            );

            // 2. Create a WashingMachine instance
            WashingMachine lgWasher = new WashingMachine(
                serialNumber: "WSH-2026-2045",
                brand: "LG",
                model: "DirectDrive",
                ownerName: "Sara Hassan",
                ownerPhone: "0933556677",
                receivedDate: new DateTime(2026, 11, 2),
                expectedDeliveryDate: new DateTime(2026, 11, 10),
                baseInspectionFee: 50000m,
                drumCapacityKg: 9.0,
                isFrontLoad: true,
                needsBearingReplacement: true,
                drainPumpReplaced: true
            );

            // 3. Create an AirConditioner instance (with earlier expected delivery to test Delay Event)
            AirConditioner greeAc = new AirConditioner(
                serialNumber: "AIR-2026-3089",
                brand: "Gree",
                model: "Polar 24K",
                ownerName: "Khaled Omar",
                ownerPhone: "0988776655",
                receivedDate: new DateTime(2026, 10, 29),
                expectedDeliveryDate: new DateTime(2026, 11, 4),
                baseInspectionFee: 45000m,
                btuCapacity: 24000,
                isInverter: true,
                needsChemicalWash: true,
                capacitorReplaced: true
            );

            // Register all derived objects into the polymorphic MaintenanceCenter list
            center.RegisterAppliance(samsungFridge);
            center.RegisterAppliance(lgWasher);
            center.RegisterAppliance(greeAc);

            Console.WriteLine();
            Console.WriteLine("[VALIDATION TEST] Testing Static Class ApplianceValidator with Invalid Serial:");
            if (!ApplianceValidator.IsValidSerialNumber("123"))
            {
                Console.WriteLine("  [REJECTED] Error: Serial Number '123' is invalid! Must match XXX-YYYY-NNNN.");
            }

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("[WORKFLOW] Updating Maintenance Statuses & Firing Real-Time Events:");

            // Transition refrigerator status -> fires StatusChanged event & Customer SMS alert
            samsungFridge.UpdateStatus(MaintenanceStatus.UnderDiagnosis);
            samsungFridge.UpdateStatus(MaintenanceStatus.ReadyForDelivery);

            // Transition washing machine to UnderRepair
            lgWasher.UpdateStatus(MaintenanceStatus.UnderRepair);

            // Check for overdue appliances on 2026-11-08 -> fires DeliveryDelayed event for Gree AC
            center.CheckAllForDelays(new DateTime(2026, 11, 8));

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("[POLYMORPHIC REPORT] Processing List<Appliance> via Override Methods:");
            center.DisplayAllMaintenanceReports();

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("[STATIC & FINANCIAL SUMMARY] Global Metrics Across Center:");
            Console.WriteLine($"  • Total Registered Appliances (Appliance.TotalAppliancesCount)      : {Appliance.TotalAppliancesCount} Devices");
            Console.WriteLine($"  • Completed / Ready Repairs   (Appliance.TotalCompletedRepairsCount): {Appliance.TotalCompletedRepairsCount} Device");
            Console.WriteLine($"  • Devices Currently Delayed   (Center.GetAppliancesByStatus)        : {center.GetAppliancesByStatus(MaintenanceStatus.Delayed).Count} Device");
            Console.WriteLine($"  • Total Expected Repair Revenue (Polymorphic Sum)                   : {center.CalculateTotalExpectedRevenue():N0} SYP");
            Console.WriteLine("================================================================================");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}