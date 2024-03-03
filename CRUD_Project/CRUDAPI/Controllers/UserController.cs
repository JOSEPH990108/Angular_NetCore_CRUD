using CRUDAPI.Helpers;
using AutoMapper;
using CRUDAPI.DTOs;
using CRUDAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CRUDAPI.Extensions;

namespace CRUDAPI.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IUnitOfWorkRepository _unitOfWork;
        private readonly IMapper _mapper;

        public UserController(IUnitOfWorkRepository unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Get all freelancers' information
        //[HttpGet]
        // public async Task<IActionResult> GetAllFreelancers()
        // {
        //     try
        //     {
        //         var users = await _unitOfWork.UserRepository.GetUsersAsync();
        //         if(users != null)
        //             return Ok(users);
        //         else
        //             return Ok("Not users for now!");
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, new { error = ex.Message });
        //     }
        // }
        [HttpGet]
        public async Task<ActionResult<PageList<UserDTO>>> GetAllFreelancers([FromQuery]PaginationParams paginationParams)
        {
            var users = await _unitOfWork.UserRepository.GetMembersAsync(paginationParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

        //Get freelancer's information by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreeancerById(int id){
            try
            {
                var user = await _unitOfWork.UserRepository.GetUserByIdAsync(id);
                if(user != null)
                    return Ok(user);
                else
                    return NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Register new freelancer
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(RegisterDTO registerDTO)
        {
            try
            {
                if (registerDTO == null)
                    return BadRequest("User object is null");

                if (await _unitOfWork.UserRepository.UserExists(registerDTO.Username))
                    return BadRequest("Username already exists");

                if (await _unitOfWork.UserRepository.MailExists(registerDTO.Mail))
                    return BadRequest("Email already exists");

                if (await _unitOfWork.UserRepository.PhoneNumberExists(registerDTO.PhoneNumber))
                    return BadRequest("Phone number already exists");

                var addedUser = await _unitOfWork.UserRepository.AddUser(registerDTO);
                return Ok(addedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Update existing freelancer's information
        [HttpPut("update/{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] UserDTO userDTO)
        { 
            try
            {
                var user = await _unitOfWork.UserRepository.UpdateUser(id, userDTO);

                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                var userToReturn = _mapper.Map<UserDTO>(user);

                return Ok(userToReturn);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Delete freelancer's information from database
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            string username = await _unitOfWork.UserRepository.GetUsernameByIdAsync(id);
            username = username.ToUpper();
            try
            {
                var deletedUser = await _unitOfWork.UserRepository.DeleteUser(id);
                if (deletedUser == null)
                {
                    return NotFound(new { message = $"User with ID={id} not found." });
                }
                return Ok(new { message = $"{username} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Get all hobbies from database
        [HttpGet("available-hobbies")]
        public async Task<IActionResult> GetAvailableHobbies()
        {
            try
            {
                var availableHobbies = await _unitOfWork.UserRepository.GetHobbiesAsync();
                return Ok(availableHobbies);
            }
            catch (Exception ex)
            {
                 return StatusCode(500, new { error = ex.Message });
            }
        }

        //Get all skillsets from database
        [HttpGet("available-skillsets")]
        public async Task<IActionResult> GetAvailableSkillsets()
        {
            try
            {
                var availableSkillsets = await _unitOfWork.UserRepository.GetSkillsetsAsync();
                return Ok(availableSkillsets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}