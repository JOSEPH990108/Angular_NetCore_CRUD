using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.Models
{
    public class Skillset
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<UserSkillset> UserSkillsets { get; set; }
    }
}