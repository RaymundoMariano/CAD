using CAD.Client;
using CAD.Client.Aplicacao;
using CAD.Client.Autenticacao;
using CAD.Client.Seguranca;
using CAD.Domain;
using CAD.Domain.Contracts.Clients.Aplicacao;
using CAD.Domain.Contracts.Clients.Autenticacao;
using CAD.Domain.Contracts.Clients.Seguranca;
using CAD.Domain.Contracts.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //Serviços da Aplicação
        builder.Services.AddTransient<IPessoaClient, PessoaClient>();
        builder.Services.AddTransient<IEmpresaClient, EmpresaClient>();

        //Serviços do Autenticador
        builder.Services.AddTransient<IAutenticacaoClient, AutenticacaoClient>();

        //Serviços do Seguranca
        builder.Services.AddTransient<ISegurancaClient, SegurancaClient>();

        //Serviço de Unidade de Trabalho
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();


        //Serviço de Cookie
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.SlidingExpiration = true;
                options.LoginPath = "/Conta/Login";
            });

        //Todos os controllers protegidos contra acesso anônimo 
        builder.Services.AddControllersWithViews(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            //app.UseExceptionHandler("/Home/Error"); //<== Original
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseCookiePolicy();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
