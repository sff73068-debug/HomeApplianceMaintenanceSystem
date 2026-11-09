using System;
using System.Collections.Generic;
using System.Text;

using System;
using HomeApplianceMaintenanceSystem.Enums;

namespace HomeApplianceMaintenanceSystem.Interfaces
{
    /// <summary>
    /// Defines core maintenance operations shared across all repairable home appliances.
    /// Fulfils the Abstraction requirement (Interface).
    /// </summary>
    public interface IMaintainable
    {
        // Diagnoses the technical issue of the appliance and returns a detailed report
        string DiagnoseIssue();

        // Updates the maintenance lifecycle status and triggers relevant events
        void UpdateStatus(MaintenanceStatus newStatus);

        // Checks whether the appliance has exceeded its expected delivery date
        bool CheckDeliveryDelay(DateTime currentDate);

        // Returns a formatted maintenance card summary including polymorphic repair cost
        string GetMaintenanceSummary();
    }
}