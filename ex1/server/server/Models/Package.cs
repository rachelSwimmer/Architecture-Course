using Microsoft.Identity.Client;

namespace server.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quentity { get; set; }
        public int Price { get; set; }

        public ICollection<PurchasePackage> PurchasePackages { get; set; } = new List<PurchasePackage>();
        
       



    }
}
