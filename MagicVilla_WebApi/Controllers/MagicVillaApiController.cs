using AutoMapper;
using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Logging;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_WebApi.Controllers
{
    [ApiController]
    [Route("api/magicVilla")]
    public class MagicVillaApiController : ControllerBase
    {
        private readonly ILogging logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MagicVillaApiController(ILogging logger, ApplicationDbContext context, IMapper mapper)
        {
            this.logger = logger;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetVillas()
        {
            logger.Log("Getting all villas");
            var villas = await _context.Villas.ToListAsync();
            var villasDto = _mapper.Map<List<VillaDTO>>(villas);
            return Ok(villas);

        }



        [HttpGet("{id:int}", Name = "GetVilla")]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVilla([FromRoute] int id)
        {
            var vilal = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (vilal == null) return NotFound();
            return Ok(_mapper.Map<VillaDTO>(vilal));

        }

        [HttpPost]
        public async Task<IActionResult> CreateVilla([FromBody] VillaDTOCreate villaDto)
        {
            //if(!ModelState.IsValid) return BadRequest(ModelState);
            if (villaDto == null) return BadRequest();
            if (await _context.Villas.FirstOrDefaultAsync(x => x.Name == villaDto.Name) != null)
            {
                ModelState.AddModelError("", "villa name already exists");
                return BadRequest(ModelState);

            }
            //if (villa.Id != 0) return StatusCode(StatusCodes.Status500InternalServerError);
            ////villa.Id = _context.Villas.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            var villaObj = _mapper.Map<Villa>(villaDto);

            await _context.Villas.AddAsync(villaObj);
            await _context.SaveChangesAsync();
            //return StatusCode(StatusCodes.Status201Created, villa);
            return CreatedAtRoute("GetVilla", new { id = villaObj.Id }, villaDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVilla([FromRoute] int id)
        {
            var deleted = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (deleted == null) return NotFound();
            _context.Villas.Remove(deleted);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateVilla([FromRoute] int id, [FromBody] VillaDTOUpdate villaDto)
        {
            if (villaDto == null || id != villaDto.Id) return BadRequest();

            var villaObj = _mapper.Map<Villa>(villaDto);
            _context.Villas.Update(villaObj);
            await _context.SaveChangesAsync();
            return Ok(villaDto);

        }


        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdatePatch([FromRoute] int id, JsonPatchDocument<VillaDTOUpdate> patchDocument)
        {
            if (patchDocument == null) return BadRequest();
            var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null) return BadRequest();
            var villaDto = _mapper.Map<VillaDTOUpdate>(villa);

            patchDocument.ApplyTo(villaDto, ModelState);

            var model = _mapper.Map<Villa>(villaDto);

            if (!ModelState.IsValid) return BadRequest();
            _context.Villas.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
