using System.ComponentModel.DataAnnotations;

namespace server.DTOs
{
    public class PackageCreateDtos
    {
        [Required,MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, Range(1, double.MaxValue)]
        public int Quentity { get; set; }
        [Required, Range(10, double.MaxValue)]
        public int Price { get; set; }


    }
    public class PackageUpdateDtos
    {
        [MaxLength (255)]
        public string Name { get; set; }
        
        public string Description { get; set; }
        [Range(1,double.MaxValue)]
        public int Quentity { get; set; }
        [Range(10,double.MaxValue)]
        public int Price { get; set; }

    }
    public class PackageResponseDtos
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quentity { get; set; }
        public int Price { get; set; }
    }
}
