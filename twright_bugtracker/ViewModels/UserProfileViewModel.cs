using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_bugtracker.Models;

namespace twright_bugtracker.ViewModels
{
    public class UserProfileViewModel
    {
        public IndexViewModel IndexViewModel { get; set; }
        public UserViewModel UserViewModel { get; set; }
    }
}