﻿using System.ComponentModel;

namespace BelotApp.ViewModels
{
    public class GameResultsVM
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public int TeamOneResult { get; set; }

        public int TeamTwoResult { get; set; }

        [DisplayName("Zvanja")]
        public string? Combination { get; set; }
    }
}
