using F_Store.Models;
using F_Store.ModelView;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace F_Store.Controllers
{
    public class UsersController : Controller
    {
        // Conexión a Base de datos
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index()
        {
            // Devuelve Lista de Usuarios
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = userManager.Users.ToList();
            var usersView = new List<UserView>();

            foreach (var user in users)
            {
                var userView = new UserView
                {
                    Email = user.Email,
                    Name = user.UserName,
                    UserID = user.Id
                };
                usersView.Add(userView);
            }

            return View(usersView);
        }

        public ActionResult Roles(string userID)
        {
            // Devuelve Lista de Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            // Devuelve Lista de Usuarios
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            // Da la lista de todos usuarios
            var users = userManager.Users.ToList();
            // Da la lista de todos los roles
            var roles = roleManager.Roles.ToList();
            // Busca el usuario capturador en el parametro userID
            var user = users.Find(u => u.Id == userID);

            var rolesView = new List<RoleView>();
            foreach (var item in user.Roles)
            {
                var role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View(userView);
        }

        // Cierra la conexión de base de datos
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}