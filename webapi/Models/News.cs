using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class News
{
    public string AnnouncementId { get; set; } = null!;

    public DateTime PublishTime { get; set; }

    public string? PublishPos { get; set; }

    public string Title { get; set; } = null!;

    public string? Contents { get; set; }

    public decimal? Likes { get; set; }

    public decimal? ViewCount { get; set; }
}
