using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Models.Aplicacao;
using CAD.Domain.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CAD.Client.Aplicacao
{
    public class Client<T> : BaseClient, IClient<T> where T : _Model
    {
        public Client(string uri) : base(uri) { }

        #region ObterAsync
        public async Task<List<T>> ObterAsync(string token)
        {
            base.NovaRota("", token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<List<T>>(response.ObjectRetorno.ToString());
        }

        public async Task<T> ObterAsync(int id, string token)
        {

            base.NovaRota("/" + id, token);
            var response = await base.Client.GetFromJsonAsync<ResponseModel>("");

            if (!response.Succeeded) throw new Exception();

            return JsonConvert.DeserializeObject<T>(response.ObjectRetorno.ToString());
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(T model, string token)
        {
            base.NovaRota("", token);
            Response(await base.Client.PostAsJsonAsync("", model));
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, T model, string token)
        {
            base.NovaRota("/" + id, token);
            Response(await base.Client.PutAsJsonAsync<T>("", model));
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int id, string token)
        {
            base.NovaRota("/" + id, token);
            Response(await base.Client.DeleteAsync(""));
        }
        #endregion
    }
}
