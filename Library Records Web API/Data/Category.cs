using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string CategoryName { get; set; }

        public IList<Book> Books { get; set; }
    }
}
