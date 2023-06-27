using FirstApi.Data;
using FirstApi.Models;
using FirstApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserApiController : ControllerBase
	{
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<IEnumerable<UserDto>> GetUsers()
		{
			return Ok(UserStore.userList);
		}

		[HttpGet("{id:int}", Name = "GetUserById")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<UserDto> GetUserById(int id)
		{
			if(id == 0)
			{
				return BadRequest();
			}
			var users = UserStore.userList.FirstOrDefault(u => u.Id == id);
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
			if(UserStore.userList.FirstOrDefault(u => u.Name.ToLower() == userDto.Name.ToLower()) != null)
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
			userDto.Id = UserStore.userList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
			UserStore.userList.Add(userDto);

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
			var user = UserStore.userList.FirstOrDefault(u => u.Id == id);
			if(user == null)
			{
				return NotFound();
			}
			UserStore.userList.Remove(user);
			return NoContent();
		}
	}
}
