using Domain.Endpoint;
using Domain.Endpoint.DTOs;
using Domain.Endpoint.Entities;
using Domain.Endpoint.Services;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Learn2SwitchApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UserController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public ActionResult<User> Create(UserDTO nuevoUser)
        {
            User newUser = _userService.createUser(nuevoUser);
            return Ok(newUser);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUsuarioById(Guid Id)
        {
            User usuario = _userService.FilterUser(Id);
            return Ok(usuario);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsuario()
        {
            List<User> usuario = await _userService.Get();
            return Ok(usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO campos)
        {
            List<User> usuarios = await _userService.Get();
            ServiceEncryptDecrypt enc = new ServiceEncryptDecrypt();

            bool autenticado = false;
            User authenticatedUser = null;

            foreach (var user in usuarios)
            {
                string password = enc.Decrypt(user.Password, user.Email);
                if (user.Email == campos.Email && password == campos.Password)
                {
                    autenticado = true;
                    authenticatedUser = user;
                    break; // Sal del bucle si la autenticación es exitosa.
                }
            }

            if (autenticado)
            {
                // Generar JWT
                var secretKey = "klhbaihsvkbkhbvhwb"; 
                var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                   new Claim(ClaimTypes.Email, authenticatedUser.Email)
                  
                };

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "Learn2Switch",
                    audience: "Estudents",
                    claims: claims,
                    expires: DateTime.Now.AddHours(24), 
                    signingCredentials: signingCredentials
                );

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.WriteToken(jwtSecurityToken);

                return Ok(new { Token = token });
            }
            else
            {
                return Ok("Login denied");
            }
        }



        [HttpDelete]
        public ActionResult Eliminar(Guid Id)
        {
            _userService.Delete(Id);
            return Ok();
        }

        [HttpPut]
        public ActionResult modificarUsuario(Guid Id, UserDTO nuevosCampos)
        {
            _userService.Update(Id, nuevosCampos);
            return Ok("El usuario ha sido modificado");
        }

    }
}
