using Library_Records_Web_API.Data;
using Library_Records_Web_API.IRepository;
using System;
using System.Threading.Tasks;

namespace Library_Records_Web_API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        private IGenericRepository<User> _users;
        private IGenericRepository<SecurityQuestion> _security_questions;
        private IGenericRepository<Member> _members;
        private IGenericRepository<Book> _books;
        private IGenericRepository<Record> _records;
        private IGenericRepository<RecordNo> _record_nos;
        private IGenericRepository<Category> _categories;
        
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<User> Users => _users ??= new GenericRepository<User>(_context);

        public IGenericRepository<SecurityQuestion> SecurityQuestions => _security_questions ??= new GenericRepository<SecurityQuestion>(_context);

        public IGenericRepository<Member> Members => _members ??= new GenericRepository<Member>(_context);

        public IGenericRepository<Book> Books => _books ??= new GenericRepository<Book>(_context);

        public IGenericRepository<Record> Records => _records ??= new GenericRepository<Record>(_context);

        public IGenericRepository<RecordNo> RecordNos => _record_nos ??= new GenericRepository<RecordNo>(_context);

        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
