using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Models
{
    public class UserHobby
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int HobbyId { get; set; }
        public Hobby Hobby { get; set; }
    }
}