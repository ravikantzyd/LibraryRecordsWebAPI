using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{
    public class CreateMemberDTO
    {
        [Required]
        [MaxLength(60)]
        public string MemberId { get; set; }

        [Required]
        [MaxLength(300)]
        public string MemberName { get; set; }

        [Required]
        [MaxLength(60)]
        public string RNPost { get; set; }

        [Required]
        [MaxLength(500)]
        public string ClassDepartment { get; set; }       
    }

    public class UpdateMemberDTO : CreateMemberDTO
    {

    }

    public class ViewByMemberNameDTO
    {
        public string MemberName { get; set; } = "";
    }

    public class ViewByMemberIdDTO
    {
        public string MemberId { get; set; } = "";
    }

    public class SearchByMemberDataDTO
    {
        public string MemberData { get; set; }
    }

    public class MemberDTO : CreateMemberDTO
    {
        public int Id { get; set; }
        
        public virtual List<RecordDTO> Records { get; set; }
    }
}
