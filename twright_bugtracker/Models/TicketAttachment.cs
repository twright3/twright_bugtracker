using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace twright_bugtracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string FilePath { get; set; }
        public string AttachmentDescription { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserId { get; set; }
        


        //Parents
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }



        //Children

    }
}