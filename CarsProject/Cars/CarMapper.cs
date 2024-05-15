using Cars.Data.PostgreSQL.Data;

namespace CarsProject.Cars
{
    public static class CarMapper
    {
        public static Car ToDomainModel(this CarDTO carDTO)
        {
            return new Car
            {
                id = carDTO.id,
                VIN = carDTO.VIN,
                date = carDTO.date,
                creationReason = carDTO.creationReason,
            };
        }

        public static CarDTO ToDTO(this Car car)
        {
            return new CarDTO
            {
                id = car.id,
                VIN = car.VIN,
                date = car.date,
                creationReason = car.creationReason,
            };
        }
        public static List<CarDTO> ToDTOList(this List<Car> products)
        {
            return products.Select(p => p.ToDTO()).ToList();
        }
    }

}
