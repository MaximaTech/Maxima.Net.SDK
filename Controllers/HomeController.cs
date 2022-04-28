using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Maxima.Net.SDK.Models;
using Maxima.Net.SDK.Domain.Model;
using Maxima.Net.SDK.Integracao.Api;
using Maxima.Net.SDK.Domain.Utils;
using Maxima.Net.SDK.Integracao.Entidades;
using Maxima.Net.SDK.Integracao.Dto;
using Hangfire;
using Maxima.Net.SDK.Domain.Interfaces;
using System.Threading;

namespace Maxima.Net.SDK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MaximaIntegracao apiMaxima;
        private readonly WorkPubSubMaxima workPubSubMaxima;

        public HomeController(ILogger<HomeController> logger, MaximaIntegracao apiMaxima, WorkPubSubMaxima workPubSubMaxima)
        {
            _logger = logger;
            this.apiMaxima = apiMaxima;
            this.workPubSubMaxima = workPubSubMaxima;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Exemplo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Maxima([Bind("Login,Senha"), FromForm] ConfiguracaoMaxima configuracaoMaxima)
        {
            try
            {
                //Fazendo login na api máxima não sera mais necessário realizar o login novamente, visto que
                //a classe MaximaIntegracao e injetada no container no formato singleton.

                var logado = apiMaxima.RealizarLogin(configuracaoMaxima.Login, configuracaoMaxima.Senha);

                if (logado)
                {
                    //Ativa os ouvintes para receber os pedidos enviados pela máxima.
                    workPubSubMaxima.IniciarOuvintes();

                    //Adiciona a tarefa recorrente ao hangfire
                    //https://www.hangfire.io/
                    RecurringJob.AddOrUpdate<ICidadeApiErp>(x => x.EnviarCidades(CancellationToken.None), "0 23 * * *", timeZone: TimeZoneInfo.Local);

                    //Chama o painel do hangfire onde pode ser consultado a fila de processo.
                    return Redirect("/filas");
                }
                else
                {
                    ModelState.AddModelError("", "Usuário ou senha inválidos, Não foi possivel logar: ");
                    return View(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Não foi possivel logar: " + ex.Message);
                return View(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
