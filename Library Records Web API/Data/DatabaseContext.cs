using Microsoft.EntityFrameworkCore;

namespace Library_Records_Web_API.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<SecurityQuestion> SecurityQuestions { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<RecordNo> RecordNos { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
