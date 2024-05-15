using Cars.Data.PostgreSQL.Data;
using System.Data;

namespace CarsProject.Cars.CarRepositry
{
    public class CarRepositry : ICarRepositry
    {
        private readonly CarDbContext _context;

        public CarRepositry(CarDbContext context) {
            _context = context;
        }

        public string createNewInspection(CarDTO car)
        {
            var result = car.ToDomainModel();
            result.date = DateTime.UtcNow;
            _context.Cars.Add(result);
            _context.SaveChanges();
            return "Created Successfully";
        }

        public string deleteInspection(Car car)
        {
            _context.Cars.Remove(car);
            _context.SaveChanges();
            return "Deleted Succsecfully";
        }

        public IEnumerable<CarDTO> getAllCars()
        {
            var result = _context.Cars.ToList();
            return result.ToDTOList();
        }

        public CarDTO getCarById(int id)
        {
            Car car = _context.Cars.Find(id);

            if (car == null)
            {
                throw new Exception($"Car with ID {id} not found.");
            }
            var result = car.ToDTO();
            return result;
        }

        public int updateCar(CarDTO updatedCar)
        {

            var result = updatedCar.ToDomainModel();    
            var existingCar = _context.Cars.Find(result.id);

            if (existingCar != null)
            {
                existingCar.VIN = result.VIN;
                existingCar.creationReason = result.creationReason;
                _context.Update(existingCar);
                _context.SaveChanges();

                return 1;
            }
            return 0;
        }

    }
}
