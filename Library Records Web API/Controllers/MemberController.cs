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
    public class MemberController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<MemberController> _logger;
        private readonly IMapper _mapper;

        public MemberController(IUnitOfWork unitOfWork, ILogger<MemberController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetMembers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembers()
        {
            try
            {
                var members = await _unitOfWork.Members.GetAll();
                var results = _mapper.Map<IList<MemberDTO>>(members);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembers)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("MembersCount", Name = "GetMembersCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembersCount()
        {
            try
            {
                var members = await _unitOfWork.Members.GetCount();
                //var results = _mapper.Map<IList<CustomerDTO>>(Memberss);

                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembersCount)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("MembersBySearch", Name = "GetMembersBySearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembersBySearch([FromBody] SearchByMemberDataDTO searchByMember)
        {
            try
            {
                IList<Member> members = null;

                if (!searchByMember.MemberData.Equals(""))
                {
                    string[] words = searchByMember.MemberData.Trim().Split(' ');

                    members = await _unitOfWork.Members.GetAll(
                        q => q.MemberId.Contains(searchByMember.MemberData) ||
                        q.MemberName.Contains(searchByMember.MemberData) ||
                        q.RNPost.Contains(searchByMember.MemberData) ||
                        q.ClassDepartment.Contains(searchByMember.MemberData),
                        q => q.OrderBy(s => s.Id));
                }
                else
                {
                    members = await _unitOfWork.Members.GetAll(
                    null,
                    q => q.OrderBy(s => s.Id));
                }

                var results = _mapper.Map<IList<MemberDTO>>(members);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembersBySearch)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("MembersByDecending", Name = "GetMembersByDecending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembersByDecending()
        {
            try
            {
                var members = await _unitOfWork.Members.GetAll(null, q => q.OrderByDescending(i => i.Id));
                var results = _mapper.Map<IList<MemberDTO>>(members);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembersByDecending)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("MembersById/{id:int}", Name = "GetMembersById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembersById(int id)
        {
            try
            {
                var members = await _unitOfWork.Members.Get(q => q.Id == id);
                var result = _mapper.Map<MemberDTO>(members);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembersById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("MembersByName", Name = "GetMembersByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembersByName([FromBody] ViewByMemberNameDTO membersname)
        {
            try
            {
                var members = await _unitOfWork.Members.Get(q => q.MemberName == membersname.MemberName);
                var result = _mapper.Map<MemberDTO>(members);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembersByName)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("MembersByMemberId", Name = "GetMembersByMemberId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMembersByMemberId([FromBody] ViewByMemberIdDTO member_id)
        {
            try
            {
                var members = await _unitOfWork.Members.Get(q => q.MemberId == member_id.MemberId);
                var result = _mapper.Map<MemberDTO>(members);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetMembersByMemberId)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMembers([FromBody] CreateMemberDTO memberDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateMembers) }");
                return BadRequest(ModelState);
            }

            try
            {
                var _member = await _unitOfWork.Members.Get(q => (q.MemberId == memberDTO.MemberId) ||
                                                            q.MemberName == memberDTO.MemberName);

                if (_member != null)
                {
                    _logger.LogError($"Invalid POST attempt in { nameof(CreateMembers) }");
                    return BadRequest("Submitted data is duplicate!");
                }

                var member = _mapper.Map<Member>(memberDTO);
                await _unitOfWork.Members.Insert(member);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetMembersById", new { id = member.Id }, member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateMembers)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMembers(int id, [FromBody] UpdateMemberDTO memberDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in { nameof(UpdateMembers) }");
                return BadRequest(ModelState);
            }

            try
            {
                var members = await _unitOfWork.Members.Get(q => q.Id == id);

                var member_name = await _unitOfWork.Members.Get(q => q.MemberName == memberDTO.MemberName);    

                if ((members == null)&&(member_name!=null))
                {
                    _logger.LogError($"Invalid PUT attempt in { nameof(UpdateMembers) }");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(memberDTO, members);

                _unitOfWork.Members.Update(members);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateMembers)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMembers(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteMembers) }");
                return BadRequest(ModelState);
            }

            try
            {
                var members = await _unitOfWork.Members.Get(q => q.Id == id);

                if (members == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteMembers) }");
                    return BadRequest("Submitted data is invalid!");
                }

                await _unitOfWork.Members.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteMembers)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
