using Cars.Data.PostgreSQL.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CarsProject.Cars.CarRepositry
{
    public interface ICarRepositry
    {
        public string createNewInspection(CarDTO car);
        public string deleteInspection(Car car);
        public IEnumerable<CarDTO> getAllCars();
        public CarDTO getCarById(int id);
        public int updateCar(CarDTO updatedCar);

    }
}
