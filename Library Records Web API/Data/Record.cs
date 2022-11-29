using System;
using System.ComponentModel.DataAnnotations;

namespace Library_Records_Web_API.Data
{
    public class Record
    {
        public int Id { get; set; }

        [Required]
        public string RecordId { get; set; }

        [Required]
        public int MemberId { get; set; }
        public Member Members { get; set; }

        [Required]
        public int BookId { get; set; }
        public Book Books { get; set; }

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
        public User Users { get; set; }
    }
}
