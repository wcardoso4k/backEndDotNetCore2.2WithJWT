using Newtonsoft.Json;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace backEnd_Test
{
    public class Tests
    {
        const string url = "https://localhost:44391/api/";
        const string urlAuthenticate = "User/authenticate";
        private static readonly HttpClient HttpClient = new HttpClient();

        [SetUp]
        public void Setup()
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.BaseAddress = new System.Uri(url);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public async Task AuthenticateAsyncTest()
        {
            var login = GetLogin("teste", "teste");

            var jsonContent = JsonConvert.SerializeObject(login);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var resultado = await HttpClient.PostAsync(urlAuthenticate, contentString);

            if (resultado.StatusCode != System.Net.HttpStatusCode.OK)
                throw new HttpRequestException($"{resultado.StatusCode}-{resultado.RequestMessage}");

            var retorno = await resultado.Content.ReadAsStringAsync();
            Assert.Fail();
        }

        public AccessCredentialsViewModel GetLogin(string UserName, string password)
        {
            return new AccessCredentialsViewModel { UserName = UserName, Password = password };
        }
    }

    public class AccessCredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}