using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace A_Mover_Desktop_Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Authentication
        public IActionResult LogIn()
        {
            return View("/Views/Authentication/Login.cshtml");
        }

        public async Task<IActionResult> Index()
        {
            // Estatísticas de Produção
            ViewBag.TotalMotas = await _context.Motas.CountAsync();
            ViewBag.MotasEmProducao = await _context.Motas.CountAsync(m => m.Estado == EstadoMota.EmProdução);
            ViewBag.MotasAtivas = await _context.Motas.CountAsync(m => m.Estado == EstadoMota.Ativo);
            
            // Estatísticas de Encomendas
            ViewBag.TotalEncomendas = await _context.Encomendas.CountAsync();
            ViewBag.EncomendasPendentes = await _context.Encomendas.CountAsync(e => e.Estado == EstadoEncomenda.Pendente);
            ViewBag.EncomendasEmProducao = await _context.Encomendas.CountAsync(e => e.Estado == EstadoEncomenda.EmProducao);
            ViewBag.EncomendasConcluidas = await _context.Encomendas.CountAsync(e => e.Estado == EstadoEncomenda.Concluida);
            
            // Estatísticas de Ordens de Produção
            ViewBag.TotalOrdens = await _context.OrdemProducao.CountAsync();
            ViewBag.OrdensPendentes = await _context.OrdemProducao.CountAsync(o => o.Estado == EstadoOrdemProducao.Pendente);
            ViewBag.OrdensEmProducao = await _context.OrdemProducao.CountAsync(o => o.Estado == EstadoOrdemProducao.EmProducao);
            ViewBag.OrdensConcluidas = await _context.OrdemProducao.CountAsync(o => o.Estado == EstadoOrdemProducao.Concluida);
            
            // Estatísticas de Serviços
            ViewBag.TotalServicos = await _context.Servico.CountAsync();
            ViewBag.ServicosAgendados = await _context.Servico.CountAsync(s => s.Estado == EstadoServico.Agendado);
            ViewBag.ServicosEmCurso = await _context.Servico.CountAsync(s => s.Estado == EstadoServico.EmCurso);
            ViewBag.ServicosConcluidos = await _context.Servico.CountAsync(s => s.Estado == EstadoServico.Concluido);
            
            // Dados para gráficos
            var ultimosMeses = 6;
            var dataInicio = DateTime.Now.AddMonths(-ultimosMeses);
            
            // Produção mensal (últimos 6 meses)
            var producaoMensal = await _context.OrdemProducao
                .Where(o => o.DataConclusao.HasValue && o.DataConclusao >= dataInicio)
                .GroupBy(o => new { Mes = o.DataConclusao.Value.Month, Ano = o.DataConclusao.Value.Year })
                .Select(g => new { 
                    Mes = g.Key.Mes, 
                    Ano = g.Key.Ano, 
                    Total = g.Count() 
                })
                .OrderBy(x => x.Ano)
                .ThenBy(x => x.Mes)
                .ToListAsync();
                
            ViewBag.MesesProducao = producaoMensal.Select(p => $"{p.Mes}/{p.Ano}").ToList();
            ViewBag.ValoresProducao = producaoMensal.Select(p => p.Total).ToList();
            
            // Top 5 modelos mais produzidos
            var topModelos = await _context.Motas
                .GroupBy(m => m.ModeloMota.Nome)
                .Select(g => new {
                    Modelo = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(5)
                .ToListAsync();
                
            ViewBag.TopModelosNomes = topModelos.Select(m => m.Modelo).ToList();
            ViewBag.TopModelosValores = topModelos.Select(m => m.Total).ToList();
            
            // Serviços por tipo
            var servicosPorTipo = await _context.Servico
                .GroupBy(s => s.Tipo)
                .Select(g => new {
                    Tipo = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();
                
            ViewBag.TiposServico = servicosPorTipo.Select(s => s.Tipo.ToString()).ToList();
            ViewBag.ValoresServico = servicosPorTipo.Select(s => s.Total).ToList();
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
