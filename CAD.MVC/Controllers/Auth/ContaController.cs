﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using SEG.Domain.Contracts.Clients.Auth;
using SEG.Domain.Contracts.Clients;
using Seguranca.Domain.Auth.Requests;
using Seguranca.Domain.Auth.Responses;
using Seguranca.Domain.Aplication.Responses;
using Seguranca.Domain.Models;
using Seguranca.Domain.Enums;

namespace CAD.MVC.Controllers.Auth
{
    [AllowAnonymous]
    public class ContaController : Controller
    {
        private string Email { get { return User.FindFirst(ClaimTypes.Email).Value; } }
        private readonly IRegisterClient _registerClient;
        private readonly ILoginClient _loginClient;
        private readonly ITrocaSenhaClient _trocaSenhaClient;
        private readonly IUsuarioClient _usuarioClient;
        private readonly IModuloClient _moduloClient;
        private readonly IPerfilClient _perfilClient;
        public ContaController(
            IRegisterClient registerClient,
            ILoginClient loginClient,
            ITrocaSenhaClient trocaSenhaClient,
            IUsuarioClient usuarioClient,
            IModuloClient moduloClient,
            IPerfilClient perfilClient)
        {
            _registerClient = registerClient;
            _loginClient = loginClient;
            _trocaSenhaClient = trocaSenhaClient;
            _usuarioClient = usuarioClient;
            _moduloClient = moduloClient;
            _perfilClient = perfilClient;
        }

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequest register)
        {
            register.Modulo = "CadastroUnico";
            var result = ObterResult(await _registerClient.RegisterAsync(register));

            foreach (var erro in result.Errors) { ModelState.AddModelError("UserName", erro); }

            if (ModelState.IsValid)
            {
                var reg = JsonConvert.DeserializeObject<RegisterResponse>(result.ObjectRetorno.ToString());
                if (!await Login(reg, "Register")) return View("Error");
                return RedirectToAction("Index", "Home");
            }
            return View(register);
        }
        #endregion

        #region Login
        [HttpGet]
        public ActionResult Login(string returnURL)
        {
            if (User.Identity.IsAuthenticated) { return RedirectToAction("Index", "Home"); }

            var model = new LoginRequest() { ReturnUrl = returnURL };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest login)
        {
            login.Modulo = "CadastroUnico";
            var result = ObterResult(await _loginClient.LoginAsync(login));

            foreach (var erro in result.Errors) { ModelState.AddModelError("Email", erro); }

            if (ModelState.IsValid)
            {
                var register = JsonConvert.DeserializeObject<RegisterResponse>(result.ObjectRetorno.ToString());
                if (!await Login(register, "Login")) return View("Error");

                if (!string.IsNullOrEmpty(login.ReturnUrl) && Url.IsLocalUrl(login.ReturnUrl))
                {
                    return Redirect(login.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(login);
        }
        #endregion

        #region TrocaSenha
        [HttpGet]
        public ActionResult TrocaSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> TrocaSenha(TrocaSenhaRequest trocaSenha)
        {
            if (trocaSenha.NovaSenha != trocaSenha.ConfirmeSenha)
            {
                ModelState.AddModelError("NovaSenha", "Nova senha é diferente da confirmação!");
                return View(trocaSenha);
            }

            trocaSenha.Email = Email;
            var result = ObterResult(await _trocaSenhaClient.TrocaSenhaAsync(trocaSenha));
            if (result.Succeeded) return RedirectToAction("Index", "Home");

            foreach (var erro in result.Errors) { ModelState.AddModelError("SenhaAtual", erro); }
            return View(trocaSenha);
        }
        #endregion

        #region LogOut
        [HttpGet]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Conta");
        }
        #endregion

        private ResultResponse ObterResult(HttpResponseMessage httpResponse)
        {
            var conteudo = httpResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResultResponse>(conteudo);
        }

        private async Task<bool> Login(RegisterResponse register, string title)
        {
            Seguranca.Domain.Seguranca seguranca;
            try
            {
                seguranca = await NewInstance(register, title);
            }
            catch (Exception) { return false; }  

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, register.UserName),
                new Claim(ClaimTypes.Email, register.Email),
                new Claim(ClaimTypes.Role, "Usuario_Comum"),
                new Claim("Token", register.Token),
                new Claim("Seguranca", JsonConvert.SerializeObject(seguranca))
            };

            var identity = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal claimPrincipal = new ClaimsPrincipal(identity);

            var auth = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(2),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, auth);

            return true;
        }

        #region NewInstance
        private async Task<Seguranca.Domain.Seguranca> NewInstance(RegisterResponse register, string title)
        {
            try
            {
                var usuario = new UsuarioModel();
                var result = await _usuarioClient.ObterAsync(register.Seguranca.UsuarioId, register.Token);
                if (!result.Succeeded) Error(result, title);

                usuario = JsonConvert.DeserializeObject<UsuarioModel>(result.ObjectRetorno.ToString());

                var modulo = new ModuloModel();
                result = await _moduloClient.ObterAsync(register.Seguranca.ModuloId, register.Token);
                if (!result.Succeeded) Error(result, title);
                
                modulo = JsonConvert.DeserializeObject<ModuloModel>(result.ObjectRetorno.ToString());

                var perfil = new PerfilModel();
                result = await _perfilClient.ObterAsync(register.Seguranca.PerfilId, register.Token);
                if (!result.Succeeded) Error(result, title);
                
                perfil = JsonConvert.DeserializeObject<PerfilModel>(result.ObjectRetorno.ToString());

                return new Seguranca.Domain.Seguranca(usuario, modulo, perfil);
            }catch (Exception) { throw; }
        }
        #endregion

        #region Error
        private void Error(ResultResponse result, string title)
        {
            if (result.ObjectResult == (int)EObjectResult.ErroFatal)
            {
                ViewBag.ErrorTitle = null;
            }
            else
            {
                ViewBag.ErrorTitle = title;
                ViewBag.ErrorMessage = result.Errors[0];
            }
            var x = 1; var y = 0; x = x / y;
        }
        #endregion
    }
}
