using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{

    public class CreateCategoryDTO
    {       
        [Required]
        [MaxLength(300)]
        public string CategoryName { get; set; }
    }

    public class UpdateCategoryDTO : CreateCategoryDTO
    {

    }

    public class ViewByCategoryNameDTO
    {
        public string CategoryName { get; set; }
    }

    public class CategoryDTO : CreateCategoryDTO
    {
        public int Id { get; set; }

        public List<BookDTO> Books { get; set; }
    }
}
