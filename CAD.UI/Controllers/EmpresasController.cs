using CAD.Client;
using CAD.Domain;
using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Enums;
using CAD.Domain.Models.Aplicacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CAD.UI.Controllers
{
    public class EmpresasController : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
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
                if (mensagem != null) return Error(mensagem);

                return View(await _empresaClient.ObterAsync(Token));
            }
            catch (Exception) { return Error(null); }
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
            catch { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Empresa", "Incluir");
                    if (mensagem != null) return Error(mensagem);

                    await _empresaClient.InsereAsync(empresa, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(empresa);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(empresa);
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Empresa", "Alterar");
                    if (mensagem != null) return Error(mensagem);

                    await _empresaClient.UpdateAsync(id, empresa, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(empresa);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(empresa);
            }
            catch (Exception) { return Error(null); }
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
                if (mensagem != null) return Error(mensagem);

                await _empresaClient.RemoveAsync(id, Token);

                return RedirectToAction(nameof(Index));
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }

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
            catch { return Error(null); }
        }

        // GET: EmpresasController/EditEndereco
        [HttpPost]
        public async Task<ActionResult> EditEndereco(int id, EEvento eEvento, EnderecoModel enderecoModel)
        {
            try
            {
                enderecoModel.Evento = (int)eEvento;

                var mensagem = Seguranca.TemPermissao("Empresa", "Associar Endereco");
                if (mensagem != null) return Error(mensagem);

                await _empresaClient.ManterEnderecoAsync(id, enderecoModel, Token);
                
                return RedirectToAction("EditEndereco", new { Id = id });
            }
            catch (Exception) { return Error(null); }
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
                ViewBag.Empresa = await _empresaClient.ObterAsync(empresaId, Token);
                
                return View(await _empresaClient.ObterFiliaisAsync(empresaId, Token));
            }
            catch (Exception) { return Error(null); }
        }

        // GET: EmpresasController/EditFilial
        [HttpPost]
        public async Task<ActionResult> EditFilial(int empresaId, List<FilialModel> filiaisModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Associar Filial");
                if (mensagem != null) return Error(mensagem);

                await _empresaClient.ManterFiliaisAsync(empresaId, filiaisModel, Token);
                
                return RedirectToAction("IndexFilial", new { empresaId = empresaId });
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
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
                ViewBag.Empresa = await _empresaClient.ObterAsync(empresaId, Token);

                return View(await _empresaClient.ObterSociosAsync(empresaId, Token));
            }
            catch (Exception) { return Error(null); }
        }

        // GET: EmpresasController/EditSocio
        [HttpPost]
        public async Task<ActionResult> EditSocio(int empresaId, List<PessoaModel> pessoasModel)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Empresa", "Associar Socio");
                if (mensagem != null) return Error(mensagem);

                await _empresaClient.ManterSociosAsync(empresaId, pessoasModel, Token);
                
                return RedirectToAction("IndexSocio", new { empresaId = empresaId });
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region GetEmpresa
        private async Task<ActionResult> GetEmpresa(int empresaId)
        {
            try
            {
                var empresa = await _empresaClient.ObterAsync(empresaId, Token);
                
                ViewBag.Empresa = empresa;

                return View(empresa);
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Error
        private ActionResult Error(string mensagem)
        {
            if (mensagem == null)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = "Evento";
                ViewBag.ErrorMessage = mensagem;
            }
            return View("Error");
        }
        #endregion
    }
}