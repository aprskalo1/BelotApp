using System;
using System.Collections.Generic;

namespace BelotApp.Models;

public partial class GameResult
{
    public int Id { get; set; }

    public int? GameId { get; set; }

    public int? TeamOneResult { get; set; }

    public int? TeamTwoResult { get; set; }

    public string? Combination { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Game? Game { get; set; }
}
