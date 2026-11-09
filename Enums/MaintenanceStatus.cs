using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace HomeApplianceMaintenanceSystem.Enums
{
    /// <summary>
    /// Represents the lifecycle states of an appliance inside the maintenance center.
    /// </summary>
    public enum MaintenanceStatus
    {
        Received = 0,         // تم استلام الجهاز في المركز
        UnderDiagnosis = 1,   // قيد الفحص وتشخيص العطل
        UnderRepair = 2,      // قيد الإصلاح الفني
        ReadyForDelivery = 3, // مكتمل الإصلاح وجاهز للتسليم
        Delivered = 4,        // تم التسليم للعميل
        Delayed = 5           // متأخر عن موعد التسليم المتوقع
    }
}