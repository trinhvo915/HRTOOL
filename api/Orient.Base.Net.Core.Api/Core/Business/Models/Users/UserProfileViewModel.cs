using Orient.Base.Net.Core.Api.Core.Entities;
using Orient.Base.Net.Core.Api.Core.Entities.Enums;
using System;

namespace Orient.Base.Net.Core.Api.Core.Business.Models.Users
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel()
        {

        }

        public UserProfileViewModel(User user) : this()
        {
            if (user != null)
            {
                Id = user.Id;
                Name = user.Name;
                Avatar = user.AvatarUrl;
                Mobile = user.Mobile;
                Address = user.Address;
                DateOfBirth = user.DateOfBirth;
                Email = user.Email;
                Gender = user.Gender;
            }
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public UserEnums.Gender Gender { get; set; }
    }
}
