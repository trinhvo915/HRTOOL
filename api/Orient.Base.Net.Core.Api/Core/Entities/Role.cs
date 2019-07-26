using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("Role")]
    public class Role : BaseEntity
    {
        public Role() : base()
        {

        }

        [StringLength(512)]
        [Required]
        public string Name { get; set; }

        public List<UserInRole> UserInRoles { get; set; }
    }
}
