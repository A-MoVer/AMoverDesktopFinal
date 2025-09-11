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
        public async Task<IActionResult> IndexOficina()
{
    var hoje = DateTime.Today;
    var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
    var fimMes = inicioMes.AddMonths(1).AddDays(-1);

    // Estatísticas básicas
    ViewBag.TotalIntervencoes = await _context.Servico.CountAsync();
    ViewBag.ServicosHoje = await _context.Servico
        .Where(s => s.DataServico.Date == hoje)
        .CountAsync();
    ViewBag.ServicosAgendados = await _context.Servico
        .Where(s => s.Estado == EstadoServico.Agendado)
        .CountAsync();
    ViewBag.ServicosEmAndamento = await _context.Servico
        .Where(s => s.Estado == EstadoServico.EmCurso)
        .CountAsync();
    ViewBag.ConcluídosHoje = await _context.Servico
        .Where(s => s.DataConclusao.HasValue && s.DataConclusao.Value.Date == hoje)
        .CountAsync();
    ViewBag.MotasEmManutencao = await _context.Servico
        .Where(s => s.Estado == EstadoServico.EmCurso)
        .Select(s => s.IDMota)
        .Distinct()
        .CountAsync();
    ViewBag.IntervencoesEsteMes = await _context.Servico
        .Where(s => s.DataServico >= inicioMes && s.DataServico <= fimMes)
        .CountAsync();
    ViewBag.PecasEsteMes = await _context.ServicosPecasAlteradas
        .Include(spa => spa.Servico)
        .Where(spa => spa.Servico.DataServico >= inicioMes && spa.Servico.DataServico <= fimMes)
        .CountAsync();

    // Próximos serviços (7 dias) - Corrigido para retornar List<object>
    var proximosServicos = await _context.Servico
        .Include(s => s.Mota)
        .Where(s => s.DataServico >= hoje && s.DataServico <= hoje.AddDays(7))
        .OrderBy(s => s.DataServico)
        .Take(5)
        .ToListAsync();

    ViewBag.ProximasIntervencoes = proximosServicos.Select(s => new {
        Id = s.IDServico,
        DataAgendamento = s.DataServico,
        MotaNumero = s.Mota?.NumeroIdentificacao ?? "N/A",
        TipoServico = s.Tipo.ToString(),
        Estado = s.Estado.ToString(),
        CorEstado = s.Estado == EstadoServico.Agendado ? "info" : 
                   s.Estado == EstadoServico.EmCurso ? "warning" : "success"
    }).Cast<object>().ToList();

    return View("IndexOficina");
}
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Oficina"))
        {
            return await IndexOficina();
        }
            // Estatísticas gerais
            var agora = DateTime.Now;
            var inicioMes = new DateTime(agora.Year, agora.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);
            var inicioAno = new DateTime(agora.Year, 1, 1);

            // Estatísticas de Produção
            ViewBag.TotalMotas = await _context.Motas.CountAsync();
            ViewBag.MotasEmProducao = await _context.Motas.CountAsync(m => m.Estado == EstadoMota.EmProdução);
            ViewBag.MotasAtivas = await _context.Motas.CountAsync(m => m.Estado == EstadoMota.Ativo);
            ViewBag.MotasEsteMes = await _context.Motas.CountAsync(m => m.DataRegisto >= inicioMes && m.DataRegisto <= fimMes);
            
            // Calcular crescimento mensal
            var mesAnterior = inicioMes.AddMonths(-1);
            var fimMesAnterior = inicioMes.AddDays(-1);
            var motasMesAnterior = await _context.Motas.CountAsync(m => m.DataRegisto >= mesAnterior && m.DataRegisto <= fimMesAnterior);
            ViewBag.CrescimentoMotas = motasMesAnterior > 0 ? 
                Math.Round(((double)(ViewBag.MotasEsteMes - motasMesAnterior) / motasMesAnterior) * 100, 1) : 0;

            // Estatísticas de Encomendas
            ViewBag.TotalEncomendas = await _context.Encomendas.CountAsync();
            ViewBag.EncomendasPendentes = await _context.Encomendas.CountAsync(e => e.Estado == EstadoEncomenda.Pendente);
            ViewBag.EncomendasEmProducao = await _context.Encomendas.CountAsync(e => e.Estado == EstadoEncomenda.EmProducao);
            ViewBag.EncomendasConcluidas = await _context.Encomendas.CountAsync(e => e.Estado == EstadoEncomenda.Concluida);
            ViewBag.EncomendasEsteMes = await _context.Encomendas.CountAsync(e => e.DateCriacao >= inicioMes && e.DateCriacao <= fimMes);
            
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
            ViewBag.ServicosEsteMes = await _context.Servico.CountAsync(s => s.DataServico >= inicioMes && s.DataServico <= fimMes);

            // Estatísticas de Clientes
            ViewBag.TotalClientes = await _context.Clientes.CountAsync();
            ViewBag.ClientesAtivos = await _context.Clientes.CountAsync(c => true) // Removida a filtragem por Estado que não existe mais
;
            // Dados para gráficos - Últimos 12 meses
            var ultimosMeses = 12;
            var dataInicio = DateTime.Now.AddMonths(-ultimosMeses);
            
            // Produção mensal
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
                
            ViewBag.MesesProducao = producaoMensal.Select(p => $"{p.Mes:D2}/{p.Ano}").ToList();
            ViewBag.ValoresProducao = producaoMensal.Select(p => p.Total).ToList();
            
            // Encomendas mensais
            var encomendaMensal = await _context.Encomendas
                .Where(e => e.DateCriacao >= dataInicio)
                .GroupBy(e => new { Mes = e.DateCriacao.Month, Ano = e.DateCriacao.Year })
                .Select(g => new { 
                    Mes = g.Key.Mes, 
                    Ano = g.Key.Ano, 
                    Total = g.Count() 
                })
                .OrderBy(x => x.Ano)
                .ThenBy(x => x.Mes)
                .ToListAsync();
                
            ViewBag.ValoresEncomendas = encomendaMensal.Select(e => e.Total).ToList();

            // Top 5 modelos mais produzidos
            var topModelos = await _context.Motas
                .Include(m => m.ModeloMota)
                .Where(m => m.ModeloMota != null)
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
                .OrderByDescending(x => x.Total)
                .ToListAsync();
                
            ViewBag.TiposServico = servicosPorTipo.Select(s => s.Tipo.ToString()).ToList();
            ViewBag.ValoresServico = servicosPorTipo.Select(s => s.Total).ToList();

            // Estados das encomendas para gráfico
            ViewBag.EncomendasPorEstado = new int[] { 
                ViewBag.EncomendasPendentes, 
                ViewBag.EncomendasEmProducao, 
                ViewBag.EncomendasConcluidas 
            };

            // Atividades recentes
            var atividadesRecentes = new List<object>();

            // Últimas ordens criadas
            var ultimasOrdens = await _context.OrdemProducao
                .Include(o => o.Encomenda)
                    .ThenInclude(e => e.Cliente)
                .OrderByDescending(o => o.DataCriacao)
                .Take(3)
                .Select(o => new {
                    Id = o.IDOrdemProducao,
                    Tipo = "Ordem de Produção",
                    Descricao = $"OP #{o.NumeroOrdem} - Cliente: {o.Encomenda.Cliente.Nome}",
                    Estado = o.Estado.ToString(),
                    Data = o.DataCriacao,
                    Cor = o.Estado == EstadoOrdemProducao.Pendente ? "info" : 
                          o.Estado == EstadoOrdemProducao.EmProducao ? "warning" : "success"
                })
                .ToListAsync();

            // Últimos serviços
            var ultimosServicos = await _context.Servico
                .Include(s => s.Mota)
                .OrderByDescending(s => s.DataServico)
                .Take(3)
                .Select(s => new {
                    Id = s.IDServico,
                    Tipo = "Serviço",
                    Descricao = $"{s.Tipo} - Mota: {s.Mota.NumeroIdentificacao}",
                    Estado = s.Estado.ToString(),
                    Data = s.DataServico,
                    Cor = s.Estado == EstadoServico.Agendado ? "info" : 
                          s.Estado == EstadoServico.EmCurso ? "warning" : "success"
                })
                .ToListAsync();

            ViewBag.AtividadesRecentes = ultimasOrdens.Cast<object>()
                .Concat(ultimosServicos.Cast<object>())
                .OrderByDescending(a => ((dynamic)a).Data)
                .Take(5)
                .ToList();

            // Alertas e notificações
            var alertas = new List<string>();
            
            if (ViewBag.EncomendasPendentes > 5)
                alertas.Add($"Você tem {ViewBag.EncomendasPendentes} encomendas pendentes que requerem atenção.");
                
            if (ViewBag.ServicosAgendados > 0)
                alertas.Add($"Existem {ViewBag.ServicosAgendados} serviços agendados para os próximos dias.");

            ViewBag.Alertas = alertas;

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