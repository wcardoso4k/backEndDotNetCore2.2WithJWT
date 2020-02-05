using Microsoft.VisualStudio.TestTools.UnitTesting;
using backEnd_Master.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using backEndMaster.Repositorio;

namespace backEnd_Master.Controllers.Tests
{
    [TestClass()]
    public class UserControllerUserTests
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public UserControllerUserTests(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        [TestMethod()]
        public async Task AuthenticateAsyncTest()
        {
            UserController userController = new UserController(_usuarioRepositorio);
            await userController.AuthenticateAsync(new backEndMaster.Modelos.User { });
            Assert.Fail();
        }
    }
}