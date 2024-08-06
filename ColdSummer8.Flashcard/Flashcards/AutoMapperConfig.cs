using AutoMapper;
using Model;

namespace Flashcards
{
    public static class AutoMapperConfig
    {
        public static IMapper InitializeAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(x =>
            {
                x.CreateMap<Stack, StackDTO>().ReverseMap();
                x.CreateMap<Flashcard, FlashcardDTO>().ReverseMap();    
                x.CreateMap<Study, StudyDTO>().ReverseMap();    
            });
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }  
}
