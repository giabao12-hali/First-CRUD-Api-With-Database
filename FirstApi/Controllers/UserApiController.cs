using FirstApi.Data;
using FirstApi.Logging;
using FirstApi.Models;
using FirstApi.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.XPath;

namespace FirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserApiController : ControllerBase
	{
		private readonly ILogging _logger;
		private readonly DataContext _context;
		public UserApiController(ILogging logger, DataContext context)
		{
			_logger = logger;
			_context = context;
		}


		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<UserDto>> GetUsers()
		{
			_logger.Log("Getting All Users", "");
			return Ok(_context.Users.ToList());
		}

		[HttpGet("{id:int}", Name = "GetUserById")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<UserDto> GetUserById(int id)
		{
			if(id == 0)
			{
				_logger.Log("Get User Error by Id - Not Found ID " + id, "error");
				return BadRequest();
			}
			var users = _context.Users.FirstOrDefault(u => u.Id == id);
			if(users == null)
			{
				return NotFound();
			}
			return Ok(users);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<UserDto> CreateUser([FromBody]UserDto userDto)
		{
			//if (!ModelState.IsValid)
			//{
			//	return BadRequest(ModelState);
			//}

			//custom validation
			if(_context.Users.FirstOrDefault(u => u.Name.ToLower() == userDto.Name.ToLower()) != null)
			{
				ModelState.AddModelError("UserError", "Tên đã có!");
				return BadRequest(ModelState);
			}

			if(userDto == null)
			{
				return BadRequest(userDto);
			}
			if(userDto.Id > 0)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
			User model = new()
			{
				Amenity = userDto.Amenity,
				Name = userDto.Name,
				Rate = userDto.Rate,
				Sqft = userDto.Sqft,
				Id = userDto.Id,
				ImageUrl = userDto.ImageUrl,
				Occupancy = userDto.Occupancy,
				Details = userDto.Details,
			};
			_context.Users.Add(model);
			_context.SaveChanges();

			return CreatedAtRoute("GetUserById", new {id = userDto.Id}, userDto);
		}

		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[HttpDelete("{id:int}", Name = "DeleteUserById")]
		public IActionResult DeleteUser(int id)
		{
			if(id == 0)
			{
				return BadRequest();
			}
			var user = _context.Users.FirstOrDefault(u => u.Id == id);
			if(user == null)
			{
				return NotFound();
			}
			_context.Users.Remove(user);
			_context.SaveChanges();
			return NoContent();
		}

		[HttpPut("{id:int}", Name = "UpdateUserOnOneRecord")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdateUserOnOneRecord(int id, [FromBody] UserDto userDto)
		{
			if(userDto == null || id != userDto.Id)
			{
				return BadRequest();
			}
			//var users = UserStore.userList.FirstOrDefault(u => u.Id == id);
			//users.Name = userDto.Name;
			//users.Sqft = userDto.Sqft;
			//users.Occupancy = userDto.Occupancy;
			User model = new()
			{
				Amenity = userDto.Amenity,
				Name = userDto.Name,
				Rate = userDto.Rate,
				Sqft = userDto.Sqft,
				Id = userDto.Id,
				ImageUrl = userDto.ImageUrl,
				Occupancy = userDto.Occupancy,
				Details = userDto.Details,
			};
			_context.Users.Update(model);
			_context.SaveChanges();
			return NoContent();
		}

		[HttpPatch("{id:int}", Name = "UpdatePartialUserOnOneRecord")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult UpdatePartialUserOnOneRecord(int id, JsonPatchDocument<UserDto> pathDTO)
		{
			if(pathDTO == null || id == 0)
			{
				return BadRequest();
			}
			var users = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);

			UserDto userDto = new()
			{
				Amenity = users.Amenity,
				Name = users.Name,
				Rate = users.Rate,
				Sqft = users.Sqft,
				Id = users.Id,
				ImageUrl = users.ImageUrl,
				Occupancy = users.Occupancy,
				Details = users.Details,
			};
			if (users == null)
			{
				return BadRequest();
			}
			pathDTO.ApplyTo(userDto, ModelState);
			User model = new()
			{
				Amenity = userDto.Amenity,
				Name = userDto.Name,
				Rate = userDto.Rate,
				Sqft = userDto.Sqft,
				Id = userDto.Id,
				ImageUrl = userDto.ImageUrl,
				Occupancy = userDto.Occupancy,
				Details = userDto.Details,
			};
			_context.Users.Update(model);
			_context.SaveChanges();

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			return NoContent();
		}
	}
}
