using Acessorio.Util.Services;
using CAD.Domain.Contracts.Clients;
using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cadastro.MVC.Controllers
{
    public class EmpresasController : Controller
    {
        private Seguranca.Domain.Seguranca Seguranca
        {
            get
            {
                return JsonConvert
                    .DeserializeObject<Seguranca.Domain.Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IEmpresaClient _empresaClient;
        public EmpresasController(IEmpresaClient empresaClient)
        {
            _empresaClient = empresaClient;
        }

        #region Index
        // GET: EmpresasController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    var empresas = JsonConvert.DeserializeObject<List<EmpresaModel>>(result.ObjectRetorno.ToString());
                    return View(empresas);
                }
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Details
        // GET: EmpresasController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                ViewBag.Tipos = EnumService<ETipoEmpresa>.GetOptions<ETipoEmpresa>();
                return await GetEmpresa(id);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Create
        // GET: EmpresasController/Create
        public ActionResult Create()
        {
            ViewBag.Tipos = EnumService<ETipoEmpresa>.GetOptions<ETipoEmpresa>();
            return View();
        }

        // POST: EmpresasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmpresaModel empresa)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Incluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.InsereAsync(empresa, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Cgc", erro); }
                    return View(empresa);
                }
                return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Edit
        // GET: EmpresasController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: EmpresasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EmpresaModel empresa)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Alterar");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.UpdateAsync(id, empresa, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Cgc", erro); }
                    return View(empresa);
                }
                return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region Delete
        // GET: EmpresasController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: EmpesasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, EmpresaModel empresa)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Excluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.RemoveAsync(id, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region EditEndereco
        // GET: EmpresasController/EditEndereco
        [AllowAnonymous]
        public async Task<ActionResult> EditEndereco(int id)
        {
            try
            {
                ViewBag.Id = id;
                ViewBag.Tipos = EnumService<ETipoEndereco>.GetOptions<ETipoEndereco>();

                return await GetEmpresa(id);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }

        // GET: EmpresasController/EditEndereco
        [HttpPost]
        public async Task<ActionResult> EditEndereco(int id, EEvento eEvento, EnderecoModel enderecoModel)
        {
            try
            {
                enderecoModel.Evento = (int)eEvento;

                var mensagem = Seguranca.TemPermissao("Empresa", "Associar Endereco");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.ManterEnderecoAsync(id, enderecoModel, Token);
                if (result.Succeeded) return RedirectToAction("EditEndereco", new { Id = id });
                else
                    return Error(result);
            }
            catch
            {
                return Error(ETipoErro.Fatal, null);
            }
        }
        #endregion

        #region IndexFilial
        // GET: EmpresasController/IndexFilial
        [AllowAnonymous]
        public async Task<ActionResult> IndexFilial(int empresaId)
        {
            return await GetEmpresa(empresaId);
        }
        #endregion

        #region EditFilial
        // GET: EmpresasController/EditFilial
        [AllowAnonymous]
        public async Task<ActionResult> EditFilial(int empresaId)
        {
            try
            {
                var result = await _empresaClient.ObterAsync(empresaId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Empresa = JsonConvert.DeserializeObject<EmpresaModel>(result.ObjectRetorno.ToString());
                }
                else return Error(result);

                result = await _empresaClient.ObterFiliaisAsync(empresaId, Token);
                if (result.Succeeded)
                {
                    var filiais = JsonConvert.DeserializeObject<List<EmpresaModel>>(result.ObjectRetorno.ToString());
                    return View(filiais);
                }
                else return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        
        // GET: EmpresasController/EditFilial
        [HttpPost]
        public async Task<ActionResult> EditFilial(int empresaId, List<EmpresaModel> empresasModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Associar Filial");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.ManterFiliaisAsync(empresaId, empresasModel, Token);
                if (result.Succeeded) return RedirectToAction("EditFilial", new { Id = empresaId });
                else
                    return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region IndexSocio
        // GET: EmpresasController/IndexSocio
        [AllowAnonymous]
        public async Task<ActionResult> IndexSocio(int empresaId)
        {
            return await GetEmpresa(empresaId);
        }
        #endregion

        #region EditSocio
        // GET: EmpresasController/EditSocio
        [AllowAnonymous]
        public async Task<ActionResult> EditSocio(int empresaId)
        {
            try
            {
                var result = await _empresaClient.ObterAsync(empresaId, Token);
                if (result.Succeeded)
                {
                    ViewBag.Empresa = JsonConvert.DeserializeObject<EmpresaModel>(result.ObjectRetorno.ToString());
                }
                else return Error(result);

                result = await _empresaClient.ObterSociosAsync(empresaId, Token);
                if (result.Succeeded)
                {
                    var socios = JsonConvert.DeserializeObject<List<PessoaModel>>(result.ObjectRetorno.ToString());
                    return View(socios);
                }
                else return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }

        // GET: EmpresasController/EditSocio
        [HttpPost]
        public async Task<ActionResult> EditSocio(int empresaId, List<PessoaModel> pessoasModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Associar Socio");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _empresaClient.ManterSociosAsync(empresaId, pessoasModel, Token);
                if (result.Succeeded) return RedirectToAction("EditSocio", new { Id = empresaId });
                else
                    return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetEmpresa
        private async Task<ActionResult> GetEmpresa(int empresaId)
        {
            try
            {
                var result = await _empresaClient.ObterAsync(empresaId, Token);
                if (result.Succeeded)
                {
                    var empresa = JsonConvert.DeserializeObject<EmpresaModel>(result.ObjectRetorno.ToString());
                    ViewBag.Empresa = empresa;
                    return View(empresa);
                }
                else return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Error
        private ActionResult Error(ETipoErro eTipoErro, string mensagem)
        {
            return Error(new ResultResponse()
            {
                ObjectResult = (eTipoErro == ETipoErro.Fatal)
                    ? (int)EObjectResult.ErroFatal
                    : (int)eTipoErro,
                Errors = new List<string>() { mensagem }
            });
        }

        private ActionResult Error(ResultResponse result)
        {
            if (result.ObjectResult == (int)EObjectResult.ErroFatal)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = "Empresa";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}