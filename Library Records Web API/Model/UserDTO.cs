using Library_Records_Web_API.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(300)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
    }

    public class UpdateUserDTO : CreateUserDTO
    {

    }

    public class ViewByUserNameDTO
    {
        public string UserName { get; set; } = "";
    }

    public class UserDTO : CreateUserDTO
    {
        public int Id { get; set; }        

        public virtual List<SecurityQuestionDTO> SecurityQuestions { get; set; }
    }
}
