using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_bugtracker.Models;

namespace twright_bugtracker.Helpers
{
    public class TicketHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static UserRolesHelper userRolesHelper = new UserRolesHelper();

        public int GetNewTicketStatus(string oldDeveloper, string newDeveloper)
        {
            var newAssignment = string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper);
            var unAssignment = !string.IsNullOrEmpty(oldDeveloper) && string.IsNullOrEmpty(newDeveloper);
            var reAssignment = !string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper) && oldDeveloper != newDeveloper;


            var statusId = -1;
         

            if (newAssignment)
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "new/Unassigned").Id;
            }
                

            else if(unAssignment)
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unassigned").Id;
            }
                

            else if(reAssignment)
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Assigned").Id;
            }
               
            else
            {
                statusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "Unknown").Id;
            }
                

            return statusId;
        }

        public List<Ticket> GetMyTickets()
        {
            var myTickets = new List<Ticket>();
            var currentUser = HttpContext.Current.User.Identity;
            if (currentUser.IsAuthenticated)
            {
                var userId = currentUser.GetUserId();
                var myRole = userRolesHelper.ListUserRoles(userId).FirstOrDefault();
                switch(myRole)
                {
                    case "ProjectManager":
                        var user = db.Users.Find(userId);
                        myTickets = user.Projects.SelectMany(p => p.Tickets).ToList();
                        break;
                    case "Developer":
                        myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
                        break;
                    case "Submitter":
                        myTickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
                        break;

                }

                myTickets = myTickets.Where(t => !t.Deleted).ToList();

            }

            return myTickets;
        }


    }
}