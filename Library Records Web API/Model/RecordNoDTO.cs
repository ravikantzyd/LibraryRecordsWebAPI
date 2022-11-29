using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{
    public class CreateRecordNoDTO
    {
        [Required]
        [MaxLength(30)]
        public string Date { get; set; }

        public int Number { get; set; }
    }

    public class UpdateRecordNoDTO : CreateRecordNoDTO
    {

    }

    public class ViewRecordNoByDateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Date { get; set; }
    }

    public class RecordNoDTO : CreateRecordNoDTO
    {
        public int Id { get; set; }
    }
}
