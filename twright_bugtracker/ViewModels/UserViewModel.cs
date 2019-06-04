﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace twright_bugtracker.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public HttpPostedFileBase Avatar { get; set; }


        public string ProfilePic { get; set; }
    }
}