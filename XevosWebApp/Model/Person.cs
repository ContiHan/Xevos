using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XevosWebApp.Model
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Jméno")]
        public string? Jmeno { get; set; }
        [Required]
        [DisplayName("Příjmení")]
        public string? Prijmeni { get; set; }
        [Required]
        [DisplayName("Datum")]
        public DateTime Date { get; set; }
    }
}
