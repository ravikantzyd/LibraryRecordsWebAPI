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
    public class SecurityQuestionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SecurityQuestionsController> _logger;
        private readonly IMapper _mapper;

        public SecurityQuestionsController(IUnitOfWork unitOfWork, ILogger<SecurityQuestionsController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetSecurityQuestions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSecurityQuestions()
        {
            try
            {
                var questions = await _unitOfWork.SecurityQuestions.GetAll();
                var results = _mapper.Map<IList<SecurityQuestionDTO>>(questions);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetSecurityQuestions)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("SecurityQuestionsCount", Name = "GetSecurityQuestionsCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSecurityQuestionsCount()
        {
            try
            {
                var questions = await _unitOfWork.SecurityQuestions.GetCount();
                //var results = _mapper.Map<IList<CustomerDTO>>(SecurityQuestionss);

                return Ok(questions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetSecurityQuestionsCount)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("SecurityQuestionsByDecending", Name = "GetSecurityQuestionsByDecending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSecurityQuestionsByDecending()
        {
            try
            {
                var questions = await _unitOfWork.SecurityQuestions.GetAll(null, q => q.OrderByDescending(i => i.Id));
                var results = _mapper.Map<IList<SecurityQuestionDTO>>(questions);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetSecurityQuestionsByDecending)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("SecurityQuestionsById/{id:int}", Name = "GetSecurityQuestionsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSecurityQuestionsById(int id)
        {
            try
            {
                var questions = await _unitOfWork.SecurityQuestions.Get(q => q.Id == id);
                var result = _mapper.Map<SecurityQuestionDTO>(questions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetSecurityQuestionsById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("SecurityQuestionsByName", Name = "GetSecurityQuestionsByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSecurityQuestionsByName([FromBody] ViewBySecurityQuestionDTO securityQuestionDTO)
        {
            try
            {
                var questions = await _unitOfWork.SecurityQuestions.Get(q => q.Question == securityQuestionDTO.Question);
                var result = _mapper.Map<SecurityQuestionDTO>(questions);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetSecurityQuestionsByName)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("SecurityQuestionsByUserId/{id:int}", Name = "GetSecurityQuestionsByUserId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSecurityQuestionsByUserId(int id)
        {
            try
            {
                var questions = await _unitOfWork.SecurityQuestions.GetAll(q => q.UserId == id);
                var results = _mapper.Map<IList<SecurityQuestionDTO>>(questions);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetSecurityQuestions)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSecurityQuestions([FromBody] CreateSecurityQuestionDTO securityQuestionDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateSecurityQuestions) }");
                return BadRequest(ModelState);
            }

            try
            {
                var _question = await _unitOfWork.SecurityQuestions.Get(q => q.Question == securityQuestionDTO.Question);

                //if (_question != null)
                //{
                //    _logger.LogError($"Invalid POST attempt in { nameof(CreateSecurityQuestions) }");
                //    return BadRequest("Submitted data is duplicate!");
                //}

                var question = _mapper.Map<SecurityQuestion>(securityQuestionDTO);
                await _unitOfWork.SecurityQuestions.Insert(question);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetSecurityQuestionsById", new { id = question.Id }, question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateSecurityQuestions)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSecurityQuestions(int id, [FromBody] UpdateSecurityQuestionDTO questionDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in { nameof(UpdateSecurityQuestions) }");
                return BadRequest(ModelState);
            }

            try
            {
                var questions = await _unitOfWork.SecurityQuestions.Get(q => q.Id == id);

                if (questions == null)
                {
                    _logger.LogError($"Invalid PUT attempt in { nameof(UpdateSecurityQuestions) }");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(questionDTO, questions);

                _unitOfWork.SecurityQuestions.Update(questions);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateSecurityQuestions)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSecurityQuestions(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteSecurityQuestions) }");
                return BadRequest(ModelState);
            }

            try
            {
                var questions = await _unitOfWork.SecurityQuestions.Get(q => q.Id == id);

                if (questions == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteSecurityQuestions) }");
                    return BadRequest("Submitted data is invalid!");
                }

                await _unitOfWork.SecurityQuestions.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteSecurityQuestions)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
