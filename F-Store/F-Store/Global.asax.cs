using F_Store.Data;
using F_Store.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace F_Store
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Habilita las migraciones Automaticas
            Database.SetInitializer
                (new MigrateDatabaseToLatestVersion<Data.F_StoreContext,
                Migrations.Configuration>());

            // Conecta la base de datos
            // Se usa esta DB porque es la que almacena la seguridad
            ApplicationDbContext db = new ApplicationDbContext();

            CreateRoles(db);

            // Super Usuario
            CreateSuperUser(db);
            // Pemisos para Super Usuario
            AddPermisionsToSuperUser(db);

            // Cierra la base de datos
            db.Dispose();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermisionsToSuperUser(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.FindByName("lethal1010@hotmail.com");

            // Verifica si el usuario NO está en el permiso View
            if (!userManager.IsInRole(user.Id, "View"))
            {
                userManager.AddToRole(user.Id, "View");
            }

            // Verifica si el usuario NO está en el permiso Create
            if (!userManager.IsInRole(user.Id, "Create"))
            {
                userManager.AddToRole(user.Id, "Create");
            }

            // Verifica si el usuario NO está en el permiso Delete
            if (!userManager.IsInRole(user.Id, "Delete"))
            {
                userManager.AddToRole(user.Id, "Delete");
            }

            // Verifica si el usuario NO está en el permiso Edit
            if (!userManager.IsInRole(user.Id, "Edit"))
            {
                userManager.AddToRole(user.Id, "Edit");
            }
        }

        private void CreateSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = userManager.FindByName("lethal1010@hotmail.com");

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "lethal1010@hotmail.com",
                    Email = "lethal1010@hotmail.com",
                };
                userManager.Create(user, "IPM2019!");
            }
               
        }

        private void CreateRoles(ApplicationDbContext db)
        {
            // Funcion para insertar los roles si ellos no existen en la base de datos, por eso el ! al inicio
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (!roleManager.RoleExists("View"))
            {
                roleManager.Create(new IdentityRole("View"));
            }

            if (!roleManager.RoleExists("Edit"))
            {
                roleManager.Create(new IdentityRole("Edit"));
            }

            if (!roleManager.RoleExists("Create"))
            {
                roleManager.Create(new IdentityRole("Create"));
            }

            if (!roleManager.RoleExists("Delete"))
            {
                roleManager.Create(new IdentityRole("Delete"));
            }
        }
    }
}
