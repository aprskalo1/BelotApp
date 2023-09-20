﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelotApp.ViewModels
{
    public class GameVM
    {
        public int Id { get; set; }

        [DisplayName("Prvi tim")]
        [StringLength(50)]
        public string? TeamOneName { get; set; }

        [DisplayName("Drugi tim")]
        [StringLength(50)]
        public string? TeamTwoName { get; set; }

        [DisplayName("Datum")]
        public DateTime PlayedAt { get; set; }

        [DisplayName("Pobjednik")]
        public string? Winner { get; set; }

        [DisplayName("Trajanje")]
        public int? GameLength { get; set; }
    }
}
