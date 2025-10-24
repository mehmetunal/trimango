using AutoMapper;
using Trimango.Data.Mssql.Entities;
using Trimango.Dto.Mssql.Supplier;
using Trimango.Dto.Mssql.Property;
using Trimango.Dto.Mssql.PropertyType;
using Trimango.Dto.Mssql.Location;
using Trimango.Dto.Mssql.User;

namespace Trimango.Mssql.Services.Mapping;

/// <summary>
/// AutoMapper konfigürasyon profili
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Supplier mappings
        CreateMap<Supplier, SupplierDto>()
            .ForMember(dest => dest.PropertiesCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0))
            .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.Users != null ? src.Users.Count : 0));

        CreateMap<CreateSupplierDto, Supplier>();
        CreateMap<UpdateSupplierDto, Supplier>();

        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType != null ? src.PropertyType.Name : string.Empty))
            .ForMember(dest => dest.LocationCity, opt => opt.MapFrom(src => src.Location != null && src.Location.City != null ? src.Location.City.Name : string.Empty))
            .ForMember(dest => dest.LocationDistrict, opt => opt.MapFrom(src => src.Location != null && src.Location.District != null ? src.Location.District.Name : string.Empty))
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : string.Empty))
            .ForMember(dest => dest.ImagesCount, opt => opt.MapFrom(src => src.PropertyImages != null ? src.PropertyImages.Count : 0))
            .ForMember(dest => dest.UnitsCount, opt => opt.MapFrom(src => src.Units != null ? src.Units.Count : 0))
            .ForMember(dest => dest.ReservationsCount, opt => opt.MapFrom(src => src.Reservations != null ? src.Reservations.Count : 0));

        CreateMap<CreatePropertyDto, Property>();
        CreateMap<UpdatePropertyDto, Property>();

        // PropertyType mappings
        CreateMap<PropertyType, PropertyTypeDto>()
            .ForMember(dest => dest.PropertiesCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0));

        CreateMap<CreatePropertyTypeDto, PropertyType>();
        CreateMap<UpdatePropertyTypeDto, PropertyType>();

        // Location mappings
        CreateMap<Location, LocationDto>()
            .ForMember(dest => dest.PropertiesCount, opt => opt.MapFrom(src => src.Properties != null ? src.Properties.Count : 0));

        CreateMap<CreateLocationDto, Location>();
        CreateMap<UpdateLocationDto, Location>();

        // User mappings
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<ApplicationUser, UserListDto>();
        CreateMap<CreateUserDto, ApplicationUser>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));
        CreateMap<UpdateUserDto, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}