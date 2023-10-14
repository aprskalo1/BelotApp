using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BelotApp.ViewModels
{
    public class BugReportVM
    {
        [DisplayName("Ime")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Name { get; set; }

        [DisplayName("Naslov")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Subject { get; set; }

        [DisplayName("Opis")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Description { get; set; }
    }
}
