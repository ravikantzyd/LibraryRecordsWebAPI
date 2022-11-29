using AutoMapper;
using Library_Records_Web_API.Data;
using Library_Records_Web_API.Model;

namespace Library_Records_Web_API.Configuration
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, CreateUserDTO>().ReverseMap();
            CreateMap<User, UpdateUserDTO>().ReverseMap();

            CreateMap<SecurityQuestion, SecurityQuestionDTO>().ReverseMap();
            CreateMap<SecurityQuestion, CreateSecurityQuestionDTO>().ReverseMap();
            CreateMap<SecurityQuestion, UpdateSecurityQuestionDTO>().ReverseMap();

            CreateMap<Member, MemberDTO>().ReverseMap();
            CreateMap<Member, CreateMemberDTO>().ReverseMap();
            CreateMap<Member, UpdateMemberDTO>().ReverseMap();

            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, CreateBookDTO>().ReverseMap();
            CreateMap<Book, UpdateBookDTO>().ReverseMap();

            CreateMap<Record, RecordDTO>().ReverseMap();
            CreateMap<Record, CreateRecordDTO>().ReverseMap();
            CreateMap<Record, UpdateRecordDTO>().ReverseMap();

            CreateMap<RecordNo, RecordNoDTO>().ReverseMap();
            CreateMap<RecordNo, CreateRecordNoDTO>().ReverseMap();
            CreateMap<RecordNo, UpdateRecordNoDTO>().ReverseMap();

            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
        }
    }
}
