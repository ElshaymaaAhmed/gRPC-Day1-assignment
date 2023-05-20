namespace ITI.gRPC.Client.Models
{
    public class Order
    {
        public int UserId { get; set; }
        public int Cost { get; set; }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
        
    }
}
