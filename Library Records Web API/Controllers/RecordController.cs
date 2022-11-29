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
    public class RecordController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RecordController> _logger;
        private readonly IMapper _mapper;

        public RecordController(IUnitOfWork unitOfWork, ILogger<RecordController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetRecords")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecords()
        {
            try
            {
                var records = await _unitOfWork.Records.GetAll(null,
                    q=> (IOrderedQueryable<Record>)q.Select(r=>new Record
                    {
                        Id = r.Id,
                        RecordId = r.RecordId,
                        MemberId = r.MemberId,
                        Members = r.Members,
                        BookId = r.BookId,
                        Books = r.Books,
                        BorrowDate = r.BorrowDate,
                        ReturnDate = r.ReturnDate,
                        DateExtended = r.DateExtended,
                        UserId = r.UserId,
                        Users = r.Users
                    }),
                    new List<string> { "Members","Books"});
                var results = _mapper.Map<IList<RecordDTO>>(records);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecords)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("RecordsCount", Name = "GetRecordsCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordsCount()
        {
            try
            {
                var records = await _unitOfWork.Records.GetCount();
                //var results = _mapper.Map<IList<CustomerDTO>>(Recordss);

                return Ok(records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordsCount)}");
                return StatusCode(500, ex);
            }
        }
        
        [HttpGet("RecordsByDecending", Name = "GetRecordsByDecending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordsByDecending()
        {
            try
            {
                var records = await _unitOfWork.Records.GetAll(null,
                    q => (IOrderedQueryable<Record>)q.OrderByDescending(i => i.Id)
                    .Select(r => new Record
                    {
                        Id = r.Id,
                        RecordId = r.RecordId,
                        MemberId = r.MemberId,
                        Members = r.Members,
                        BookId = r.BookId,
                        Books = r.Books,
                        BorrowDate = r.BorrowDate,
                        ReturnDate = r.ReturnDate,
                        DateExtended = r.DateExtended,
                        UserId = r.UserId,
                        Users = r.Users
                    })
                    , new List<string> { "Members", "Books" });
                var results = _mapper.Map<IList<RecordDTO>>(records);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordsByDecending)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("RecordsById/{id:int}", Name = "GetRecordsById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordsById(int id)
        {
            try
            {
                var records = await _unitOfWork.Records.Get(q => q.Id == id);
                var result = _mapper.Map<RecordDTO>(records);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordsById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("RecordsByBookId/{id:int}", Name = "GetRecordsByBookId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordsByBookId(int id)
        {
            try
            {
                var records = await _unitOfWork.Records.GetAll(q => q.BookId == id);
                var result = _mapper.Map<IList<RecordDTO>>(records);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordsByBookId)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("RecordsByMemberId/{id:int}", Name = "GetRecordsByMemberId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordsByMemberId(int id)
        {
            try
            {
                var records = await _unitOfWork.Records.GetAll(q => q.MemberId == id);
                var result = _mapper.Map<IList<RecordDTO>>(records);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordsByMemberId)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("ViewByRIDandBookName", Name = "GetViewByRIDandBookName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetViewByRIDandBookName([FromBody] ViewByRIDandBookNameDTO record_data)
        {
            try
            {
                var records = await _unitOfWork.Records.GetAll(q => (q.RecordId == record_data.RecordId) &&
                    (q.Books.BookName.Equals(record_data.BookName)),null,
                    new List<string> { "Members", "Books" });

                var result = _mapper.Map<IList<RecordDTO>>(records);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetViewByRIDandBookName)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("ViewByRID", Name = "GetViewByRID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetViewByRID([FromBody] ViewByRIDandBookNameDTO record_data)
        {
            try
            {
                var records = await _unitOfWork.Records.GetAll(q => q.RecordId == record_data.RecordId, null,
                                                        new List<string> { "Members", "Books" });
                var result = _mapper.Map<IList<RecordDTO>>(records);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetViewByRID)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("RecordsBySearch", Name = "GetRecordsBySearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRecordsBySearch([FromBody] SearchByRecordDataDTO searchByRecord)
        {
            try
            {
                IList<Record> records = null;

                if (!searchByRecord.RecordData.Equals(""))
                {
                    string[] words = searchByRecord.RecordData.Trim().Split(' ');

                    records = await _unitOfWork.Records.GetAll(
                        q => q.RecordId.Contains(searchByRecord.RecordData) ||
                        q.Members.MemberName.Contains(searchByRecord.RecordData) ||
                        q.Books.BookName.Contains(searchByRecord.RecordData) ||
                        q.Books.Author.Contains(searchByRecord.RecordData) ||
                        q.Books.BookId.Contains(searchByRecord.RecordData) ||
                        q.Members.MemberId.Contains(searchByRecord.RecordData) ||
                        q.Members.RNPost.Contains(searchByRecord.RecordData),
                        q => (IOrderedQueryable<Record>)q.OrderBy(s => s.Id)
                        .Select(r => new Record
                        {
                            Id = r.Id,
                            RecordId = r.RecordId,
                            MemberId = r.MemberId,
                            Members = r.Members,
                            BookId = r.BookId,
                            Books = r.Books,
                            BorrowDate = r.BorrowDate,
                            ReturnDate = r.ReturnDate,
                            DateExtended = r.DateExtended,
                            UserId = r.UserId,
                            Users = r.Users
                        }),
                        new List<string> { "Members", "Books" });
                }
                else
                {
                    records = await _unitOfWork.Records.GetAll(null,
                    q => (IOrderedQueryable<Record>)q.Select(r => new Record
                    {
                        Id = r.Id,
                        RecordId = r.RecordId,
                        MemberId = r.MemberId,
                        Members = r.Members,
                        BookId = r.BookId,
                        Books = r.Books,
                        BorrowDate = r.BorrowDate,
                        ReturnDate = r.ReturnDate,
                        DateExtended = r.DateExtended,
                        UserId = r.UserId,
                        Users = r.Users
                    }),
                    new List<string> { "Members", "Books" });
                }

                var results = _mapper.Map<IList<RecordDTO>>(records);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetRecordsBySearch)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRecords([FromBody] CreateRecordDTO recordDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateRecords) }");
                return BadRequest(ModelState);
            }

            try
            {                
                var record = _mapper.Map<Record>(recordDTO);
                await _unitOfWork.Records.Insert(record);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetRecordsById", new { id = record.Id }, record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateRecords)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRecords(int id, [FromBody] UpdateRecordDTO recordDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in { nameof(UpdateRecords) }");
                return BadRequest(ModelState);
            }

            try
            {
                var records = await _unitOfWork.Records.Get(q => q.Id == id);

                if (records == null)
                {
                    _logger.LogError($"Invalid PUT attempt in { nameof(UpdateRecords) }");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(recordDTO, records);

                _unitOfWork.Records.Update(records);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateRecords)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRecords(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteRecords) }");
                return BadRequest(ModelState);
            }

            try
            {
                var records = await _unitOfWork.Records.Get(q => q.Id == id);

                if (records == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteRecords) }");
                    return BadRequest("Submitted data is invalid!");
                }

                await _unitOfWork.Records.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteRecords)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
