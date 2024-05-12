using Cars.Data.PostgreSQL.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace Cars.Cars
{
    public class CarService
    {

        private readonly CarDbContext _context;

        public CarService (CarDbContext context)
        {
            _context = context;
        }

        public string createNewInspection(Car car)
        {
            

                car.date = DateTime.UtcNow;
            

            _context.Cars.Add(car);
            _context.SaveChanges();
            return "Created Successfully";
        }

        public string deleteInspection(Car car)
        {
            _context.Cars.Remove(car);
            _context.SaveChanges();
            return "Deleted Succsecfully";
        }

        internal IEnumerable<Car> GetAllCars()
        {
            return _context.Cars.ToList();
        }

        internal Car GetCarById(int id)
        {
                Car car = _context.Cars.Find(id);

                if (car == null)
                {
                    throw new Exception($"Car with ID {id} not found.");
                }

                return car;
        }

        public int UpdateCar(Car updatedCar)
        {
            var existingCar = _context.Cars.Find(updatedCar.id);

            if (existingCar != null)
            {
                existingCar.VIN = updatedCar.VIN; 
                existingCar.creationReason = updatedCar.creationReason;

                _context.SaveChanges();

                return 1;
            }
            return 0;
        }

    }
}
