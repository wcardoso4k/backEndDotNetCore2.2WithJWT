using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace backEnd_Master.Helpers
{
    public abstract class SegurancaUtils
    {
        private static string charsValidosSenha = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static string regexValidacaoLogin = @"^[a-z][a-z0-9]*[_][a-z0-9]+$";
        public static string regexValidacaoSenha = @"^[a-z0-9]{8,}$";

        public static string GerarSenha()
        {
            var senhaGerada = new char[8];
            var random = new Random();

            for (int i = 0; i < 8; i++)
            {
                char charGerado;

                do
                {
                    charGerado = charsValidosSenha[random.Next(0, charsValidosSenha.Length)];
                }
                while (senhaGerada.Contains(charGerado));

                senhaGerada[i] = charGerado;
            }

            return new string(senhaGerada);
        }

        public static string CriptografarSenha(string senha)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(senha));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }
    }
}
