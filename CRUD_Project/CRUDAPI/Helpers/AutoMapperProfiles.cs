using CRUDAPI.Models;
using AutoMapper;
using CRUDAPI.DTOs;


namespace CRUDAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.UserHobbies, opt => opt.MapFrom(src => src.UserHobbies.Select(uh => uh.HobbyId)))
                .ForMember(dest => dest.UserSkillsets, opt => opt.MapFrom(src => src.UserSkillsets.Select(us => us.SkillsetId)));

             CreateMap<UserDTO, User>()
            .ForMember(dest => dest.UserHobbies, opt => opt.Ignore())
            .ForMember(dest => dest.UserSkillsets, opt => opt.Ignore());

            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.UserHobbies, opt => opt.MapFrom(src => MapUserHobbies(src.UserHobbies)))
                .ForMember(dest => dest.UserSkillsets, opt => opt.MapFrom(src => MapUserSkillsets(src.UserSkillsets)));
        }

        public List<UserHobby> MapUserHobbies(List<int> hobbyIds)
        {
            return hobbyIds.Select(hobbyId => new UserHobby { HobbyId = hobbyId }).ToList();
        }

        public List<UserSkillset> MapUserSkillsets(List<int> skillsetIds)
        {
            return skillsetIds.Select(skillsetId => new UserSkillset { SkillsetId = skillsetId }).ToList();
        }
    }
}

                // // Assign UserId to UserHobby entities
                // if (registerDTO.UserHobbies != null && registerDTO.UserHobbies.Any())
                // {
                //     foreach (var hobbyId in registerDTO.UserHobbies)
                //     {
                //         var userHobby = new UserHobby { UserId = user.Id, HobbyId = hobbyId };

                //         // Check if the entity is already tracked
                //         var existingUserHobby = await _db.UserHobbies.FindAsync(userHobby.UserId, userHobby.HobbyId);
                //         if (existingUserHobby != null)
                //         {
                //             _db.Entry(existingUserHobby).State = EntityState.Modified;
                //         }
                //         else
                //         {
                //             _db.UserHobbies.Add(userHobby);
                //         }
                //     }
                //     await _db.SaveChangesAsync();
                // }

                // Assign UserId to UserHobby entities
                // if (registerDTO.UserSkillsets != null && registerDTO.UserSkillsets.Any())
                // {
                //     foreach (var skillsetId in registerDTO.UserSkillsets)
                //     {
                //         var userSkillset = new UserSkillset { UserId = user.Id, SkillsetId = skillsetId };

                //         // Check if the entity is already tracked
                //         var existingUserSkillset = await _db.UserSkillsets.FindAsync(userSkillset.UserId, userSkillset.SkillsetId);
                //         if (existingUserSkillset != null)
                //         {
                //             _db.Entry(existingUserSkillset).State = EntityState.Modified;
                //         }
                //         else
                //         {
                //             _db.UserSkillsets.Add(userSkillset);
                //         }
                //     }
                //     await _db.SaveChangesAsync();
                // }   