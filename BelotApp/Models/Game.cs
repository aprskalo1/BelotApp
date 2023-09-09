using System;
using System.Collections.Generic;

namespace BelotApp.Models;

public partial class Game
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string? TeamOneName { get; set; }

    public string? TeamTwoName { get; set; }

    public string? Winner { get; set; }

    public DateTime? PlayedAt { get; set; }

    public virtual ICollection<GameResult> GameResults { get; set; } = new List<GameResult>();
}
