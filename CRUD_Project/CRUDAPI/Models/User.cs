using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<UserHobby> UserHobbies { get; set; }
        public ICollection<UserSkillset> UserSkillsets { get; set; }
    }
}