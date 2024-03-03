using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAPI.DTOs
{
    public class RegisterDTO
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
         [RegularExpression(@"^\+?[0-9]*$")] // Allow optional "+" sign followed by numeric characters
        public string PhoneNumber { get; set; }

        public List<int> UserHobbies { get; set; }

        public List<int> UserSkillsets { get; set; }
    }
}