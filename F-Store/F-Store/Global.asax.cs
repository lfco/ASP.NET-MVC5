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

            // Cierra la base de datos
            db.Dispose();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
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
