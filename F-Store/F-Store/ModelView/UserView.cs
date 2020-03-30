using F_Store.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace F_Store.ModelView
{
    public class UserView
    {
        // Se usa string porque el id del memberty de seguridad es un token
        public string UserID { get; set; }
        public string Name { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Para mostrar los roles del usuario
        public List<RoleView> Roles { get; set; }
    }
}