using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string MemberId { get; set; }

        [Required]
        [MaxLength(300)]
        public string MemberName { get; set; }

        [Required]
        [MaxLength(100)]
        public string RNPost { get; set; }

        [Required]
        [MaxLength(500)]
        public string ClassDepartment { get; set; }
                
        public IList<Record> Records { get; set; }
    }
}
