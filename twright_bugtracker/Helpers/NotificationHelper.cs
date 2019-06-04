using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using twright_bugtracker.Models;

namespace twright_bugtracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void TriggerAssignmentNotifications(int ticketId, string oldDeveloper, string newDeveloper)
        {
            //Setup a few simple variables that tell me which situation is afoot
            var newAssignment = string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper);
            var unAssignment = !string.IsNullOrEmpty(oldDeveloper) && string.IsNullOrEmpty(newDeveloper);
            var reAssignment = !string.IsNullOrEmpty(oldDeveloper) && !string.IsNullOrEmpty(newDeveloper) && oldDeveloper != newDeveloper;

            if(newAssignment)
            {
                //Trigger an assignment notification
                AddAssignmentNotification(ticketId, newDeveloper);
            }
            else if(unAssignment)
            {
                //Trigger an unassignment notification
                AddUnAssignmentNotification(ticketId, oldDeveloper);
            }
            else if(reAssignment)
            {
                //Trigger and assignment notification
                AddAssignmentNotification(ticketId, newDeveloper);
                //Trigger an unAssignment notification
                AddUnAssignmentNotification(ticketId, oldDeveloper);
            }

        }

        private void AddAssignmentNotification(int ticketId, string newDeveloper)
        {
            var newNotification = new TicketNotification
            {
                Created = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-4, 0, 0)),
                TicketId = ticketId,
                Unread = true,
                UserId = newDeveloper,
                NotificationBody = $"You have been assigned to Ticket: {ticketId}."
            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

        private void AddUnAssignmentNotification(int ticketId, string oldDeveloper)
        {
            var newNotification = new TicketNotification
            {
                Created = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-4, 0, 0)),
                TicketId = ticketId,
                Unread = true,
                UserId = oldDeveloper,
                NotificationBody = $"You have been unassigned to Ticket: {ticketId}."
            };

            db.TicketNotifications.Add(newNotification);
            db.SaveChanges();
        }

    }
}