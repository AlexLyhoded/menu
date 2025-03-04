using AutoMapper;
using menu.Interface;
using menu.Model;

public class MappingProfile : Profile
{
    private readonly IDishRepository _dishRepository;

    public MappingProfile(IDishRepository dishRepository)
    {
        _dishRepository = dishRepository;
    }

    public MappingProfile()
    {
        // Маппинг для Dish и DishDto (обратный маппинг тоже)
        CreateMap<Dish, DishDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));

        // Обратный маппинг для DishDto -> Dish
        CreateMap<DishDto, Dish>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight));

        // Маппинг для Category и CategoryDto (обратный маппинг тоже)
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.DishesId, opt => opt.MapFrom(src => src.Dishes))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        // Обратный маппинг для CategoryDto -> Category
        CreateMap<CategoryDto, Category>()
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.DishesId))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.DishesId, opt => opt.MapFrom(src => src.Dishes))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status)); 
        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.Dishes, opt => opt.MapFrom(src => src.DishesId))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

    }
}
