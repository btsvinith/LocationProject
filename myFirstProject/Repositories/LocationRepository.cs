using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using myFirstProject.Models;

namespace myFirstProject.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly string _connectionString;

        public LocationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("LocationDB");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            using var conn = CreateConnection();
            var all = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            var countries = new List<Country>();
            foreach (var r in all)
            {
                if (r.CountryId.HasValue && !countries.Exists(c => c.Id == r.CountryId.Value))
                    countries.Add(new Country { Id = r.CountryId.Value, CountryName = r.CountryName ?? string.Empty, RequiresStateDistrict = r.RequiresStateDistrict != 0 });
            }
            return countries;
        }

        public async Task<IEnumerable<State>> GetStatesByCountryAsync(int countryId)
        {
            using var conn = CreateConnection();
            var all = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            var states = new List<State>();
            foreach (var r in all)
            {
                if (r.CountryId == countryId && r.StateId.HasValue && !states.Exists(s => s.Id == r.StateId.Value))
                    states.Add(new State { Id = r.StateId.Value, StateName = r.StateName ?? string.Empty, CountryId = countryId });
            }
            return states;
        }

        public async Task<IEnumerable<District>> GetDistrictsByStateAsync(int stateId)
        {
            using var conn = CreateConnection();
            var all = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            var districts = new List<District>();
            foreach (var r in all)
            {
                if (r.StateId == stateId && r.DistrictId.HasValue && !districts.Exists(d => d.Id == r.DistrictId.Value))
                    districts.Add(new District { Id = r.DistrictId.Value, DistrictName = r.DistrictName ?? string.Empty, StateId = stateId });
            }
            return districts;
        }

        public async Task<IEnumerable<District>> GetDistrictsByCountryAsync(int countryId)
        {
            using var conn = CreateConnection();
            var all = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            var districts = new List<District>();
            foreach (var r in all)
            {
                if (r.CountryId == countryId && r.DistrictId.HasValue && !districts.Exists(d => d.Id == r.DistrictId.Value))
                    districts.Add(new District { Id = r.DistrictId.Value, DistrictName = r.DistrictName ?? string.Empty, StateId = r.StateId ?? 0 });
            }
            return districts;
        }

        public async Task<int> SaveLocationAsync(LocationDto dto)
        {
            using var conn = CreateConnection();
            var p = new DynamicParameters();
            if (dto.Id.HasValue && dto.Id.Value > 0)
            {
                p.Add("Action", "UPDATE");
                p.Add("CountryId", dto.CountryId);
                p.Add("StateId", dto.StateId);
                p.Add("DistrictId", dto.Id); 
                p.Add("CountryName", dto.CountryName);
                p.Add("StateName", dto.StateName);
                p.Add("DistrictName", dto.DistrictName);
            }
            else
            {
                p.Add("Action", "INSERT");
                p.Add("CountryName", dto.CountryName);
                p.Add("StateName", dto.StateName);
                p.Add("DistrictName", dto.DistrictName);
            }

            var res = await conn.ExecuteAsync("sp_CountryStateDistrict", p, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<int> SaveLocationsAsync(IEnumerable<LocationDto> dtos)
        {
            using var conn = CreateConnection();
            int total = 0;
            foreach (var dto in dtos)
            {
                var p = new DynamicParameters();
                if (dto.Id.HasValue && dto.Id.Value > 0)
                {
                    p.Add("Action", "UPDATE");
                    p.Add("CountryId", dto.CountryId);
                    p.Add("StateId", dto.StateId);
                    p.Add("DistrictId", dto.Id); 
                    p.Add("CountryName", dto.CountryName);
                    p.Add("StateName", dto.StateName);
                    p.Add("DistrictName", dto.DistrictName);
                }
                else
                {
                    p.Add("Action", "INSERT");
                    p.Add("CountryName", dto.CountryName);
                    p.Add("StateName", dto.StateName);
                    p.Add("DistrictName", dto.DistrictName);
                }

                total += await conn.ExecuteAsync("sp_CountryStateDistrict", p, commandType: CommandType.StoredProcedure);
            }

            return total;
        }

        public async Task<IEnumerable<LocationDto>> GetAllLocationsAsync()
        {
            using var conn = CreateConnection();
            var res = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            return res.OrderBy(r => r.DistrictId ?? r.Id ?? 0);
        }

        public async Task<(IEnumerable<LocationDto> Items, int TotalCount)> GetLocationsPagedAsync(int page, int pageSize)
        {
            using var conn = CreateConnection();
            var res = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            var list = res.OrderBy(r => r.DistrictId ?? r.Id ?? 0).ToList();
            var total = list.Count;
            var items = list.Skip((page - 1) * pageSize).Take(pageSize);
            return (items, total);
        }

        public async Task<int> DeleteLocationAsync(int id)
        {
            using var conn = CreateConnection();
            var p = new DynamicParameters();
            p.Add("Action", "DELETE");
            p.Add("DistrictId", id);
            var res = await conn.ExecuteAsync("sp_CountryStateDistrict", p, commandType: CommandType.StoredProcedure);
            return res;
        }

        public async Task<LocationDto?> GetLocationByIdAsync(int id)
        {
            using var conn = CreateConnection();
            var res = await conn.QueryAsync<LocationDto>("sp_CountryStateDistrict", new { Action = "SELECT" }, commandType: CommandType.StoredProcedure);
            return res.FirstOrDefault(r => r.DistrictId == id);
        }
    }
}
