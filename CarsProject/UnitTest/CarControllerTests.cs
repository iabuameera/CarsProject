namespace CarsProject.UnitTest
{
    using Xunit;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using Cars.Cars;
    using Cars.Data.PostgreSQL.Data;
    using Microsoft.AspNetCore.Cors.Infrastructure;

    namespace YourNamespace.Tests.Controllers
    {
        public class CarControllerTests
        {
            private CarController _carController;
            private Mock<CarService> _carServiceMock;

            public CarControllerTests()
            {
                _carServiceMock = new Mock<CarService>();
                _carController = new CarController(_carServiceMock.Object);
            }

            [Fact]
            public void InsertNewCar_ValidCar_ReturnsOk()
            {

                var car = new Car
                {
                    creationReason = "check-in",
                    VIN = "QWRWERFTWEFG"
                };

                _carServiceMock.Setup(x => x.createNewInspection(It.IsAny<Car>())).Returns(car.ToString());


                var result = _carController.insertNewCar(car) as OkObjectResult;


                Assert.NotNull(result);
                Assert.Equal(200, result.StatusCode);
                Assert.Equal(car, result.Value);
            }

            [Fact]
            public void InsertNewCar_InvalidCar_ReturnsInternalServerError()
            {
                var car = new Car
                {
                    creationReason = "check-in",
                    VIN = "QWRWERFTWEFG"
                };
                _carServiceMock.Setup(x => x.createNewInspection(It.IsAny<Car>())).Throws(new Exception("Some error message"));


                var result = _carController.insertNewCar(car) as ObjectResult;


                Assert.NotNull(result);
                Assert.Equal(500, result.StatusCode);
                Assert.Contains("Internal Server Error", result.Value.ToString());
            }

            [Fact]
            public void DeleteCarById_ExistingId_ReturnsOk()
            {
               
                int id = 2; 
                var mockCarService = new Mock<CarService>();
                mockCarService.Setup(x => x.GetCarById(id)).Returns(new Car()); 

                var carController = new CarController(mockCarService.Object);

                var result = carController.deleteCarById(id);

                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.Equal("Deleted Successfully", okResult.Value);

                mockCarService.Verify(x => x.deleteInspection(It.IsAny<Car>()), Times.Once);
            }

            [Fact]
            public void DeleteCarById_NonExistingId_ReturnsNotFound()
            {

                int id = 456;
                var mockCarService = new Mock<CarService>();
                mockCarService.Setup(x => x.GetCarById(id)).Returns((Car)null); 

                var carController = new CarController(mockCarService.Object);

                var result = carController.deleteCarById(id);

                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public void GetSingleInspection_ExistingId_ReturnsOk()
            {

                int id = 123; 
                var mockCarService = new Mock<CarService>();
                mockCarService.Setup(x => x.GetCarById(id)).Returns(new Car()); 

                var carController = new CarController(mockCarService.Object);

                var result = carController.getSingleInspection(id);

                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.IsType<Car>(okResult.Value); 
            }

            [Fact]
            public void GetSingleInspection_NonExistingId_ReturnsNotFound()
            {
                
                int id = 456; 
                var mockCarService = new Mock<CarService>();
                mockCarService.Setup(x => x.GetCarById(id)).Returns((Car)null); 

                var carController = new CarController(mockCarService.Object);

               
                var result = carController.getSingleInspection(id);

                
                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public void UpdateCar_ValidUpdate_ReturnsOk()
            {
                var updatedCar = new Car
                {

                    id = 7,
                    creationReason = "CHECK-OUT",
                    VIN = "EWRTWETRWE"
                };

                var mockCarService = new Mock<CarService>();
                mockCarService.Setup(x => x.UpdateCar(updatedCar)).Returns(1);

                var carController = new CarController(mockCarService.Object);

                var result = carController.UpdateCar(updatedCar);

                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.Equal(1, okResult.Value);
            }
        }
    }

}
