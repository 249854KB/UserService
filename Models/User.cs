using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int RankInSystem { get; set; }
        [Required]
        public int NumberOfDogs { get; set; }

    }
}