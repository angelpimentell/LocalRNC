using System.ComponentModel.DataAnnotations;

namespace LocalRNC.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        [Required]
        [StringLength(11)]
        public required string RNC { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public required DateTime Created_at { get; set; }
    }
}
