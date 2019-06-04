using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using twright_bugtracker.Helpers;
using twright_bugtracker.Models;

namespace twright_bugtracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private TicketHistoryHelper historyHelper = new TicketHistoryHelper();

        // GET: Tickets
        public ActionResult Index()
        {
            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPrority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            //return View(tickets.ToList());
            
            return View("Index", projectHelper.ListUserTickets(User.Identity.GetUserId()));
        }


        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin, ProjectManager, Developer, Submitter")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            var userRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            var authorized = false;
            switch (userRole)
            {
                case "Submitter":
                    authorized = ticket.OwnerUserId == userId;
                    break;
                case "ProjectManager":
                    var projectIds = db.Users.Find(userId).Projects.Select(p => p.Id).ToList();
                    authorized = projectIds.Contains(ticket.ProjectId);
                    break;
                case "Developer":
                    authorized = ticket.AssignedToUserId == userId;
                    break;
                case "Admin":
                    authorized = true;
                    break;
            }

            if (authorized)
            {
                return View(ticket);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? projectId)
        {
            var myTicket = new Ticket();

            if (projectId == null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
                myTicket.ProjectId = -1;
            }
            else
            {
                    myTicket.ProjectId = (int)projectId;
            }
            
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");   
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            
            return View(myTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = db.TicketStatuses.FirstOrDefault(t => t.Name == "New/Unassigned").Id;
                ticket.Created = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-4, 0, 0));
                ticket.OwnerUserId = User.Identity.GetUserId();

                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }



        // GET: Tickets/Edit/5
        // locking down by roles and user
        [Authorize(Roles ="Developer,Submitter,ProjectManager")]
        public ActionResult Edit(int? id)
        {
            


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);

            var userId = User.Identity.GetUserId();

            //This is usually a students first attempt at kicking people out that are not suppose to be editing
            // see who this is and compare it with the owner user id or assigned to user id and then kick them out if they dont match.
            if ((User.IsInRole("Submitter") && ticket.OwnerUserId != userId) ||
               (User.IsInRole("Developer") && ticket.AssignedToUserId != userId))
            {
                return RedirectToAction("UnauthorizedTicketEdit", "Admin", new { id = ticket.Id});
            }
            

            
            if (ticket == null)
            {
                return HttpNotFound();
            }

            var developers = new List<ApplicationUser>();
            var usersOnProject = projectHelper.UsersOnProject(ticket.ProjectId);
            foreach (var user in usersOnProject)
            {
                if (roleHelper.IsUserInRole(user.Id, "Developer"))
                {
                    developers.Add(user);
                }
            }



            if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
            {
                ViewBag.AssignedToUserId = new SelectList(developers, "Id", "Email", ticket.AssignedToUserId);
            }
            
            ViewBag.ProjectId = new SelectList(projectHelper.ListUserProjects(userId), "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId,Title,Description,Created,Updated")] Ticket ticket)
        {


            if (ModelState.IsValid)
            {


                // var oldAssignedToUserId = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id).AssignedToUserId;
                //var oldTicketStatusId = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id).TicketStatusId;.
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                notificationHelper.TriggerAssignmentNotifications(ticket.Id, oldTicket.AssignedToUserId, ticket.AssignedToUserId);
                //An Assignment has occured
                //TriggerNotifcations()
                //UpdateTicketStatus()
                if(oldTicket.TicketStatusId == ticket.TicketStatusId)
                {
                    ticket.TicketStatusId = ticketHelper.GetNewTicketStatus(oldTicket.AssignedToUserId, ticket.AssignedToUserId);
                }


                //Call our TicketHistroy Helper to record any meaningful changes in property values
                historyHelper.RecordTicketChanges(oldTicket, ticket);


                //Let's get a copy of the ticket before any changes have been saved.
                

                ticket.Updated = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-4, 0, 0));
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "Email", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "Email", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        //GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        //POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            //db.Tickets.Remove(ticket);
            ticket.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
