using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class Book
    {
        public int Id { get; set; }

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
        public Category Categories { get; set; }

        [Required]
        public int TotalCount { get; set; }   

        public IList<Record> Records { get; set; }
    }
}
