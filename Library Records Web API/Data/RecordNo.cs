using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class RecordNo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Date { get; set; }

        public int Number { get; set; }
    }
}
