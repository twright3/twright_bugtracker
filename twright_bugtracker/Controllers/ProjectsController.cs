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
using twright_bugtracker.ViewModels;

namespace twright_bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper userRolesHelper = new UserRolesHelper();
        private ProjectHelper projectHelper = new ProjectHelper();

        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }


        // GET: Projects
        [Authorize]
        public ActionResult MyIndex()
        {
            return View("Index", projectHelper.ListUserProjects(User.Identity.GetUserId()));
        }


        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("MyIndex");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            //What users occipy the role of developer?
            var developers = userRolesHelper.UsersInRole("Developer");
            var submitters = userRolesHelper.UsersInRole("Submitter");
            var projectManagers = userRolesHelper.UsersInRole("ProjectManager");

            //Once I assoicate people to the project I will want to see who is on the project. In order to do this I will use the 4th parameter....
            var devsOnProject = new List<string>();
            var subsOnProject = new List<string>();
            var pmOnProject = "";

            foreach (var user in projectHelper.UsersOnProject(project.Id))
            {
                var userRole = userRolesHelper.ListUserRoles(user.Id).FirstOrDefault();
                switch(userRole)
                {
                    case "Developer":
                        devsOnProject.Add(user.Id);
                        break;
                    case "Submitter":
                        subsOnProject.Add(user.Id);
                        break;
                    case "ProjectManager":
                        pmOnProject = user.Id;
                        break;
                    default:
                        break;
                    
                }
            }



            //I need to setup ViewBag properties to hold developers, submitters and PM's
            ViewBag.Developers = new MultiSelectList(developers, "Id", "Email", devsOnProject);
            ViewBag.Submitters = new MultiSelectList(submitters, "Id", "Email", subsOnProject);
            ViewBag.ProjectManager = new SelectList(projectManagers, "Id", "Email", pmOnProject);



            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project, List<string> Developers, List<string> Submitters, string ProjectManager)
        {
            if (ModelState.IsValid)
            {
                //Unassign EVERYONE from the project
                foreach(var user in projectHelper.UsersOnProject(project.Id).ToList())
                {
                    projectHelper.RemoveUserFromProject(user.Id, project.Id);
                }

                //Add back to the project all the selected Developers
                if(Developers != null)
                { 
                    foreach (var userId in Developers)
                    {
                        projectHelper.AddUserToProject(userId, project.Id);
                    }
                }

                //Add back to the project all the selected Submitters
                if (Submitters != null)
                {
                    foreach (var userId in Submitters)
                    {
                        projectHelper.AddUserToProject(userId, project.Id);
                    }
                }

                //Add back to the project all the selected Project Manager
                if (ProjectManager != null)
                {
                    projectHelper.AddUserToProject(ProjectManager, project.Id);
                }


                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyIndex");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Project project = db.Projects.Find(id);
        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(project);
        //}

        // POST: Projects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Project project = db.Projects.Find(id);
        //    db.Projects.Remove(project);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
