using System.ComponentModel.DataAnnotations;
namespace UserService.Dtos
{
    public class UserCreateDto
    {
        //Data transfer Objects
        [Required]
        public string Name { get; set; }
        public int RankInSystem { get; set; }
        [Required]
        public int NumberOfDogs { get; set; }
    }
}