namespace WebApi.Dtos
{
    public class UsuarioDto
    {
        public string Email {  get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Nombre { get; set; }  
        public string Apellido { get; set; }
    }


    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
