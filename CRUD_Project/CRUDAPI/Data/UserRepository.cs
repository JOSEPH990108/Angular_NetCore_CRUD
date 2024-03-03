using Microsoft.EntityFrameworkCore;
using AutoMapper;
using CRUDAPI.DTOs;
using CRUDAPI.Helpers;
using CRUDAPI.Interfaces;
using CRUDAPI.Models;

using AutoMapper.QueryableExtensions;


namespace CRUDAPI.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserDTO> AddUser(RegisterDTO registerDTO)
        {
            try
            {
                var user = _mapper.Map<User>(registerDTO);
                _db.Users.Add(user);
                await _db.SaveChangesAsync();     

                var addedUserDTO = _mapper.Map<UserDTO>(user);
                return addedUserDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating username by ID.", ex);
            }
        }

        public async Task<User> UpdateUser(int id, UserDTO userDTO)
        {
            try
            {
                var user = await _db.Users.Include(u => u.UserHobbies)
                        .Include(u => u.UserSkillsets)
                        .FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("User not found.");

                // Check uniqueness of updated fields
                if (await _db.Users.AnyAsync(u => u.Id != id && (u.Username == userDTO.Username || u.Mail == userDTO.Mail || u.PhoneNumber == userDTO.PhoneNumber)))
                {
                    throw new InvalidOperationException("Username, mail, or phone number already exists for another user.");
                }

                // Update user fields
                user.Username = userDTO.Username.ToLower();
                user.Mail = userDTO.Mail.ToLower();
                user.PhoneNumber = userDTO.PhoneNumber;

                // Update hobbies
                foreach (var hobbyId in userDTO.UserHobbies)
                {
                    // Validate hobby ID
                    var hobby = await _db.Hobbies.FindAsync(hobbyId) ?? throw new Exception($"Hobby with ID {hobbyId} not found.");
                    if (!user.UserHobbies.Any(uh => uh.HobbyId == hobbyId))
                    {
                        user.UserHobbies.Add(new UserHobby { UserId = id, HobbyId = hobbyId });
                    }
                }

                // Remove hobbies not in the updated list
                var hobbiesToRemove = user.UserHobbies.Where(uh => !userDTO.UserHobbies.Contains(uh.HobbyId)).ToList();
                foreach (var hobbyToRemove in hobbiesToRemove)
                {
                    user.UserHobbies.Remove(hobbyToRemove);
                }

                // Update skillsets
                foreach (var skillsetId in userDTO.UserSkillsets)
                {
                    // Validate skillset ID
                    var skillset = await _db.Skillsets.FindAsync(skillsetId) ?? throw new Exception($"Skillset with ID {skillsetId} not found.");
                    if (!user.UserSkillsets.Any(us => us.SkillsetId == skillsetId))
                    {
                        user.UserSkillsets.Add(new UserSkillset { UserId = id, SkillsetId = skillsetId });
                    }
                }

                // Remove skillsets not in the updated list
                var skillsetsToRemove = user.UserSkillsets.Where(us => !userDTO.UserSkillsets.Contains(us.SkillsetId)).ToList();
                foreach (var skillsetToRemove in skillsetsToRemove)
                {
                    user.UserSkillsets.Remove(skillsetToRemove);
                }

                await _db.SaveChangesAsync();

                return user;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid operation occurred during user update.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating user.", ex);
            }
        }


        public async Task<User> DeleteUser(int id)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id) ?? throw new Exception("User not found.");
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting user.", ex);
            }
        }

        public async Task<IEnumerable<Hobby>> GetHobbiesAsync()
        {
           try
            {
                return await _db.Hobbies.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching hobbies.", ex);
            }
        }

        public async Task<IEnumerable<Skillset>> GetSkillsetsAsync()
        {
            try
            {
                return await _db.Skillsets.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching skillsets.", ex);
            }
        }

        public async Task<PageList<UserDTO>> GetMembersAsync(PaginationParams paginationParams)
        {
            var query = _db.Users
                    .OrderBy(u => u.Username)
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .AsNoTracking();

            return await PageList<UserDTO>.CreateAsync(query, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            try
            {
                var usersDto = await _db.Users
                    .OrderBy(u => u.Username)
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return usersDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching users.", ex);
            }
        }

       public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            try
            {
                var userDto = await _db.Users
                    .Where(u => u.Id == id)
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (userDto == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }

                return userDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching user by ID.", ex);
            }
        }

        public async Task<String> GetUsernameByIdAsync(int id)
        {
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
                if(user != null){
                    return user.Username;
                }
                var message = "User Not Found!";
                return  message;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("An error occurred while retrieving username by ID.", ex);
            }
        }


        public async Task<bool> UserExists(string username)
        {
            return await _db.Users.AnyAsync(u => u.Username == username.ToLower());
        }

        public async Task<bool> MailExists(string mail)
        {
            return await _db.Users.AnyAsync(u => u.Mail == mail.ToLower());
        }

         public async Task<bool> PhoneNumberExists(string phoneNumber)
        {
            return await _db.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}