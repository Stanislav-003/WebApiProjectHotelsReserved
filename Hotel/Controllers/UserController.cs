using AutoMapper;
using Hotel.Dto;
using Hotel.Itnerfaces;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        //[HttpGet]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        //public IActionResult GetUsers()
        //{
        //    var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers());

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    return Ok(users);
        //}

        [HttpGet("GetUsersWithPlaces")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserWithPlacesDto>))]
        public IActionResult GetUsersWithPlaces()
        {
            var users = _userRepository.GetUsers();

            var usersWithPlaces = _mapper.Map<List<UserWithPlacesDto>>(users);

            foreach (var userDto in usersWithPlaces)
            {
                var places = _userRepository.GetPlacesByUser(userDto.Id);
                userDto.Places = _mapper.Map<List<PlaceDto>>(places);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(usersWithPlaces);
        }

        [HttpGet("GetUserById/{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(Guid userId)
        {
            if (!_userRepository.UserByIdExist(userId))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUserById(userId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("GetUserByEmail/{email}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByEmail(string email)
        {
            if (!_userRepository.UserByEmailExist(email))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUserByEmail(email));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("GetUserByPhone/{phone}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserByPhone(string phone)
        {
            if (!_userRepository.UserByPhoneExist(phone))
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUserByPhone(phone));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(user);
        }

        [HttpGet("GetPlacesByUserId/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Place>))]
        [ProducesResponseType(400)]
        public IActionResult GetPlacesByUser(Guid userId)
        {
            var places = _userRepository.GetPlacesByUser(userId);

            if (places == null || !places.Any())
                return NotFound();

            var placeDtos = _mapper.Map<List<PlaceDto>>(places);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(placeDtos);
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromQuery] Guid placeId, [FromBody] UserDto userCreate)
        {
            if (userCreate == null)
                return BadRequest(ModelState);

            var users = _userRepository.GetUsers()
                .Where(u => u.Email.Trim().ToUpper() == userCreate.Email.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (users != null)
            {
                ModelState.AddModelError("", "User is already exist!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userCreate);

            if (!_userRepository.CreateUser(placeId, userMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser([FromQuery] Guid? placeId, [FromQuery] Guid userId, [FromBody] UserDto updateUser)
        {
            if (updateUser == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            var userMap = _mapper.Map<User>(updateUser);

            if (!_userRepository.UpdateUser(placeId, userId, userMap))
            {
                ModelState.AddModelError("", "Something went wrong to updating user!");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("DeleteUserById/{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(Guid userId)
        {
            if (!_userRepository.UserByIdExist(userId))
                return NotFound();

            var userToDelete = _userRepository.GetUserById(userId);

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", "Something went wrong to delete user!");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
