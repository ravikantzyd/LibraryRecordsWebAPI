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
    public class RecordNoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecordNoController> _logger;
        private readonly IMapper _mapper;

        public RecordNoController(IUnitOfWork unitOfWork, ILogger<RecordNoController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetRecordNos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordNos()
        {
            try
            {
                var recordNos = await _unitOfWork.RecordNos.GetAll(null, q => q.OrderBy(i => i.Id));
                var results = _mapper.Map<IList<RecordNoDTO>>(recordNos);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordNos)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("RecordNosByDate", Name = "GetRecordNosByDate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordNosByDate([FromBody] ViewRecordNoByDateDTO date)
        {
            try
            {
                var recordNos = await _unitOfWork.RecordNos
                    .GetAll(v => v.Date == date.Date, q => q.OrderBy(i => i.Id));

                var results = _mapper.Map<IList<RecordNoDTO>>(recordNos);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordNosByDate)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("RecordNoById/{id:int}", Name = "GetRecordNoById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordNoById(int id)
        {
            try
            {
                var recordNo = await _unitOfWork.RecordNos.Get(q => q.Id == id);
                var result = _mapper.Map<RecordNoDTO>(recordNo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordNoById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRecordNo([FromBody] CreateRecordNoDTO recordNoDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateRecordNo) }");
                return BadRequest(ModelState);
            }

            try
            {
                var recordNo = _mapper.Map<RecordNo>(recordNoDTO);
                await _unitOfWork.RecordNos.Insert(recordNo);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetRecordNoById", new { id = recordNo.Id }, recordNo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateRecordNo)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
