using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Day1.Filtesr;
using WebAPI_Day1.Models;

namespace WebAPI_Day1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private static List<Car> _cars = new()
        {
            new(1,"Hyundai","Tucson","Gas"),
            new(2,"Honda","Civic","Gas"),
            new(3,"Toyota","Supra","Gas"),
            new(4,"Mazda","3","Gas"),
        };

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_cars);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetById(int id)
        {
            Car? car = _cars.FirstOrDefault(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        [Route("v1")]
        public ActionResult Add(Car car)
        {
            car.Id = _cars.Count + 1;
            car.Type = "Gas";
            _cars.Add(car);
            return CreatedAtAction(
                nameof(GetById),
                new { id = car.Id },
                new { Message = "Created Successfully" }
                );
        }

        [HttpPost]
        [Route("v2")]
        [CarTypeValidation]
        public ActionResult Add_V2(Car car)
        {
            car.Id = _cars.Count + 1;
            _cars.Add(car);
            return CreatedAtAction(
                nameof(GetById),
                new { id = car.Id },
                new { Message = "Created Successfully" }
                );
        }


        [HttpPut]
        [Route("{id}")]
        public ActionResult Update(Car car, int id)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }
            var carToUpdate = _cars.FirstOrDefault(c => c.Id == id);
            if (carToUpdate == null) { return NotFound(); }
            carToUpdate.Id = car.Id;
            carToUpdate.Model = car.Model;
            carToUpdate.Brand = car.Brand;
            carToUpdate.Type = car.Type;
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete(int id)
        {
            var car = _cars.FirstOrDefault(c => c.Id == id);
            if (car == null) { return NotFound(); };
            _cars.Remove(car);
            return NoContent();
        }


        [HttpGet]
        [Route("request_count")]
        public ActionResult GetRequestsCount() {
            return Ok(ReqeustsCounterContainer.Count);
        }
    }

    public class ReqeustsCounterContainer
    {
        public static  int Count { get; set; } = 0;

    }
}
    


