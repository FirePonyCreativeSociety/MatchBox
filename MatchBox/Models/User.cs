using MatchBox.Models;
using MatchBox.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MatchBox.API.Models
{
    public class User : IIntId
    {
        public int UserId { get; set; }

        public bool IsDisabled { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        public ICollection<Claim> Claims { get; set; } = new Collection<Claim>();
        public ICollection<UserGroup> UserGroups { get; set; } = new Collection<UserGroup>();

        int IIntId.GetId() => this.UserId;
    }
}