namespace WebAPI_Day1.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public Car() { 
            
        }
        public Car (int id, string brand, string model, string type)
        {
            Id = id;
            Brand = brand;
            Model = model;
            Type = type;
        }

    }
}
