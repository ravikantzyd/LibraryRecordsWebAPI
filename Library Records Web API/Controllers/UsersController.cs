using AutoMapper;
using Library_Records_Web_API.Data;
using Library_Records_Web_API.IRepository;
using Library_Records_Web_API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library_Records_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public UsersController(IUnitOfWork unitOfWork, ILogger<UsersController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAll();
                var results = _mapper.Map<IList<UserDTO>>(users);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetUsers)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("UsersCount", Name = "GetUsersCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersCount()
        {
            try
            {
                var users = await _unitOfWork.Users.GetCount();
                //var results = _mapper.Map<IList<CustomerDTO>>(Userss);

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetUsersCount)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("UsersByDecending", Name = "GetUsersByDecending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersByDecending()
        {
            try
            {
                var users = await _unitOfWork.Users.GetAll(null, q => q.OrderByDescending(i => i.Id));
                var results = _mapper.Map<IList<UserDTO>>(users);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetUsersByDecending)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("UsersById/{id:int}", Name = "GetUsersById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersById(int id)
        {
            try
            {
                var users = await _unitOfWork.Users.Get(q => q.Id == id);
                var result = _mapper.Map<UserDTO>(users);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetUsersById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("UsersByName", Name = "GetUsersByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsersByName([FromBody] ViewByUserNameDTO usersname)
        {
            try
            {
                var users = await _unitOfWork.Users.Get(q => q.UserName == usersname.UserName);
                var result = _mapper.Map<UserDTO>(users);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetUsersByName)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUsers([FromBody] CreateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateUsers) }");
                return BadRequest(ModelState);
            }

            try
            {
                var _user = await _unitOfWork.Users.Get(q => q.UserName == userDTO.UserName);

                if (_user != null)
                {
                    _logger.LogError($"Invalid POST attempt in { nameof(CreateUsers) }");
                    return BadRequest("Submitted data is duplicate!");
                }

                var user = _mapper.Map<User>(userDTO);
                await _unitOfWork.Users.Insert(user);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetUsersById", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateUsers)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUsers(int id, [FromBody] UpdateUserDTO userDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in { nameof(UpdateUsers) }");
                return BadRequest(ModelState);
            }

            try
            {
                var users = await _unitOfWork.Users.Get(q => q.Id == id);

                if (users == null)
                {
                    _logger.LogError($"Invalid PUT attempt in { nameof(UpdateUsers) }");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(userDTO, users);

                _unitOfWork.Users.Update(users);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateUsers)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUsers(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteUsers) }");
                return BadRequest(ModelState);
            }

            try
            {
                var users = await _unitOfWork.Users.Get(q => q.Id == id);

                if (users == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteUsers) }");
                    return BadRequest("Submitted data is invalid!");
                }

                await _unitOfWork.Users.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteUsers)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
