namespace DewaEShop.Domain.CartItem
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public virtual Product.Product? Product { get; }
        public int Quantity { get; set; }

        private CartItem() { }

        public CartItem(Product.Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
