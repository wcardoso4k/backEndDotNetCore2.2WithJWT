using backEnd_Master.Helpers;
using backEndMaster.Context;
using backEndMaster.Helpers;
using backEndMaster.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backEndMaster.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        #region Propriedades privadas

        private readonly ContextDb _contextDb;

        private readonly AppSettings _appSettings;

        #endregion

        #region Construtor

        public UsuarioRepositorio(ContextDb contextDb, IOptions<AppSettings> appSettings)
        {
            _contextDb = contextDb;
            _appSettings = appSettings.Value;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (_contextDb != null)
                _contextDb.Dispose();
        }

        #endregion

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            User user = await GetLoginAsync(username, password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await (from u in _contextDb.Usuarios
                               select new User
                               {
                                   Id = u.Id,
                                   Username = u.Username,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Token = u.Token,
                                   //Password = u.Password // TODO: remover quando realizar deploy, somente para testes !
                               }).AsNoTracking().ToListAsync();
            return users;
        }

        public async Task<User> GetLoginAsync(string username, string password)
        {
            string passwordCripto = SegurancaUtils.CriptografarSenha(password);

            User user = await (from u in _contextDb.Usuarios
                               where u.Username == username && u.Password == passwordCripto
                               select new User
                               {
                                   Id = u.Id,
                                   Username = u.Username,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName,
                                   Token = u.Token,
                                   //Password = u.Password // TODO: remover quando realizar deploy, somente para testes !
                               }).AsNoTracking().SingleOrDefaultAsync();
            return user;
        }

        public async Task RegisterAsync(User model)
        {
            try
            {
                model.Password = string.IsNullOrWhiteSpace(model.Password) ? SegurancaUtils.GerarSenha() : SegurancaUtils.CriptografarSenha(model.Password);
                _contextDb.Usuarios.Add(model);
                await _contextDb.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar usuário.", ex);
            }
        }

        public async Task<User> ForgotPassword(string username)
        {
            User user = await (from u in _contextDb.Usuarios
                               where u.Username == username
                               select new User
                               {
                                   Username = u.Username,
                                   FirstName = u.FirstName,
                                   LastName = u.LastName
                               }).AsNoTracking().SingleOrDefaultAsync();
            return user;
        }
    }
}