using Microsoft.Build.Framework;
using System.ComponentModel;

namespace BelotApp.ViewModels
{
    public class BugReportVM
    {
        [DisplayName("Ime")]
        public string Name { get; set; }

        [DisplayName("Naslov")]
        public string Subject { get; set; }

        [DisplayName("Opis problema")]
        public string Description { get; set; }
    }
}
