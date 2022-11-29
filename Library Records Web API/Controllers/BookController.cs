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
    public class BookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookController> _logger;
        private readonly IMapper _mapper;

        public BookController(IUnitOfWork unitOfWork, ILogger<BookController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                var books = await _unitOfWork.Books.GetAll(null,null,new List<string>
                {
                    "Categories"
                });

                var results = _mapper.Map<IList<BookDTO>>(books);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooks)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("BooksCount", Name = "GetBooksCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksCount()
        {
            try
            {
                var books = await _unitOfWork.Books.GetCount();
                //var results = _mapper.Map<IList<CustomerDTO>>(Bookss);

                return Ok(books);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksCount)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("BooksBySearch", Name = "GetBooksBySearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksBySearch([FromBody] SearchByBookDataDTO searchByBook)
        {
            try
            {
                IList<Book> books = null;

                if (!searchByBook.BookData.Equals(""))
                {
                    string[] words = searchByBook.BookData.Trim().Split(' ');

                    books = await _unitOfWork.Books.GetAll(
                        q => q.BookId.Contains(searchByBook.BookData) ||
                        q.BookName.Contains(searchByBook.BookData) ||
                        q.Author.Contains(searchByBook.BookData) ||
                        q.Categories.CategoryName.Contains(searchByBook.BookData),
                        q => q.OrderBy(s => s.Id),
                        new List<string>
                        {
                            "Categories"
                        });
                }
                else
                {
                    books = await _unitOfWork.Books.GetAll(
                    null,
                    q => q.OrderBy(s => s.Id),
                    new List<string>
                    {
                        "Categories"
                    });
                }

                var results = _mapper.Map<IList<BookDTO>>(books);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksBySearch)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("BooksByDecending", Name = "GetBooksByDecending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksByDecending()
        {
            try
            {
                var books = await _unitOfWork.Books.GetAll(null, q => q.OrderByDescending(i => i.Id),
                                                        new List<string>
                                                        {
                                                            "Categories"
                                                        });
                var results = _mapper.Map<IList<BookDTO>>(books);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksByDecending)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("BooksById/{id:int}", Name = "GetBooksById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksById(int id)
        {
            try
            {
                var books = await _unitOfWork.Books.Get(q => q.Id == id,
                        new List<string>
                        {
                            "Categories"
                        });
                var result = _mapper.Map<BookDTO>(books);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("BooksByName", Name = "GetBooksByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksByName([FromBody] ViewByBookNameDTO booksname)
        {
            try
            {
                var books = await _unitOfWork.Books.Get(q => q.BookName == booksname.BookName,
                    new List<string>
                        {
                            "Categories"
                        });
                var result = _mapper.Map<BookDTO>(books);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksByName)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("BooksByBookId", Name = "GetBooksByBookId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksByBookId([FromBody] ViewByBookIdDTO book_id)
        {
            try
            {
                var books = await _unitOfWork.Books.Get(q => q.BookId == book_id.BookId,
                    new List<string>
                        {
                            "Categories"
                        });
                var result = _mapper.Map<BookDTO>(books);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksByBookId)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("BooksByCategoryId", Name = "GetBooksByCategoryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooksByCategoryId([FromBody] ViewByCategoryIdDTO category_id)
        {
            try
            {
                var books = await _unitOfWork.Books.GetAll(q => q.CategoryId == category_id.CategoryId,null,
                    new List<string>
                        {
                            "Categories"
                        });
                var result = _mapper.Map<IList<BookDTO>>(books);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetBooksByCategoryId)}");
                return StatusCode(500, ex);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBooks([FromBody] CreateBookDTO bookDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateBooks) }");
                return BadRequest(ModelState);
            }

            try
            {
                var _book = await _unitOfWork.Books.Get(q => (q.BookName == bookDTO.BookName) ||
                                                        (q.BookId == bookDTO.BookId));

                if (_book != null)
                {
                    _logger.LogError($"Invalid POST attempt in { nameof(CreateBooks) }");
                    return BadRequest("Submitted data is duplicate!");
                }

                var book = _mapper.Map<Book>(bookDTO);
                await _unitOfWork.Books.Insert(book);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetBooksById", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateBooks)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBooks(int id, [FromBody] UpdateBookDTO bookDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in { nameof(UpdateBooks) }");
                return BadRequest(ModelState);
            }

            try
            {
                var books = await _unitOfWork.Books.Get(q => q.Id == id);

                var book_name = await _unitOfWork.Books.Get(q => q.BookName == bookDTO.BookName);

                if ((books == null) && (book_name != null))
                {
                    _logger.LogError($"Invalid PUT attempt in { nameof(UpdateBooks) }");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(bookDTO, books);

                _unitOfWork.Books.Update(books);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateBooks)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBooks(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteBooks) }");
                return BadRequest(ModelState);
            }

            try
            {
                var books = await _unitOfWork.Books.Get(q => q.Id == id);

                if (books == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteBooks) }");
                    return BadRequest("Submitted data is invalid!");
                }

                await _unitOfWork.Books.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteBooks)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
