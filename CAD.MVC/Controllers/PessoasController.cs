using Acessorio.Util;
using Acessorio.Util.Services;
using CAD.Client;
using CAD.Domain.Contracts.Clients;
using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cadastro.MVC.Controllers
{
    public class PessoasController : Controller
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

        private readonly IPessoaClient _pessoaClient;
        private readonly IEnderecoClient _enderecoClient;
        public PessoasController(IPessoaClient pessoaClient, IEnderecoClient enderecoClient)
        {
            _pessoaClient = pessoaClient;
            _enderecoClient = enderecoClient;
        }

        #region Index
        // GET: PessoasController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _pessoaClient.ObterAsync(Token);
                if (result.Succeeded)
                {
                    var pessoas = JsonConvert.DeserializeObject<List<PessoaModel>>(result.ObjectRetorno.ToString());
                    return View(pessoas);
                }
                else return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Details
        // GET: PessoasController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var result = await _pessoaClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var pessoa = JsonConvert.DeserializeObject<PessoaModel>(result.ObjectRetorno.ToString());
                    return View(pessoa);
                }
                else
                    return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Create
        // GET: PessoasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PessoaModel pessoa)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Pessoa", "Incluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _pessoaClient.InsereAsync(pessoa, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(pessoa);
                }
                return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Edit
        // GET: PessoasController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return await Details(id);
        }

        // POST: PessoasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PessoaModel pessoa)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Pessoa", "Alterar");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _pessoaClient.UpdateAsync(id, pessoa, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));

                if ((ETipoErro)result.ObjectResult == ETipoErro.Sistema)
                {
                    foreach (var erro in result.Errors) { ModelState.AddModelError("Nome", erro); }
                    return View(pessoa);
                }
                return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Delete
        // GET: PessoasController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return await Details(id);
        }

        // POST: PessoasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, PessoaModel pessoa)
        {
            try
            {
                var mensagem = Seguranca.TemPermissao("Pessoa", "Excluir");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _pessoaClient.RemoveAsync(id, Token);
                if (result.Succeeded) return RedirectToAction(nameof(Index));
                else
                    return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region IndexEndereco
        // GET: PessoasController/IndexEndereco
        [AllowAnonymous]
        public async Task<ActionResult> IndexEndereco(int id)
        {
            ViewBag.Id = id;
            try
            {
                var result = await _pessoaClient.ObterAsync(id, Token);
                if (result.Succeeded)
                {
                    var pessoa = JsonConvert.DeserializeObject<PessoaModel>(result.ObjectRetorno.ToString());
                    return View(pessoa);
                }
                else return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }
        #endregion

        #region EditEndereco
        // GET: PessoasController/EditEndereco
        [AllowAnonymous]
        public async Task<ActionResult> EditEndereco(int enderecoId, int pessoaId)
        {
            ViewBag.Id = pessoaId;
            try
            {
                ViewBag.Tipos = EnumService<ETipoEndereco>.GetOptions<ETipoEndereco>();

                var ep = new EnderecoPessoaModel();

                var result = await _pessoaClient.ObterAsync(pessoaId, Token);
                if (result.Succeeded)
                {
                    ep = (new EnderecoPessoaModel()
                    {
                        Pessoa = JsonConvert.DeserializeObject<PessoaModel>(result.ObjectRetorno.ToString()),
                        Endereco = null
                    });
                    ViewBag.Pessoa = ep.Pessoa;
                }
                else return Error(result);

                if (enderecoId == 0) return View(ep);

                result = await _enderecoClient.ObterAsync(enderecoId, Token);
                if (result.Succeeded)
                {
                    ep.Endereco = JsonConvert.DeserializeObject<EnderecoModel>(result.ObjectRetorno.ToString());
                    return View(ep);
                }
                else return Error(result);
            }
            catch { return Error(ETipoErro.Fatal, null); }
        }

        // GET: EmpresasController/EditEndereco
        [HttpPost]
        public async Task<ActionResult> EditEndereco(int id, EEvento eEvento, EnderecoModel enderecoModel)
        {
            try
            {
                enderecoModel.Evento = (int)eEvento;

                var mensagem = Seguranca.TemPermissao("Pessoa", "Associar Endereco");
                if (mensagem != null) return Error(ETipoErro.Sistema, mensagem);

                var result = await _pessoaClient.ManterEnderecoAsync(id, enderecoModel, Token);
                if (result.Succeeded) return RedirectToAction("IndexEndereco", new { Id = id });
                else
                    return Error(result);
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
                ViewBag.ErrorTitle = "Pessoa";
                ViewBag.ErrorMessage = result.Errors[0];
            }
            return View("Error");
        }
        #endregion
    }
}

