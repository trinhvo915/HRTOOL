using Orient.Base.Net.Core.Api.Core.Business.Models.Roles;
using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;
using System.Linq;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
	public class UserViewModel
	{
		public UserViewModel()
		{

		}

		public UserViewModel(User user) : this()
		{
			if (user != null)
			{
				Id = user.Id;
				Name = user.Name;
				Mobile = user.Mobile;
				Email = user.Email;
				Avatar = user.AvatarUrl;
				DateOfBirth = user.DateOfBirth;
				JoinDate = user.CreatedOn;
				Gender = user.Gender;
				Roles = user.UserInRoles != null ? user.UserInRoles.Select(y => new RoleViewModel(y.Role)).ToArray() : null;
                Color = user.Color;
            }
		}

		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Mobile { get; set; }

		public string Email { get; set; }

		public string Avatar { get; set; }

		public string Jobs { get; set; }

		public DateTime? DateOfBirth { get; set; }

		public DateTime? JoinDate { get; set; }

		public UserEnums.Gender Gender { get; set; }

        public RoleViewModel[] Roles { get; set; }

        public string Color { get; set; }

    }
}
