using Library_Records_Web_API.Data;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{
    public class CreateSecurityQuestionDTO
    {
        [Required]
        [MaxLength(300)]
        public string Question { get; set; }

        [Required]
        [MaxLength(300)]
        public string Answer { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public class UpdateSecurityQuestionDTO : CreateSecurityQuestionDTO
    {

    }

    public class ViewBySecurityQuestionDTO : CreateSecurityQuestionDTO
    {

    }

    public class SecurityQuestionDTO : CreateSecurityQuestionDTO
    {
        public int Id { get; set; }
        
        public virtual UserDTO Users { get; set; }
    }
}
