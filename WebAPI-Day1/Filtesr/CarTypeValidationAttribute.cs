using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI_Day1.Models;

namespace WebAPI_Day1.Filtesr
{
    public class CarTypeValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Car? car = context.ActionArguments["car"] as Car;
            var allowdTypes = new string[] { "Electric", "Gas", "Diesel", "Hybrid" };
            
            if (car is null || !allowdTypes.Contains(car.Type)){
                context.Result = new BadRequestObjectResult(new {Message = "Car Type is not allowed"});
            }
        }
    }
}
