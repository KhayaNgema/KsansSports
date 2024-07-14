﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyField.Models
{
    public class UserBaseModel : IdentityUser
    {
        public string FirstName { get; set; }   

        public string LastName { get; set; }    

        public DateTime DateOfBirth { get; set; } 

        public string ProfilePicture {  get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public bool IsSuspended { get; set; }

        public bool IsDeleted { get; set; }
    }
}
