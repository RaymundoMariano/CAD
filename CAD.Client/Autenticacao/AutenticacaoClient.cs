using CAD.Domain.Contracts.Clients.Autenticacao;
using CAD.Domain.Models.Autenticacao;
using CAD.Domain.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Autenticacao
{
    public class AutenticacaoClient : BaseClient, IAutenticacaoClient
    {
        //public AutenticacaoClient() : base("https://localhost:7108/api/usuarios/") { }
        public AutenticacaoClient() : base("https://autenticacaoapi-behkcsfnbrdkg2hb.brazilsouth-01.azurewebsites.net/api/usuarios/") { }

        #region LoginAsync
        public async Task<RegistroModel> LoginAsync(LoginModel login)
        {
            var registro = new RegistroModel();
            var response = new ResponseModel();
            try
            {
                base.NovaRota("login", null);
                response = Response(await base.Client.PostAsJsonAsync("", login));

                if (response.Errors != null && response.Errors.Count() != 0) throw new ClientException("");

                return JsonConvert.DeserializeObject<RegistroModel>(response.ObjectRetorno.ToString());
            }
            catch (ClientException)
            {
                registro.Errors = response.Errors;
                return registro;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region RegisterAsync
        public async Task<RegistroModel> RegisterAsync(RegisterModel register)
        {
            var registro = new RegistroModel();
            var response = new ResponseModel(); 
            try
            {
                base.NovaRota("register", null);
                response = Response(await base.Client.PostAsJsonAsync("", register));

                return JsonConvert.DeserializeObject<RegistroModel>(response.ObjectRetorno.ToString());
            }
            catch (ClientException)
            {
                registro.Errors = response.Errors;
                return registro;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region TrocaSenhaAsync
        public async Task<List<string>> TrocaSenhaAsync(TrocaSenhaModel trocaSenha)
        {
            var response = new ResponseModel();
            try
            {
                if (trocaSenha.NovaSenha != trocaSenha.ConfirmeSenha) throw new ClientException(
                    $"Nova senha é diferente da senha de confirmacao!");

                if (trocaSenha.SenhaAtual == trocaSenha.NovaSenha) throw new ClientException(
                    $"Nova senha não pode ser igual a senha atual!");

                base.NovaRota("trocasenha", null);
                response = Response(await base.Client.PostAsJsonAsync("", trocaSenha));

                return response.Errors;
            }
            catch (ClientException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
