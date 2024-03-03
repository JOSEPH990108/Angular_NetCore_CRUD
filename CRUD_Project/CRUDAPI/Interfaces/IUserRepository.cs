using CRUDAPI.DTOs;
using CRUDAPI.Helpers;
using CRUDAPI.Models;

namespace CRUDAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<PageList<UserDTO>> GetMembersAsync(PaginationParams paginationParams);
        Task<IEnumerable<Skillset>> GetSkillsetsAsync();
        Task<IEnumerable<Hobby>> GetHobbiesAsync();
        Task<UserDTO> AddUser(RegisterDTO registerDTO);
        Task<User> UpdateUser(int id, UserDTO userDTO);
        Task<User> DeleteUser(int id);
        Task<String> GetUsernameByIdAsync(int id);
        Task<bool> UserExists(string username);
        Task<bool> MailExists(string mail);
        Task<bool> PhoneNumberExists(string mail);
    }

}