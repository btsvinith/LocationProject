using System;

namespace myFirstProject.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public bool RequiresStateDistrict { get; set; } = true;
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }

    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }

    public class District
    {
        public int Id { get; set; }
        public string DistrictName { get; set; } = string.Empty;
        public int StateId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }

    public class LocationDto
    {
        public int? Id { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public int? StateId { get; set; }
        public string? StateName { get; set; }
        public int? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public int? RequiresStateDistrict { get; set; }
    }
}
