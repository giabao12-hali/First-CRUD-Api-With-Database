using FirstApi.Models.Dto;

namespace FirstApi.Data
{
	public static class UserStore
	{
		public static List<UserDto> userList = new List<UserDto>
			{
				new UserDto { Id = 1, Name = "Nguyen Gia Bao", Sqft = 100, Occupancy = 1},
				new UserDto { Id = 2, Name = "FirstApi Call", Sqft = 300, Occupancy = 1}
			};
	}
}
