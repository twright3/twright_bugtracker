using twright_bugtracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_bugtracker.ViewModels
{
    public class ProjectTicketsViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}