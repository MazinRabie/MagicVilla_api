using AutoMapper;
using MagicVilla_WebApi.DataStore;
using MagicVilla_WebApi.Logging;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.DTOs;
using MagicVilla_WebApi.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_WebApi.Controllers
{
    [ApiController]
    [Route("api/magicVilla")]
    public class MagicVillaApiController : ControllerBase
    {
        private readonly ILogging logger;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IVillaRepo _repository;
        protected readonly ApiResponse _response;

        public MagicVillaApiController(ILogging logger, ApplicationDbContext context, IMapper mapper, IVillaRepo repository)
        {
            this.logger = logger;
            _context = context;
            _mapper = mapper;
            _repository = repository;
            _response = new ApiResponse();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse>> Gets()
        {
            try
            {

                logger.Log("Getting all villas");
                var villas = await _repository.GetAll();
                _response.Result = _mapper.Map<List<VillaDTO>>(villas);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.ToString() };

            }
            return _response;
        }



        [HttpGet("{id:int}", Name = "Get")]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> Get([FromRoute] int id)
        {
            try
            {

                var villa = await _repository.Get(x => x.Id == id);

                if (villa == null) return NotFound();
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }


            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.ToString() };

            }
            return _response;

        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaDTOCreate villaDto)
        {
            try
            {

                //if(!ModelState.IsValid) return BadRequest(ModelState);
                if (villaDto == null) return BadRequest();
                if (await _repository.Get(x => x.Name == villaDto.Name) != null)
                {
                    ModelState.AddModelError("", "villa name already exists");
                    return BadRequest(ModelState);

                }
                //if (villa.Id != 0) return StatusCode(StatusCodes.Status500InternalServerError);
                ////villa.Id = _context.Villas.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
                var villaObj = _mapper.Map<Villa>(villaDto);
                await _repository.Create(villaObj);
                _response.Result = villaDto;
                _response.StatusCode = HttpStatusCode.Created;
                //return StatusCode(StatusCodes.Status201Created, villa);
                return CreatedAtRoute("Get", new { id = villaObj.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApiResponse>> DeleteVilla([FromRoute] int id)
        {
            try
            {
                var deleted = await _repository.Get(x => x.Id == id);
                if (deleted == null) return NotFound();
                await _repository.Remove(deleted);
                _response.Result = deleted;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }

            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.ToString() };

            }
            return _response;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdateVilla([FromRoute] int id, [FromBody] VillaDTOUpdate villaDto)
        {
            try
            {
                if (villaDto == null || id != villaDto.Id) return BadRequest();
                var villaObj = _mapper.Map<Villa>(villaDto);
                await _repository.Update(villaObj);
                _response.Result = villaDto;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages =
                    new List<string> { ex.ToString() };

            }
            return _response;

        }


        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ApiResponse>> UpdatePatch([FromRoute] int id, JsonPatchDocument<VillaDTOUpdate> patchDocument)
        {
            if (patchDocument == null) return BadRequest();
            var villa = await _repository.Get(x => x.Id == id, false);
            if (villa == null) return BadRequest();
            var villaDto = _mapper.Map<VillaDTOUpdate>(villa);
            patchDocument.ApplyTo(villaDto, ModelState);
            var model = _mapper.Map<Villa>(villaDto);
            if (!ModelState.IsValid) return BadRequest();
            await _repository.Update(model);
            return NoContent();

        }

    }
}
