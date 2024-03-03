using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Models
{
    public class UserSkillset
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int SkillsetId { get; set; }
        public Skillset Skillset { get; set; }
    }
}