using Cars.Data.PostgreSQL.Data;
using CarsProject.Cars.CarRepositry;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace CarsProject.Cars.CarService
{
    public class CarService : ICarService
    {

        private readonly ICarRepositry _carRepositry;

        public CarService(ICarRepositry carRepositry)
        {
            this._carRepositry = carRepositry;
        }
        public string createNewInspection(CarDTO car)
        {
            return _carRepositry.createNewInspection(car);
        }

        public string deleteInspection(Car car)
        {
            return _carRepositry.deleteInspection(car);
        }

        public IEnumerable<CarDTO> getAllCars()
        {
            return _carRepositry.getAllCars();
        }

        public CarDTO getCarById(int id)
        {
            return _carRepositry.getCarById(id);
        }

        public int updateCar(CarDTO updatedCar)
        {
            return _carRepositry.updateCar(updatedCar);
        }

    }
}
