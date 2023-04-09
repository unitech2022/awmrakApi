using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Dto;
using awamrakeApi.Data;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace awamrakeApi.Controllers
{

    [Route("slider")]
    [ApiController]
    public class SlidersController : ControllerBase
    {


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AwamrakeApiContext _context;


        private IMapper _mapper;
        public SlidersController(IMapper mapper, AwamrakeApiContext context,IHttpContextAccessor httpContextAccessor)
        {


            _context = context;
            _mapper = mapper;
            _httpContextAccessor=httpContextAccessor;
            
        }



        [HttpGet]
        [Route("get-sliders")]
        public async Task<ActionResult> GetAll()
        {
            var bransItems = await _context.Sliders.ToListAsync();
            return Ok(bransItems);
        }



        [HttpPost]
        [Route("add-slider")]
        public async Task<ActionResult> CreateSlider([FromForm] Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
            _context.SaveChanges();
            return Ok(slider);


        }



        [HttpPost]
        [Route("delete-slider")]
        public ActionResult DeleteCategory([FromForm] int id)
        {

            var favoriteModelFromRepo = _context.Sliders.FirstOrDefault(p =>p.Id==id);
            if (favoriteModelFromRepo == null)
            {
                return NotFound();
            }

            _context.Remove(favoriteModelFromRepo);
            _context.SaveChanges();
            return Ok(favoriteModelFromRepo);



        }




        [HttpPost()]
        [Route("update-slider")]
        public ActionResult UpdateSlider([FromForm] int id, [FromForm] CreateSliderDto category)
        {
            var brandModelFromRepo =_context.Sliders.FirstOrDefault(p =>p.Id==id);
            if (brandModelFromRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(category, brandModelFromRepo);

    
           _context.SaveChangesAsync();

            return NoContent();

        }

    }
}