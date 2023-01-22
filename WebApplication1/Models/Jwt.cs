using System.Security.Claims;

namespace WebApplication1.Models;

public class Jwt
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Subject { get; set; }


    public static dynamic validarToken(ClaimsIdentity identity)
    {
        try
        {
            if (identity.Claims.Count() == 0)
            {
                return new
                {
                    success = false,
                    message = "verificar si estas enviando un token valido",
                    result = ""
                };
            }

            var id = identity.Claims.FirstOrDefault(x => x.Type == "id").Value;
            Usuario usuario = Usuario.DB().FirstOrDefault(x => x.IdUsuario == id);

            return new
            {
                success = true,
                message = "exito",
                result = usuario
            };
        }
        catch (Exception ex)
        {
            return new
            {
                success = false,
                message = "Catch: " + ex.Message,
                result = ""
            };
        }
    }
}