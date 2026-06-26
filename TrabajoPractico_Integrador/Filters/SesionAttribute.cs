
// Filters/SesionAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TrabajoPractico_Integrador.Filters
    {
        // Requiere solo estar logueado
        public class LoginRequeridoAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                if (context.HttpContext.Session.GetInt32("UsuarioID") == null)
                {
                    context.Result = new RedirectToActionResult("Index", "Login", null);
                }
            }
        }

        // Requiere ser Administrador
        public class SoloAdminAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var rol = context.HttpContext.Session.GetString("UsuarioRol");
                if (context.HttpContext.Session.GetInt32("UsuarioID") == null || rol != "Administrador")
                {
                    // Si está logueado pero no es admin → Acceso denegado
                    // Si no está logueado → Login
                    if (context.HttpContext.Session.GetInt32("UsuarioID") != null)
                        context.Result = new RedirectToActionResult("Denegado", "Login", null);
                    else
                        context.Result = new RedirectToActionResult("Index", "Login", null);
                }
            }
        }

        // Requiere ser Vendedor (o Admin también puede)
        public class SoloVendedorAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var rol = context.HttpContext.Session.GetString("UsuarioRol");
                if (context.HttpContext.Session.GetInt32("UsuarioID") == null)
                {
                    context.Result = new RedirectToActionResult("Index", "Login", null);
                }
            }
        }
    }


