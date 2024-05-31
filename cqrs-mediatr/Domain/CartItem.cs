namespace cqrs_mediatr.Domain
{
    public class CartItem
    {
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; }
        public int Quantity { get; set; }

        private CartItem() { }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
