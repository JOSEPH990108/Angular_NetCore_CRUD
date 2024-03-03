
namespace CRUDAPI.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public List<int> UserHobbies { get; set; }
        public List<int> UserSkillsets { get; set; }
    }
}