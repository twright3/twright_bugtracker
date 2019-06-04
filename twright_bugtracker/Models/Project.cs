using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace twright_bugtracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        [StringLength(80, MinimumLength = 2, ErrorMessage = "This name must be between 3 and 80 characters long.")]
        public string Name { get; set; }

        //Parents


        //Children
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
        }
    }
}