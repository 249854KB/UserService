

namespace UserService.Dtos
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RankInSystem { get; set; }
        public int NumberOfDogs { get; set; }
    }
}