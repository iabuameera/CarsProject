using Cars.Data.PostgreSQL.Data;
using CarsProject.Cars;
using CarsProject.Cars.CarService;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace Cars.Cars
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        
        private readonly ICarService _carService;
        private readonly IDistributedCache _cache;

        public CarController(ICarService carService, IDistributedCache cache)
        {
            this._carService = carService;
            this._cache = cache;
        }

        //public CarController(CarService @object)
        //{
        //}

        [HttpPost]
        public IActionResult insertNewCar([FromBody] CarDTO carDTO)
        {
            try
            {
                _carService.createNewInspection(carDTO);
                return Ok(carDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteCarById(int id)
        {
            var CarToDelete = _carService.getCarById(id).ToDomainModel();

            if (CarToDelete == null)
            {
                return NotFound();
            }
            _carService.deleteInspection(CarToDelete);
            return Ok("Deleted Successfully");
        }

        [HttpGet("{id}")]
        public IActionResult getSingleInspection(int id)
        {
            try
            {
                CarDTO car = _carService.getCarById(id);

                if (car == null)
                {
                    return NotFound(); 
                }
                var result = car;
                return Ok(result); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        public IActionResult GetAllCars(int? filterYear = null, int? filterMonth = null, string filterCreationReason = null)
        {
            try
            {
                var cacheKey = "AllCars";
                IEnumerable<CarDTO> cars;

                var cachedData = _cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    cars = JsonConvert.DeserializeObject<IEnumerable<CarDTO>>(cachedData);
                }
                else
                {
                     cars = _carService.getAllCars();

                    _cache.SetString(cacheKey, JsonConvert.SerializeObject(cars), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                }

                if (filterYear.HasValue)
                {
                    cars = cars.Where(c => c.date.Year == filterYear).ToList();
                }

                if (filterMonth.HasValue)
                {
                    cars = cars.Where(c => c.date.Month == filterMonth).ToList();
                }

                if (!string.IsNullOrWhiteSpace(filterCreationReason))
                {
                    cars = cars.Where(c => c.creationReason == filterCreationReason).ToList();
                }

                if (!cars.Any())
                {
                    return NotFound();
                }

                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPut]
        public IActionResult UpdateCar( [FromBody] CarDTO updatedCar)
        {
            try
            {
                int updatedCarResult = _carService.updateCar(updatedCar);

                if (updatedCarResult == 0)
                {
                    return NotFound();
                }
                return Ok(updatedCarResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}
