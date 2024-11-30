using CAD.Client;
using CAD.Domain;
using CAD.Domain.Contracts.UnitOfWorks;
using CAD.Domain.Enums;
using CAD.Domain.Models.Aplicacao;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CAD.UI.Controllers
{
    public class PessoasController(IUnitOfWork unitOfWork) : Controller
    {
        private Seguranca Seguranca
        {
            get
            {
                return JsonConvert.DeserializeObject<Seguranca>(User.FindFirstValue("Seguranca"));
            }
        }
        private string Token { get { return User.FindFirstValue("Token"); } }

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        #region Index
        // GET: PessoasController
        public async Task<ActionResult> Index()
        {
            try
            {
                var mensagem = Seguranca.TemPermissao();
                if (mensagem != null) return Error(mensagem);

                return View(await _unitOfWork.Pessoas.ObterAsync(Token));
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region Details
        // GET: PessoasController/Details/5
        //[AllowAnonymous]
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                return View(await _unitOfWork.Pessoas.ObterAsync(id, Token));
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Pessoa", "Incluir");
                    if (mensagem != null) return Error(mensagem);

                    await _unitOfWork.Pessoas.InsereAsync(pessoa, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(pessoa);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(pessoa);
            }
            catch (Exception) { return Error(null); }
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
                if (ModelState.IsValid)
                {
                    var mensagem = Seguranca.TemPermissao("Pessoa", "Alterar");
                    if (mensagem != null) return Error(mensagem);

                    await _unitOfWork.Pessoas.UpdateAsync(id, pessoa, Token);

                    return RedirectToAction(nameof(Index));
                }
                return View(pessoa);
            }
            catch (ClientException ex)
            {
                ModelState.AddModelError("Nome", ex.Message);
                return View(pessoa);
            }
            catch (Exception) { return Error(null); }
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
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Pessoas.RemoveAsync(id, Token);
                
                return RedirectToAction(nameof(Index));
            }
            catch (ClientException ex) { return Error(ex.Message); }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region IndexEndereco
        // GET: PessoasController/IndexEndereco
        //[AllowAnonymous]
        public async Task<ActionResult> IndexEndereco(int id)
        {
            ViewBag.Id = id;
            try
            {
                return View(await _unitOfWork.Pessoas.ObterAsync(id, Token));
            }
            catch (Exception) { return Error(null); }
        }
        #endregion

        #region EditEndereco
        // GET: PessoasController/EditEndereco
        //[AllowAnonymous]
        public async Task<ActionResult> EditEndereco(int enderecoId, int pessoaId)
        {
            ViewBag.Id = pessoaId;
            try
            {
                ViewBag.Tipos = EnumService<ETipoEndereco>.GetOptions<ETipoEndereco>();

                var ep = new EnderecoPessoaModel();

                var pessoa = await _unitOfWork.Pessoas.ObterAsync(pessoaId, Token);

                ep = (new EnderecoPessoaModel()
                {
                    Pessoa = pessoa,
                    Endereco = null
                });
                ViewBag.Pessoa = ep.Pessoa;

                if (enderecoId == 0) return View(ep);

                foreach(var end in pessoa.EnderecoPessoas)
                {
                    if (end.EnderecoId == enderecoId)
                    {
                        ep.Endereco = end.Endereco;
                        break;
                    }
                }
                return View(ep);
            }
            catch (Exception) { return Error(null); }
        }

        // GET: EmpresasController/EditEndereco
        [HttpPost]
        public async Task<ActionResult> EditEndereco(int id, EEvento eEvento, EnderecoModel enderecoModel)
        {
            try
            {
                enderecoModel.Evento = (int)eEvento;

                var mensagem = Seguranca.TemPermissao("Pessoa", "Associar Endereco");
                if (mensagem != null) return Error(mensagem);

                await _unitOfWork.Pessoas.ManterEnderecoAsync(id, enderecoModel, Token);
                
                return RedirectToAction("IndexEndereco", new { Id = id });
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

