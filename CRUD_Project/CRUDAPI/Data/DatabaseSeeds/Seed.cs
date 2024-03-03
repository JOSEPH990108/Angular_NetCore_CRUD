using System.Text.Json;
using AutoMapper;
using CRUDAPI.DTOs;
using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDAPI.Data.DatabaseSeeds
{
    public class Seed
    {
        public static async Task SeedData(ApplicationDbContext db, IMapper mapper)
        {
            // Seed hobbies first
            await SeedHobby(db);

            // Seed skillsets next
            await SeedSkillSet(db);

            // Finally, seed users
            await SeedUsers(db, mapper);
        }
        private static async Task SeedSkillSet(ApplicationDbContext db)
        {
            if(await db.Skillsets.AnyAsync()) return;

            var skillSetData = await File.ReadAllTextAsync("Data/DatabaseSeeds/SkillSet.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var skillSets = JsonSerializer.Deserialize<List<Skillset>>(skillSetData, options);

            skillSets?.ForEach(skillSet => db.Skillsets.Add(skillSet));

            await db.SaveChangesAsync();
        }

        private static async Task SeedHobby(ApplicationDbContext db)
        {
            if(await db.Hobbies.AnyAsync()) return;

            var hobbyData = await File.ReadAllTextAsync("Data/DatabaseSeeds/Hobby.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var hobbies = JsonSerializer.Deserialize<List<Hobby>>(hobbyData, options);

            hobbies?.ForEach(hobby => db.Hobbies.Add(hobby));

            await db.SaveChangesAsync();
        }

        private static async Task SeedUsers(ApplicationDbContext db, IMapper mapper)
        {
            if (await db.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/DatabaseSeeds/Users.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var usersDTO = JsonSerializer.Deserialize<List<UserDTO>>(userData, options);

            if (usersDTO != null)
            {
                foreach (var userDTO in usersDTO)
                {
                    var user = mapper.Map<User>(userDTO);
                    // Add user to the database
                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    // Add user hobbies to the database
                    if (userDTO.UserHobbies != null && userDTO.UserHobbies.Any())
                    {
                        foreach (var hobbyId in userDTO.UserHobbies)
                        {
                            var userHobby = new UserHobby { UserId = user.Id, HobbyId = hobbyId };

                            // Check if the entity is already tracked
                            var existingUserHobby = await db.UserHobbies.FindAsync(userHobby.UserId, userHobby.HobbyId);
                            if (existingUserHobby != null)
                            {
                                db.Entry(existingUserHobby).State = EntityState.Modified;
                            }
                            else
                            {
                                db.UserHobbies.Add(userHobby);
                            }
                        }
                    }

                    // Add user skillsets to the database
                    if (userDTO.UserSkillsets != null && userDTO.UserSkillsets.Any())
                    {
                        foreach (var skillsetId in userDTO.UserSkillsets)
                        {
                            var userSkillset = new UserSkillset { UserId = user.Id, SkillsetId = skillsetId };

                            // Check if the entity is already tracked
                            var existingUserSkillset = await db.UserSkillsets.FindAsync(userSkillset.UserId, userSkillset.SkillsetId  );
                            if (existingUserSkillset != null)
                            {
                                db.Entry(existingUserSkillset).State = EntityState.Modified;
                            }
                            else
                            {
                                db.UserSkillsets.Add(userSkillset);
                            }
                        }
                    }
                }
            }
            await db.SaveChangesAsync();
        }
    }
}