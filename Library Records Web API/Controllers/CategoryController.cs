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
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryController> _logger;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAll();
                var results = _mapper.Map<IList<CategoryDTO>>(categories);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategories)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("CategoriesCount", Name = "GetCategoriesCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesCount()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetCount();
                //var results = _mapper.Map<IList<CustomerDTO>>(Categoriess);

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategoriesCount)}");
                return StatusCode(500, ex);
            }
        }
        
        [HttpGet("CategoriesByDecending", Name = "GetCategoriesByDecending")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesByDecending()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAll(null, q => q.OrderByDescending(i => i.Id));
                var results = _mapper.Map<IList<CategoryDTO>>(categories);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategoriesByDecending)}");
                return StatusCode(500, ex);
            }
        }

        [HttpGet("CategoriesById/{id:int}", Name = "GetCategoriesById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesById(int id)
        {
            try
            {
                var categories = await _unitOfWork.Categories.Get(q => q.Id == id);
                var result = _mapper.Map<CategoryDTO>(categories);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategoriesById)}");
                return StatusCode(500, ex);
            }
        }

        [HttpPost("CategoriesByName", Name = "GetCategoriesByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCategoriesByName([FromBody] ViewByCategoryNameDTO categoriesname)
        {
            try
            {
                var categories = await _unitOfWork.Categories.Get(q => q.CategoryName == categoriesname.CategoryName);
                var result = _mapper.Map<CategoryDTO>(categories);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategoriesByName)}");
                return StatusCode(500, ex);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCategories([FromBody] CreateCategoryDTO memberDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in { nameof(CreateCategories) }");
                return BadRequest(ModelState);
            }

            try
            {
                var _member = await _unitOfWork.Categories.Get(q => q.CategoryName == memberDTO.CategoryName);

                if (_member != null)
                {
                    _logger.LogError($"Invalid POST attempt in { nameof(CreateCategories) }");
                    return BadRequest("Submitted data is duplicate!");
                }

                var member = _mapper.Map<Category>(memberDTO);
                await _unitOfWork.Categories.Insert(member);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCategoriesById", new { id = member.Id }, member);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(CreateCategories)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCategories(int id, [FromBody] UpdateCategoryDTO memberDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid PUT attempt in { nameof(UpdateCategories) }");
                return BadRequest(ModelState);
            }

            try
            {
                var categories = await _unitOfWork.Categories.Get(q => q.Id == id);

                var member_name = await _unitOfWork.Categories.Get(q => q.CategoryName == memberDTO.CategoryName);

                if ((categories == null) && (member_name != null))
                {
                    _logger.LogError($"Invalid PUT attempt in { nameof(UpdateCategories) }");
                    return BadRequest("Submitted data is invalid!");
                }

                _mapper.Map(memberDTO, categories);

                _unitOfWork.Categories.Update(categories);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(UpdateCategories)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategories(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteCategories) }");
                return BadRequest(ModelState);
            }

            try
            {
                var categories = await _unitOfWork.Categories.Get(q => q.Id == id);

                if (categories == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in { nameof(DeleteCategories) }");
                    return BadRequest("Submitted data is invalid!");
                }

                await _unitOfWork.Categories.Delete(id);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteCategories)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
