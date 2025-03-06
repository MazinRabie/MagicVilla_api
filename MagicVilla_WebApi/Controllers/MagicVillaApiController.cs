using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Logging;
using MagicVilla_WebApi.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_WebApi.Controllers
{
    [ApiController]
    [Route("api/magicVilla")]
    public class MagicVillaApiController : ControllerBase
    {
        private readonly ILogging logger;

        public MagicVillaApiController(ILogging logger)
        {
            this.logger = logger;
        }

        [HttpGet("GetAll")]
        public IActionResult GetVillas()
        {
            logger.Log("Getting all villas");
            return Ok(VillaStore.villaList);

        }



        [HttpGet("{id:int}", Name = "GetVilla")]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVilla([FromRoute] int id)
        {
            var vilal = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (vilal == null) return NotFound();
            return Ok(vilal);

        }

        [HttpPost]
        public IActionResult CreateVilla([FromBody] VillaDTO villa)
        {
            //if(!ModelState.IsValid) return BadRequest(ModelState);
            if (villa == null) return BadRequest();
            if (VillaStore.villaList.FirstOrDefault(x => x.Name == villa.Name) != null)
            {
                ModelState.AddModelError("", "villa name already exists");
                return BadRequest(ModelState);

            }
            if (villa.Id != 0) return StatusCode(StatusCodes.Status500InternalServerError);
            villa.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villa);
            //return StatusCode(StatusCodes.Status201Created, villa);
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteVilla([FromRoute] int id)
        {
            var deleted = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (deleted == null) return NotFound();
            VillaStore.villaList.Remove(deleted);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateVilla([FromRoute] int id, [FromBody] VillaDTO villa)
        {
            if (villa == null || id != villa.Id) return BadRequest();
            var old = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            old.Name = villa.Name;
            old.Occupancy = villa.Occupancy;
            old.sqft = villa.sqft;
            return Ok(villa);

        }


        [HttpPatch("{id:int}")]
        public IActionResult UpdatePatch([FromRoute] int id, JsonPatchDocument<VillaDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (villa == null) return BadRequest();
            patchDocument.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid) return BadRequest();
            return NoContent();

        }

    }
}
