using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_bugtracker.Models;

namespace twright_bugtracker.Helpers
{
    public class TicketHistoryHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public void RecordTicketChanges(Ticket OldTicket, Ticket newTicket)
        {
            //Compare the oldTicket property values to the new ticket property values
            //If they are different then we add a new TicketHistory record

            // properties TicketTypeId, TicketPriorityId, TicketSatusId, OwnerUserId, AssignedToUserId, Title

            if (OldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                //record a TicketHistory for the title property
                AddTicketHistory(newTicket.Id, "TicketTypeId", OldTicket.TicketTypeId.ToString(), newTicket.TicketTypeId.ToString());

            }
            if (OldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                //record a TicketHistory for the title property
                AddTicketHistory(newTicket.Id, "TicketPriorityId", OldTicket.TicketPriorityId.ToString(), newTicket.TicketPriorityId.ToString());
            }
            if (OldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                //record a TicketHistory for the title property
                AddTicketHistory(newTicket.Id, "TicketStatusId", OldTicket.TicketStatusId.ToString(), newTicket.TicketStatusId.ToString());
            }
            if (OldTicket.OwnerUserId != newTicket.OwnerUserId)
            {
                //record a TicketHistory for the title property
                AddTicketHistory(newTicket.Id, "OwnerUserId", OldTicket.OwnerUserId, newTicket.OwnerUserId);
            }
            if (OldTicket.AssignedToUserId != newTicket.AssignedToUserId)
            {
                //record a TicketHistory for the title property
                AddTicketHistory(newTicket.Id, "AssignedToUserId", OldTicket.AssignedToUserId, newTicket.AssignedToUserId);
            }
            if (OldTicket.Title != newTicket.Title)
            {
                //record a TicketHistory for the title property
                AddTicketHistory(newTicket.Id, "Title", OldTicket.Title, newTicket.Title);
            }


        }


        public void AddTicketHistory(int ticketId, string propertyName, string oldValue, string newValue)
        {
            var newTicketHistory = new TicketHistory
            {
                Property = propertyName,
                OldValue = oldValue,
                NewValue = newValue,
                ChangeDate = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-4, 0, 0)),
                UserId = HttpContext.Current.User.Identity.GetUserId(),
                TicketId = ticketId,
            };

            db.TicketHistories.Add(newTicketHistory);
            db.SaveChanges();


        }


        public static string GetHistoryInfo(string id, string property)
        {
            if(string.IsNullOrEmpty(id))
            {
                return "";
            }
            var info = id;

            switch (property)
            {
               
                case "AssignedToUserId":
                case "OwnerUserId":
                    info = db.Users.Find(id).Email;
                    break;
                case "TicketStatusId":
                    info = db.TicketStatuses.Find(Convert.ToInt32(id)).Name;
                    break;
                case "TicketPriorityId":
                    info = db.TicketPriorities.Find(Convert.ToInt32(id)).Name;
                    break;
                case "TicketTypeId":
                    info = db.TicketTypes.Find(Convert.ToInt32(id)).Name;
                    break;
                default:
                    break;
            }
            return info;
        }
    }
}