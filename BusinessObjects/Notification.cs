using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? TargetDepartmentId { get; set; }

    public virtual Department? TargetDepartment { get; set; }
}
