using Microsoft.AspNetCore.Mvc.Filters;
using BLL.Interfaces;
using Models;

namespace API.Filters
{
    namespace API.Filters
    {
        public class EnsureUserExistsFilter : IAsyncActionFilter
        {
            private readonly IUtilizadorBLL _utilizadorBLL;

            public EnsureUserExistsFilter(IUtilizadorBLL utilizadorBLL)
            {
                _utilizadorBLL = utilizadorBLL;
            }

            public async Task OnActionExecutionAsync(
                ActionExecutingContext context,
                ActionExecutionDelegate next)
            {
                var principal = context.HttpContext.User;

                // Só corre se estiver autenticado
                if (!principal.Identity?.IsAuthenticated ?? true)
                {
                    await next();
                    return;
                }

                var userId = principal.FindFirst("sub")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    await next();
                    return;
                }

                var utilizador = new Utilizador
                {
                    Id = userId,
                    Email = principal.FindFirst("https://isi.com/email")?.Value,
                    Nome = principal.FindFirst("https://isi.com/name")?.Value,
                    ImgUrl = principal.FindFirst("https://isi.com/picture")?.Value
                };
                await _utilizadorBLL.RegisterUserAsync(utilizador);

                await next();
            }
        }
    }

}
