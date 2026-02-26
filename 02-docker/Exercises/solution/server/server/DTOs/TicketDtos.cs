namespace server.DTOs
{
    public class TicketCreateDtos
    {
        public int PurchaseId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
    }
    public class TicketResponseDtos
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
    }
    public class TicketUpdateDtos
    {
       
        public int PurchaseId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
    }
}
