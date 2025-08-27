namespace CqrsApiExample.Models
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ProductDTO()
        {

        }
        public ProductDTO(string name, string description, decimal price)
        {

            Name = name;
            Description = description;
            Price = price;
        }
    }
}