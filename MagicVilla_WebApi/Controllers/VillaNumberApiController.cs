using AutoMapper;
using MagicVilla_WebApi.Models;
using MagicVilla_WebApi.Models.DTOs;
using MagicVilla_WebApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_WebApi.Controllers
{
    [ApiController]
    [Route("/api/villaNumber")]
    public class VillaNumberApiController : ControllerBase

    {
        private readonly IVillaNumberRepo _repo;
        private readonly IMapper _mapper;
        private readonly IVillaRepo _vRepo;
        private readonly ApiResponse _response = new();

        public VillaNumberApiController(IVillaNumberRepo repo, IMapper mapper, IVillaRepo vRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _vRepo = vRepo;
        }



        [HttpGet("GetAll")]

        public async Task<ActionResult<ApiResponse>> GetAll()
        {
            var villas = await _repo.GetAll();
            var villasDto = _mapper.Map<List<VillaNumberDTO>>(villas);
            _response.Result = villasDto;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }

        [HttpGet("{No:int}")]
        public async Task<ActionResult<ApiResponse>> GetByNo([FromRoute] int No)
        {
            var villaNo = await _repo.Get(x => x.VillaNo == No);
            var Dto = _mapper.Map<VillaNumberDTO>(villaNo);
            _response.Result = Dto;
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse>> Create([FromBody] VillaNumberCreateDTO villaNoDto)
        {

            if (await _repo.Get(x => x.VillaNo == villaNoDto.VillaNo) != null)
            {
                return BadRequest("number already exists");
            }
            if (await _vRepo.Get(x => x.Id == villaNoDto.VillaId) == null)
            {
                ModelState.AddModelError("invalid villa id", "villa does not exist");

                return BadRequest(modelState: ModelState);
            }
            if (await _repo.Get(x => x.VillaId == villaNoDto.VillaId) != null)
            {
                ModelState.AddModelError("invalid villa id", "another villa number has that villa id");
                return BadRequest(ModelState);
            }
            var villaobj = _mapper.Map<VillaNumber>(villaNoDto);
            await _repo.Create(villaobj);
            _response.Result = villaNoDto;
            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
        }


        [HttpDelete("delete {vNo:int}")]
        public async Task<ActionResult<ApiResponse>> Delete([FromRoute] int vNo)
        {
            var deleted = await _repo.Get(x => x.VillaNo == vNo);
            if (deleted == null) return NotFound();
            await _repo.Remove(deleted);
            _response.Result = _mapper.Map<VillaNumberDTO>(deleted);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }

        [HttpPut("{No:int}")]
        public async Task<ActionResult<ApiResponse>> Update([FromRoute] int No, [FromBody] VillaNumberUpdateDTO villaNoDto)
        {
            if (await _repo.Get(x => x.VillaNo == No, false) == null) return NotFound();
            if (await _vRepo.Get(x => x.Id == villaNoDto.VillaId) == null)
            {
                ModelState.AddModelError("", "invalid villa id");
                return BadRequest(ModelState);
            }
            var villaExist = await _repo.Get(x => x.VillaId == villaNoDto.VillaId, false);
            if (villaExist != null && villaExist.VillaNo != No)
            {
                ModelState.AddModelError("", "some other villaNumber has that villa id already");
                return BadRequest(ModelState);
            }
            if (No != villaNoDto.VillaNo) return BadRequest();
            var newVillaNo = _mapper.Map<VillaNumber>(villaNoDto);
            await _repo.Update(newVillaNo);
            _response.Result = villaNoDto;
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }
}
