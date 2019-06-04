using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_bugtracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string CommentBody { get; set; }
        public DateTimeOffset Created { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }


        //Parent
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }



        //Children
    }
}