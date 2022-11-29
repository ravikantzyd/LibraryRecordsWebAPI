using System;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Model
{
    public class CreateRecordDTO
    {
        [Required]
        public string RecordId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public string BorrowSignature { get; set; }
        
        public DateTime ReturnDate { get; set; }

        public string ReturnSignature { get; set; }

        public int DateExtended { get; set; }

        public string DExtendedSignature { get; set; }

        [Required]
        public int UserId { get; set; }
    }

    public class UpdateRecordDTO
    {
        public DateTime ReturnDate { get; set; }

        public string ReturnSignature { get; set; }

        public int DateExtended { get; set; }

        public string DExtendedSignature { get; set; }
    }

    public class SearchByRecordDataDTO
    {
        public string RecordData { get; set; }
    }

    public class ViewByRIDandBookNameDTO
    {
        public string RecordId { get; set; }
        public string BookName { get; set; }
    }

    public class RecordDTO : CreateRecordDTO
    {
        public int Id { get; set; }
        
        public virtual MemberDTO Members { get; set; }
        
        public virtual BookDTO Books { get; set; }
        
        public virtual UserDTO Users { get; set; }
    }
}
