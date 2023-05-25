using System.Security.Claims;
using TareasMVC.Servicios.Interface;

namespace TareasMVC.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private HttpContext _httpContext;

        public ServicioUsuarios(IHttpContextAccessor context)
        {
            _httpContext = context.HttpContext;
        }

        public string ObtenerUsuarioId()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                var idClaim = _httpContext.User.Claims
                                .Where(x => x.Type == ClaimTypes.NameIdentifier)
                                .FirstOrDefault();
                return idClaim.Value;
            }
            else
            {
                throw new Exception("El usuario no esta autenticado");
            }
        }
    }
}
