using System;
using System.Collections.Generic;

namespace webapi.Models;

public partial class News
{
    public string AnnouncementId { get; set; } = null!;

    public DateTime PublishTime { get; set; } 

    public string? PublishPos { get; set; } = string.Empty;

    public string Title { get; set; } = null!;

    public string? Contents { get; set; } = string.Empty;

    public decimal? Likes { get; set; } = 0;

    public decimal? ViewCount { get; set; } = 0;
}
