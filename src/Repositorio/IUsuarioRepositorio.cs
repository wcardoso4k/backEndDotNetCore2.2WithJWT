using backEndMaster.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backEndMaster.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetLoginAsync(string username, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task RegisterAsync(User model);
        Task<User> ForgotPassword(string username);
    }
}