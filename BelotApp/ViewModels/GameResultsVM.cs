using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelotApp.ViewModels
{
    public class GameResultsVM
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public int TeamOneResult { get; set; }

        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public int TeamTwoResult { get; set; }

        [DisplayName("Adut od:")]
        public string? TrumpCall { get; set; }

        [DisplayName("Zvanja od:")]
        public string? CombinationCall { get; set; }

        [DisplayName("Zvanja")]
        public int? Combination { get; set; }
    }
}
