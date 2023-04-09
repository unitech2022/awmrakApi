using System;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Dto;
using awamrakeApi.Data;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace awamrakeApi.Controllers
{


    [Route("category")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {




        private readonly AwamrakeApiContext _context;
        private IMapper _mapper;
        public CategoriesController(IMapper mapper, AwamrakeApiContext context)
        {

            _mapper = mapper;
            _context = context;

        }



        [HttpGet]
        [Route("get-categories")]
        public async Task<ActionResult> GetAll()
        {
            var data = await _context.Categories.ToListAsync();
            return Ok(data);
        }



        [HttpPost]
        [Route("add-category")]
        public async Task<ActionResult> CreateCategory([FromForm] CreateCategoryDto categoryDto)
        {

            var coomansModel = _mapper.Map<Category>(categoryDto);
            if (categoryDto == null)
            {
                throw new ArgumentNullException(nameof(categoryDto));
            }

            await _context.Categories.AddAsync(coomansModel);
            // var commandReadDto = _mapper.Map<CategoryReadDto>(coomansModel);
            await _context.SaveChangesAsync();

            return Ok(coomansModel);


        }



        [HttpPost]
        [Route("delete-Category")]
        public async Task<ActionResult> DeleteCategory([FromForm] int id)
        {

            var categoryModelFromRepo =await _context.Categories.FirstOrDefaultAsync(p => p.Id==id);
            if (categoryModelFromRepo == null)
            {
                return NotFound();
            }

            _context.Remove(categoryModelFromRepo);
                await _context.SaveChangesAsync();
            return Ok(categoryModelFromRepo);



        }




        [HttpPost]
        [Route("update-Category")]
        public async Task<ActionResult> UpdateCategory([FromForm] int id, [FromForm] CreateCategoryDto category)
        {
            var brandModelFromRepo =await _context.Categories.FirstOrDefaultAsync(p => p.Id==id);
            if (brandModelFromRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(category, brandModelFromRepo);

            // _repository.UpdateCategory(brandModelFromRepo);

          await  _context.SaveChangesAsync();

            return NoContent();

        }

    }



}