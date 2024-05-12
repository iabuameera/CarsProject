using Cars.Data.PostgreSQL.Data;
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
        
        private readonly CarService _carService;
        private readonly IDistributedCache _cache;

        public CarController(CarService carService, IDistributedCache cache)
        {
            this._carService = carService;
            this._cache = cache;
        }

        public CarController(CarService @object)
        {
        }

        [HttpPost]
        public IActionResult insertNewCar([FromBody] Car car)
        {
            try
            {
                var result = this._carService.createNewInspection(car);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult deleteCarById(int id)
        {
            var CarToDelete = _carService.GetCarById(id);

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
                Car car = _carService.GetCarById(id);

                if (car == null)
                {
                    return NotFound(); 
                }

                return Ok(car); 
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
                IEnumerable<Car> cars;

                var cachedData = _cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(cachedData))
                {
                    cars = JsonConvert.DeserializeObject<IEnumerable<Car>>(cachedData,
                        new JsonSerializerSettings
                        {
                            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                        });
                }
                else
                {
                     cars = _carService.GetAllCars();

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
        public IActionResult UpdateCar( [FromBody] Car updatedCar)
        {
            try
            {
               int updatedCarResult = _carService.UpdateCar(updatedCar);

                if (updatedCarResult == 1)
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
