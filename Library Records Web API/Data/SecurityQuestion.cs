using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class SecurityQuestion
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Question { get; set; }

        [Required]
        [MaxLength(300)]
        public string Answer { get; set; }

        [Required]        
        public int UserId { get; set; }
        public User Users { get; set; }
    }
}
