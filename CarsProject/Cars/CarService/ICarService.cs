using Cars.Data.PostgreSQL.Data;

namespace CarsProject.Cars.CarService
{
    public interface ICarService
    {
        public IEnumerable<CarDTO> getAllCars();
        string deleteInspection(Car car);
        string createNewInspection(CarDTO car);
        public CarDTO getCarById(int id);
        int updateCar(CarDTO updatedCar);

    }
}
