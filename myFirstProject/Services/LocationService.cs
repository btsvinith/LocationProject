using System.Collections.Generic;
using System.Threading.Tasks;
using myFirstProject.Models;
using myFirstProject.Repositories;

namespace myFirstProject.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repo;
        public LocationService(ILocationRepository repo)
        {
            _repo = repo;
        }

        public Task<int> DeleteLocationAsync(int id) => _repo.DeleteLocationAsync(id);
        public Task<IEnumerable<LocationDto>> GetAllLocationsAsync() => _repo.GetAllLocationsAsync();
        public Task<(IEnumerable<LocationDto> Items, int TotalCount)> GetLocationsPagedAsync(int page, int pageSize) => _repo.GetLocationsPagedAsync(page, pageSize);
        public Task<LocationDto?> GetLocationByIdAsync(int id) => _repo.GetLocationByIdAsync(id);
        public Task<int> SaveLocationAsync(LocationDto dto) => _repo.SaveLocationAsync(dto);
        public Task<int> SaveLocationsAsync(IEnumerable<LocationDto> dtos) => _repo.SaveLocationsAsync(dtos);
        public Task<IEnumerable<Country>> GetAllCountriesAsync() => _repo.GetAllCountriesAsync();
        public Task<IEnumerable<State>> GetStatesByCountryAsync(int countryId) => _repo.GetStatesByCountryAsync(countryId);
        public Task<IEnumerable<District>> GetDistrictsByStateAsync(int stateId) => _repo.GetDistrictsByStateAsync(stateId);
        public Task<IEnumerable<District>> GetDistrictsByCountryAsync(int countryId) => _repo.GetDistrictsByCountryAsync(countryId);
    }
}
