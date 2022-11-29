using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        public List<SecurityQuestion> SecurityQuestions { get; set; }
    }
}
