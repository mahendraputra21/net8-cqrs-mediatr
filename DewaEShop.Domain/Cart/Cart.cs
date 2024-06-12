namespace DewaEShop.Domain.Cart
{
    public class Cart
    {
        public Guid Id { get; private set; }
        public List<CartItem.CartItem>? Items { get; private set; }
        public decimal TotalPrice => CalculateTotalPrice();

        private Cart() { }

        public Cart(Guid id)
        {
            Id = id;
            Items = new List<CartItem.CartItem>();
        }

        // Method to add an item to the cart
        public void AddCartItem(Product.Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity should be greater than zero.");

            // If the product is already in the cart, update the quatity
            var existingItem = Items?.Find(item => item?.Product?.Id == product.Id);
            if (existingItem != null)
                existingItem.Quantity += quantity;
            else
                Items?.Add(new CartItem.CartItem(product, quantity));
        }

        // Method to update the quatity of an item in the cart
        public void UpdateCartItemQuantity(Product.Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity should be greater than zero.");

            var existingItem = Items?.Find(item => item?.Product?.Id == product.Id);
            if (existingItem != null)
                existingItem.Quantity = quantity;
            else
                throw new ArgumentException("Product not found in cart.");
        }

        // Method to remove an item from the cart
        public void RemoveCartItem(Product.Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");

            var existingItem = Items?.Find(item => item?.Product?.Id == product.Id);
            if (existingItem != null)
                Items?.Remove(existingItem);
            else
                throw new ArgumentException("Product not found in cart.");
        }

        // Method to calculate the total price of all items in the cart
        private decimal CalculateTotalPrice()
        {
            decimal totalPrice = 0;

            if (Items?.Count > 0)
            {
                foreach (var item in Items)
                {
                    totalPrice += item.Product.Price * item.Quantity;
                }
            }

            return totalPrice;
        }
    }
}
