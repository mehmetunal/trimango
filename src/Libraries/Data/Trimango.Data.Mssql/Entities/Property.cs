using Trimango.Data.Mssql;

namespace Trimango.Data.Mssql.Entities
{
    /// <summary>
    /// Konaklama tesis entity'si
    /// </summary>
    public class Property : BaseEntity
    {
        public Guid SupplierId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid PropertyTypeId { get; set; }
        public Guid LocationId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? DistrictId { get; set; }
        public int Capacity { get; set; }
        public int RoomCount { get; set; }
        public int BathroomCount { get; set; }
        public int SquareMeter { get; set; }
        
        // Navigation Properties
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual PropertyType PropertyType { get; set; } = null!;
        public virtual Location Location { get; set; } = null!;
        public virtual City? City { get; set; }
        public virtual District? District { get; set; }
        public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public virtual ICollection<PropertyFeatureMapping> PropertyFeatureMappings { get; set; } = new List<PropertyFeatureMapping>();
        public virtual ICollection<DistanceInfo> DistanceInfos { get; set; } = new List<DistanceInfo>();
        public virtual ICollection<SeasonalPricing> SeasonalPricings { get; set; } = new List<SeasonalPricing>();
        public virtual ICollection<StayRule> StayRules { get; set; } = new List<StayRule>();
        public virtual ICollection<ExtraFee> ExtraFees { get; set; } = new List<ExtraFee>();
    }
}
