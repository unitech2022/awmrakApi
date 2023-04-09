using System;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Data;
using awamrakeApi.Dto;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace awamrakeApi.Controllers
{


    [Route("cares")]
    [ApiController]
    public class CaresController : ControllerBase
    {




        private readonly AwamrakeApiContext _context;
        private IMapper _mapper;
        public CaresController(IMapper mapper, AwamrakeApiContext context)
        {

            _mapper = mapper;
            _context = context;

        }



        [HttpGet]
        [Route("get-cares")]
        public async Task<ActionResult> GetAll()
        {
            var data = await _context.Cares.ToListAsync();
            return Ok(data);
        }



        [HttpPost]
        [Route("add-Care")]
        public async Task<ActionResult> CreateCare([FromForm] CreateCareDto CareDto)
        {

            var caresModel = _mapper.Map<Care>(CareDto);
            if (CareDto == null)
            {
                throw new ArgumentNullException(nameof(CareDto));
            }

            await _context.Cares.AddAsync(caresModel);
            // var commandReadDto = _mapper.Map<CareReadDto>(coomansModel);
            await _context.SaveChangesAsync();

            return Ok(caresModel);


        }



        [HttpPost]
        [Route("delete-Care")]
        public async Task<ActionResult> DeleteCare([FromForm] int id)
        {

            var CareModelFromRepo =await _context.Cares.FirstOrDefaultAsync(p => p.Id==id);
            if (CareModelFromRepo == null)
            {
                return NotFound();
            }

            _context.Remove(CareModelFromRepo);
                await _context.SaveChangesAsync();
            return Ok(CareModelFromRepo);



        }




        [HttpPost]
        [Route("update-Care")]
        public async Task<ActionResult> UpdateCare([FromForm] int id, [FromForm] CreateCareDto Care)
        {
            var brandModelFromRepo =await _context.Cares.FirstOrDefaultAsync(p => p.Id==id);
            if (brandModelFromRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(Care, brandModelFromRepo);

            // _repository.UpdateCare(brandModelFromRepo);

          await  _context.SaveChangesAsync();

            return NoContent();

        }



            [HttpPost]
        [Route("get-CareById")]
        public async Task<ActionResult> GetCareById([FromForm] int id)
        {

            Care care =await _context.Cares.FirstOrDefaultAsync(p => p.Id == id);

            return Ok(care);




        }

    }



}