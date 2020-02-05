using backEndMaster.Modelos;
using backEndMaster.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace backEnd_Master.Controllers
{
    /// <summary>
    /// Para testar colocar no navegaodor https://localhost:44395/swagger ou https://localhost:5001/swagger/index.html    
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Repositório

        private readonly IUsuarioRepositorio _usuarioRepositorio;

        #endregion

        #region Construtor
        public UserController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        #endregion

        #region Métodos públicos

        [AllowAnonymous]
        [HttpPost()]
        [Route("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody]User param)
        {
            var user = await _usuarioRepositorio.AuthenticateAsync(param.Username, param.Password);

            if (user == null)
                return BadRequest(new { message = "Falha ao buscar usuário, dados inválidos." });

            return Ok(user);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("getAll")]
        public async Task<ActionResult> GetAllAsync()
        {
            var users = await _usuarioRepositorio.GetAllAsync();

            return Ok(users);
        }

        // POST        
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] User param)
        {
            if (param == null ||
                string.IsNullOrWhiteSpace(param.Username) &&
                string.IsNullOrWhiteSpace(param.FirstName) &&
                string.IsNullOrWhiteSpace(param.LastName))
            {
                return BadRequest(new { message = "Falha ao adicionar usuários, Dados inválidos." });
            }

            try
            {
                await _usuarioRepositorio.RegisterAsync(param);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao adicionar usuários, Dados inválidos. " + ex.Message });
            }

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("forgot")]
        public async Task<ActionResult> ForgotPasswordAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new { message = "Falha ao buscar usuário, dados inválidos." });
            }

            var user = await _usuarioRepositorio.ForgotPassword(username);

            if (user == null)
                return BadRequest(new { message = "Nenhum login encontrado." });

            return Ok(user);

        }

        #endregion
    }
}