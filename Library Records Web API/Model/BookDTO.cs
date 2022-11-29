using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{
    public class CreateBookDTO
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        [MaxLength(500)]
        public string BookName { get; set; }

        [Required]
        [MaxLength(500)]
        public string Author { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int TotalCount { get; set; }  
    }

    public class UpdateBookDTO : CreateBookDTO
    {

    }

    public class ViewByBookNameDTO
    {
        public string BookName { get; set; } = "";
    }

    public class ViewByBookIdDTO
    {
        public string BookId { get; set; } = "";
    }

    public class ViewByCategoryIdDTO
    {
        public int CategoryId { get; set; }
    }

    public class SearchByBookDataDTO
    {
        public string BookData { get; set; }
    }

    public class BookDTO : CreateBookDTO
    {
        public int Id { get; set; }

        public CategoryDTO Categories { get; set; }
        public virtual List<RecordDTO> Records { get; set; }
    }
}
