using AutoMapper;
using Hotel.Data;
using Hotel.Dto;
using Hotel.Itnerfaces;
using Hotel.Models;
using Hotel.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : Controller
    {
        private readonly IPlaceRepository _placeRepository;
        private readonly IMapper _mapper;

        public PlaceController(IPlaceRepository placeRepository, IMapper mapper) 
        {
            _placeRepository = placeRepository;
            _mapper = mapper;
        }

        [HttpGet("GetPlaces")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Place>))]
        public IActionResult GetPlaces()
        {
            var places = _mapper.Map<List<PlaceDto>>(_placeRepository.GetPlaces());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(places);
        }

        [HttpGet("GetPlaceById/{placeId}")]
        [ProducesResponseType(200, Type = typeof(Place))]
        [ProducesResponseType(400)]
        public IActionResult GetPlace(Guid placeId)
        {
            if (!_placeRepository.PlaceExists(placeId))
                return NotFound();

            var place = _mapper.Map<PlaceDto>(_placeRepository.GetPlace(placeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(place);
        }

        [HttpGet("GetUsersByPlaceId/{placeId}")]
        [ProducesResponseType(200, Type = typeof(Place))]
        [ProducesResponseType(400)]
        public IActionResult GetUsersByPlaceId(Guid placeId)
        {
            var users = _placeRepository.GetUsersByPlace(placeId);

            if (users == null || !users.Any())
                return NotFound();

            var userDtos = _mapper.Map<List<UserDto>>(users);

            return Ok(userDtos);
        }

        [HttpPost("CreatePlace")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePlace([FromBody] PlaceDto placeCreate)
        {
            if (placeCreate == null)
                return BadRequest(ModelState);

            var place = _placeRepository.GetPlaces()
                .Where(p => p.Id == placeCreate.Id)
                .FirstOrDefault();

            if (place != null)
            {
                ModelState.AddModelError("", "Place is already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var placeMap = _mapper.Map<Place>(placeCreate);

            if (!_placeRepository.CreatePlace(placeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("UpdatePlaceById/{placeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePlace(Guid placeId, [FromBody] PlaceDto updatePlace)
        {
            if (updatePlace == null)
                return BadRequest(ModelState);

            if (placeId != updatePlace.Id)
                return BadRequest(ModelState);

            if (!_placeRepository.PlaceExists(placeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var placeMap = _mapper.Map<Place>(updatePlace);

            if (!_placeRepository.UpdatePlace(placeMap))
            {
                ModelState.AddModelError("", "Something went wrong to updating place!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("DeletePlaceById/{placeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(Guid placeId)
        {
            if (!_placeRepository.PlaceExists(placeId))
                return NotFound();

            var placeToDelete = _placeRepository.GetPlace(placeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_placeRepository.DeletePlace(placeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong to delete place!");
            }

            return NoContent();
        }
    }
}
