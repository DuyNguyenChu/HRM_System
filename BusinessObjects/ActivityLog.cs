using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class ActivityLog
{
    public int ActivityLogId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string TablePrimaryKey { get; set; } = null!;

    public int RecordId { get; set; }

    public string? Details { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
