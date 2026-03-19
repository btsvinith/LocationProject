using System.Collections.Generic;
using System.Threading.Tasks;
using myFirstProject.Models;

namespace myFirstProject.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<IEnumerable<State>> GetStatesByCountryAsync(int countryId);
        Task<IEnumerable<District>> GetDistrictsByStateAsync(int stateId);
        Task<IEnumerable<District>> GetDistrictsByCountryAsync(int countryId);
        Task<int> SaveLocationAsync(LocationDto dto);
        Task<int> SaveLocationsAsync(IEnumerable<LocationDto> dtos);
        Task<IEnumerable<LocationDto>> GetAllLocationsAsync();
        Task<(IEnumerable<LocationDto> Items, int TotalCount)> GetLocationsPagedAsync(int page, int pageSize);
        Task<int> DeleteLocationAsync(int id);
        Task<LocationDto?> GetLocationByIdAsync(int id);
    }
}
