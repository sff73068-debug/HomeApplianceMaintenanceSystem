using System;
using System.Collections.Generic;
using System.Text;

using System;
using HomeApplianceMaintenanceSystem.Enums;
using HomeApplianceMaintenanceSystem.Models;

namespace HomeApplianceMaintenanceSystem.Events
{
    /// <summary>
    /// Delegate for notifying subscribers whenever an appliance's maintenance status changes.
    /// </summary>
    public delegate void ApplianceStatusChangedHandler(
        Appliance appliance,
        MaintenanceStatus oldStatus,
        MaintenanceStatus newStatus,
        string notificationMessage);

    /// <summary>
    /// Delegate for alerting management and customers when repair delivery is delayed.
    /// </summary>
    public delegate void MaintenanceAlertHandler(
        Appliance appliance,
        int daysDelayed,
        string alertMessage);
}