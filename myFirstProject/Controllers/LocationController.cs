using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myFirstProject.Models;
using Serilog;
using System.Linq;
using System.Collections.Generic;
using myFirstProject.Services;

namespace myFirstProject.Controllers
{
    public class LocationController : Controller
    {
        private readonly Serilog.ILogger _logger = Serilog.Log.ForContext<LocationController>();
        private readonly ILocationService _service;
        public LocationController(ILocationService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            _logger.Information("Index page requested");
            var countries = await _service.GetAllCountriesAsync();
            ViewBag.Countries = countries;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] List<LocationDto> dtos)
        {
            if (dtos == null || dtos.Count == 0) return BadRequest();

            try
            {
                _logger.Information("Save called with {Count} items", dtos?.Count ?? 0);
                var countries = await _service.GetAllCountriesAsync();
                var normalized = new List<LocationDto>();
                foreach (var dto in dtos)
                {
                    if (dto == null) continue;
                    if (!dto.CountryId.HasValue)
                    {
                        _logger.Warning("Save validation failed: missing CountryId");
                        return BadRequest(new { message = "Country is required." });
                    }

                    var country = countries.FirstOrDefault(c => c.Id == dto.CountryId);
                    var requires = country?.RequiresStateDistrict ?? true;

                    if (!requires)
                    {
                        dto.StateId = null;
                        dto.DistrictId = null;
                        dto.StateName = string.Empty;
                        dto.DistrictName = string.Empty;
                    }
                    else
                    {
                        if (!dto.StateId.HasValue || !dto.DistrictId.HasValue)
                        {
                            _logger.Warning("Save validation failed: missing state/district for countryId={CountryId}", dto.CountryId);
                            return BadRequest(new { message = "State and District are required for selected countries." });
                        }
                    }

                    normalized.Add(dto);
                }

                int total = await _service.SaveLocationsAsync(normalized);
                _logger.Information("Save succeeded, saved {Count} rows", total);
                return Ok(new { message = "Saved", count = total });
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Save failed");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStates(int countryId)
        {
            try
            {
                _logger.Information("GetStates called for countryId={CountryId}", countryId);
                var states = await _service.GetStatesByCountryAsync(countryId);
                _logger.Information("GetStates success for countryId={CountryId} returned {Count} items", countryId, states == null ? 0 : states.Count());
                return Json(states);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "GetStates failed for countryId={CountryId}", countryId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDistricts(int stateId)
        {
            try
            {
                _logger.Information("GetDistricts called for stateId={StateId}", stateId);
                var districts = await _service.GetDistrictsByStateAsync(stateId);
                _logger.Information("GetDistricts success for stateId={StateId} returned {Count} items", stateId, districts == null ? 0 : districts.Count());
                return Json(districts);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "GetDistricts failed for stateId={StateId}", stateId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDistrictsByCountry(int countryId)
        {
            try
            {
                _logger.Information("GetDistrictsByCountry called for countryId={CountryId}", countryId);
                var districts = await _service.GetDistrictsByCountryAsync(countryId);
                _logger.Information("GetDistrictsByCountry success for countryId={CountryId} returned {Count} items", countryId, districts == null ? 0 : districts.Count());
                return Json(districts);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "GetDistrictsByCountry failed for countryId={CountryId}", countryId);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {       
            int page = 1;
            int pageSize = 5;
            try
            {
                _logger.Information("GetPaged called page={Page} pageSize={PageSize}", page, pageSize);
                var paged = await _service.GetLocationsPagedAsync(page, pageSize);
                _logger.Information("GetPaged success page={Page} items={Count}", page, paged.Items == null ? 0 : paged.Items.Count());
                return Json(new { items = paged.Items, total = paged.TotalCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "GetPaged failed page={Page} pageSize={PageSize}", page, pageSize);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 5)
        {
            try
            {
                _logger.Information("GetAll called (redirecting to GetPaged)");
                var paged = await _service.GetLocationsPagedAsync(page, pageSize);
                _logger.Information("GetAll success items={Count}", paged.Items == null ? 0 : paged.Items.Count());
                return Json(new { items = paged.Items, total = paged.TotalCount });
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "GetAll failed");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.Information("GetById called id={Id}", id);
                var item = await _service.GetLocationByIdAsync(id);
                _logger.Information("GetById success id={Id} found={Found}", id, item != null);
                return Json(item);
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "GetById failed id={Id}", id);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            try
            {
                _logger.Information("Delete called id={Id}", id);
                await _service.DeleteLocationAsync(id);
                _logger.Information("Delete succeeded id={Id}", id);
                return Ok(new { message = "Deleted" });
            }
            catch (System.Exception ex)
            {
                _logger.Error(ex, "Delete failed id={Id}", id);
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
