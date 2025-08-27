namespace CqrsApiExample.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public Product()
        {

        }
        public Product(int id, string name, string description, decimal price)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}