namespace cqrs_mediatr.Domain
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public decimal Price { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        // Parameterless constructor for EF Core
        
        public Product(string name, string description, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
        }

        // Method to Update the Product
        public void UpdateProduct(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        // Method to Set the product to Inactive
        public void InactiveProduct(bool isDeleted)
        {
            if (isDeleted)
                throw new ArgumentException("Product is not active");

            // Soft delete
            IsDeleted = true;
        }
    }
}
