using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class BackupHistory
{
    public int BackupId { get; set; }

    public DateTime? BackupDate { get; set; }

    public string BackupFilePath { get; set; } = null!;
}
