using System.Reflection.Metadata;

namespace server.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ?Description { get; set; }

        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string DonorId { get; set; }
        public string? WinnerId { get; set; }
        public bool IsDrown { get; set; } = false;

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();


        public Category Category { get; set; }
        public Donor Donor { get; set; }
        public User ?Winner { get; set; }

    }
}
