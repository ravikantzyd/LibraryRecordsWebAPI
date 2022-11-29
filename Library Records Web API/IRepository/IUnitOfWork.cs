using Library_Records_Web_API.Data;
using System;
using System.Threading.Tasks;

namespace Library_Records_Web_API.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<User> Users { get; }
        IGenericRepository<SecurityQuestion> SecurityQuestions { get; }
        IGenericRepository<Member> Members { get; }
        IGenericRepository<Book> Books { get; }
        IGenericRepository<Record> Records { get; }
        IGenericRepository<RecordNo> RecordNos { get; }
        IGenericRepository<Category> Categories { get; }
        
        Task Save();
    }
}
