namespace twright_bugtracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using twright_bugtracker.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<twright_bugtracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(twright_bugtracker.Models.ApplicationDbContext context)
        {
           
            // I want to write some code that allow me to introduce a few Roles

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            // I want to write some code that allow me to introduce a few Users

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "travwright3@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "travwright3@gmail.com",
                    UserName = "travwright3@gmail.com",
                    FirstName = "Travis",
                    LastName = "Wright",
                    DisplayName = "twright"
                };

                userManager.Create(user, "TKkhhW41010!");
            }

            var userId = userManager.FindByEmail("travwright3@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");


            if (!context.Users.Any(u => u.Email == "twrightprojectmanager@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "twrightprojectmanager@gmail.com",
                    UserName = "twrightprojectmanager@gmail.com",
                    FirstName = "PM",
                    LastName = "Wright",
                    DisplayName = "PMwright"
                };

                userManager.Create(user, "TKkhhW41010!");
            }

            userId = userManager.FindByEmail("twrightprojectmanager@gmail.com").Id;
            userManager.AddToRole(userId, "ProjectManager");


            if (!context.Users.Any(u => u.Email == "twrightsubmitter@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "twrightsubmitter@gmail.com",
                    UserName = "twrightsubmitter@gmail.com",
                    FirstName = "Submitter",
                    LastName = "Wright",
                    DisplayName = "Subwright"
                };

                userManager.Create(user, "TKkhhW41010!");
            }

            userId = userManager.FindByEmail("twrightsubmitter@gmail.com").Id;
            userManager.AddToRole(userId, "Submitter");


            if (!context.Users.Any(u => u.Email == "twrightdeveloper@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "twrightdeveloper@gmail.com",
                    UserName = "twrightdeveloper@gmail.com",
                    FirstName = "Dev",
                    LastName = "Wright",
                    DisplayName = "Devwright"
                };

                userManager.Create(user, "TKkhhW41010!");
            }

            userId = userManager.FindByEmail("twrightdeveloper@gmail.com").Id;
            userManager.AddToRole(userId, "Developer");

            //Demo Logins
            if (!context.Users.Any(u => u.Email == "DemoAdmin@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "DemoAdmin@mailinator.com",
                    UserName = "DemoAdmin@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                    DisplayName = "DemoAdmin"
                };

                userManager.Create(user, "Abc&123!");
            }

            userId = userManager.FindByEmail("DemoAdmin@mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");


            if (!context.Users.Any(u => u.Email == "DemoProjectManager@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "DemoProjectManager@mailinator.com",
                    UserName = "DemoProjectManager@mailinator.com",
                    FirstName = "Demo",
                    LastName = "PM",
                    DisplayName = "DemoPM"
                };

                userManager.Create(user, "Abc&123!");
            }

            userId = userManager.FindByEmail("DemoProjectManager@mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");


            if (!context.Users.Any(u => u.Email == "DemoDeveloper@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "DemoDeveloper@mailinator.com",
                    UserName = "DemoDeveloper@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer",
                    DisplayName = "DemoDev"
                };

                userManager.Create(user, "Abc&123!");
            }

            userId = userManager.FindByEmail("DemoDeveloper@mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");


            if (!context.Users.Any(u => u.Email == "DemoSubmitter@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    Email = "DemoSubmitter@mailinator.com",
                    UserName = "DemoSubmitter@mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter",
                    DisplayName = "DemSub"
                };

                userManager.Create(user, "Abc&123!");
            }

            userId = userManager.FindByEmail("DemoSubmitter@mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            //Add records into the TicketSatus table
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "Immediate", Description = "The highest possible priority"},
                    new TicketPriority { Name = "High", Description = "The 2nd highest possible priority" },
                    new TicketPriority { Name = "Medium", Description = "A mid-level priority to denote average priority" },
                    new TicketPriority { Name = "Low", Description = "The 2nd lowest possible priority" },
                    new TicketPriority { Name = "None", Description = "The highest possible priority" }
                );

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                    new TicketType { Name = "Defect", Description = "A reported defect in a supported project or application" },
                    new TicketType { Name = "New functionality request", Description = "A new request for functionality in a supported project or application" },
                    new TicketType { Name = "Call for documentation", Description = "A request for documentation in a supported project or application" }

                );

            context.TicketStatuses.AddOrUpdate(
                t => t.Name,
                    new TicketStatus { Name = "New/Unassigned", Description = "This will be the status of all newly created Tickets" },
                    new TicketStatus { Name = "Unassigned", Description = "This will be the status of any unassigned Ticket" },
                    new TicketStatus { Name = "Assigned", Description = "This will be the status of Tickets that are assigned to a Developer" },
                    new TicketStatus { Name = "Completed", Description = "This will be the status of all completed created Tickets" },
                    new TicketStatus { Name = "Archived", Description = "This will be the status of all archived created Tickets" },
                    new TicketStatus { Name = "Unknown", Description = "This will be the status of any ticket where the status could not be determined" }
                );








        }
    }
}
