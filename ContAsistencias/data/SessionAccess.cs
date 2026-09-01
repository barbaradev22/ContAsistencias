using Microsoft.AspNetCore.Http;

namespace ContAsistencias.data
{
    public static class SessionAccess
    {
        public const string RoleKey = "SessionRol";
        public const string CorreoKey = "SessionCorreo";
        public const string NombreKey = "SessionNombre";
        public const string UsuarioIdKey = "SessionIdUsuario";

        public static string? GetRol(this HttpContext context)
        {
            return context.Session.GetString(RoleKey)?.Trim().ToLowerInvariant();
        }

        public static string? GetCorreo(this HttpContext context)
        {
            return context.Session.GetString(CorreoKey);
        }

        public static string? GetNombre(this HttpContext context)
        {
            return context.Session.GetString(NombreKey);
        }

        public static int? GetUsuarioId(this HttpContext context)
        {
            int? idUsuario = context.Session.GetInt32(UsuarioIdKey);
            if (idUsuario.HasValue)
            {
                return idUsuario.Value;
            }

            string? idStr = context.Session.GetString(UsuarioIdKey);
            return int.TryParse(idStr, out int parsedIdUsuario) ? parsedIdUsuario : null;
        }

        public static bool IsAuthenticated(this HttpContext context)
        {
            return !string.IsNullOrWhiteSpace(context.Session.GetString(RoleKey));
        }

        public static bool IsAdmin(this HttpContext context)
        {
            return context.GetRol() == "admin";
        }

        public static bool IsEmpleado(this HttpContext context)
        {
            return context.GetRol() == "empleado";
        }
    }
}
