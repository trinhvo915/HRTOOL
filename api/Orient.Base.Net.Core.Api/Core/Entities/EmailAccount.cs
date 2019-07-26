using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Orient.Base.Net.Core.Api.Core.Entities
{
    [Table("EmailAccount")]
    public class EmailAccount : BaseEntity
    {
        public EmailAccount() : base()
        {

        }

        [StringLength(512)]
        public string Email { get; set; }

        [StringLength(512)]
        public string DisplayName { get; set; }

        [StringLength(512)]
        public string Host { get; set; }

        public int Port { get; set; }

        [StringLength(512)]
        public string Username { get; set; }

        [StringLength(512)]
        public string Password { get; set; }

        public bool EnableSsl { get; set; }

        public bool UseDefaultCredentials { get; set; }

        public int TimeOut { get; set; }

        public bool IsDefault { get; set; }

        public virtual ICollection<EmailQueue> EmailQueues { get; set; }
    }
}
