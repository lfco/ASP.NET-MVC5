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

        public void ViewBagRole()
        {
            // Llenado de Combobox de Roles
            var list = db.Roles.ToList();
            list.Add(new IdentityRole { Id = "", Name = "[Seleccione un rol]" });
            list = list.OrderBy(c => c.Name).ToList();
            ViewBag.RoleID = new SelectList(list, "Id", "Name");
        }

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
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

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

            if (user == null)
            {
                return HttpNotFound();
            }

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

        // GET
        public ActionResult AddRole(string userID)
        {
            if (string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Devuelve Lista de Usuarios
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            // Da la lista de todos usuarios
            var users = userManager.Users.ToList();
            // Busca el usuario capturador en el parametro userID
            var user = users.Find(u => u.Id == userID);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };

            // Llenado de Combobox
            ViewBagRole();

            return View(userView);
        }

        [HttpPost]
        public ActionResult AddRole(string userID, FormCollection form)
        {
            var roleID = Request["RoleID"];
            // Devuelve Lista de Usuarios
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            // Da la lista de todos usuarios
            var users = userManager.Users.ToList();
            // Busca el usuario capturador en el parametro userID
            var user = users.Find(u => u.Id == userID);
            // Devuelve Lista de Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };

            if (string.IsNullOrEmpty(roleID))
            {
                ViewBag.Error = "You must select a role";

                if (string.IsNullOrEmpty(userID))
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }

                if (user == null)
                {
                    return HttpNotFound();
                }

                // Llenado de Combobox
                ViewBagRole();

                return View(userView);
            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleID);

            if (!userManager.IsInRole(userID, role.Name))
            {
                userManager.AddToRole(userID, role.Name);
            }

            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
                var roleView = new RoleView
                {
                    RoleID = role.Id,
                    Name = role.Name
                };
                rolesView.Add(roleView);
            }

            userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                Roles = rolesView
            };

            return View("Roles", userView);
        }

        public ActionResult Delete(string userID, string roleID)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(userID))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            // Devuelve Lista de Roles
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            // Devuelve Lista de Usuarios
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.Users.ToList().Find(u => u.Id == userID);
            var role = roleManager.Roles.ToList().Find(r => r.Id == roleID);

            // Eliminar el rol
            if (userManager.IsInRole(user.Id, role.Name))
            {
                userManager.RemoveFromRole(user.Id, role.Name);
            }

            // Prepara la vista

            var users = userManager.Users.ToList();
            // Da la lista de todos los roles
            var roles = roleManager.Roles.ToList();
            // Busca el usuario capturador en el parametro userID
            var rolesView = new List<RoleView>();

            foreach (var item in user.Roles)
            {
                role = roles.Find(r => r.Id == item.RoleId);
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

            return View("Roles", userView);
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