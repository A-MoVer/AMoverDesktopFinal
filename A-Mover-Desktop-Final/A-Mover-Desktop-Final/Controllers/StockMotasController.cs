using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using A_Mover_Desktop_Final.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class StockMotasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StockMotasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "StockMotas";

            var stock = await _context.StockMotas
                .Include(s => s.ModeloMota)
                .OrderBy(s => s.ModeloMota.Nome)
                .ToListAsync();

            return View(stock);
        }

        public IActionResult Create()
        {
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota.OrderBy(m => m.Nome), "IDModelo", "Nome");
            return View(new StockMota { QuantidadeDisponivel = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDModelo,QuantidadeDisponivel,Observacoes")] StockMota stock)
        {
            if (stock.QuantidadeDisponivel <= 0)
            {
                ModelState.AddModelError("QuantidadeDisponivel", "A quantidade deve ser maior que 0.");
            }

            if (ModelState.IsValid)
            {
                var existente = await _context.StockMotas.FirstOrDefaultAsync(s => s.IDModelo == stock.IDModelo);
                if (existente != null)
                {
                    existente.QuantidadeDisponivel += stock.QuantidadeDisponivel;
                    existente.DataAtualizacao = DateTime.Now;
                    if (!string.IsNullOrWhiteSpace(stock.Observacoes))
                    {
                        existente.Observacoes = stock.Observacoes;
                    }
                    _context.Update(existente);
                }
                else
                {
                    stock.DataCriacao = DateTime.Now;
                    stock.DataAtualizacao = DateTime.Now;
                    _context.Add(stock);
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Stock atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IDModelo"] = new SelectList(_context.ModelosMota.OrderBy(m => m.Nome), "IDModelo", "Nome", stock.IDModelo);
            return View(stock);
        }

        public async Task<IActionResult> Vender(int? id)
        {
            ViewData["ActiveMenu"] = "VenderMota";
            ViewData["IDModelo"] = new SelectList(_context.StockMotas
                .Include(s => s.ModeloMota)
                .Where(s => s.QuantidadeDisponivel > 0)
                .OrderBy(s => s.ModeloMota.Nome)
                .Select(s => new { s.IDModelo, s.ModeloMota.Nome }), "IDModelo", "Nome", id);
            ViewData["IDCliente"] = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "IDCliente", "Nome");

            return View(new VendaMota());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Vender([Bind("IDModelo,IDCliente,ClienteNome,ClienteEmail,ClienteTelefone,ClienteNif,NumeroSerie,Cor,Quilometragem,PrecoVenda,CustoAquisicao,DespesasManutencao,Observacoes")] VendaMota venda)
        {
            var stock = await _context.StockMotas.FirstOrDefaultAsync(s => s.IDModelo == venda.IDModelo);
            if (stock == null || stock.QuantidadeDisponivel <= 0)
            {
                ModelState.AddModelError("IDModelo", "Não existe stock disponível para este modelo.");
            }

            if (venda.IDCliente.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(venda.IDCliente.Value);
                if (cliente != null)
                {
                    venda.ClienteNome = cliente.Nome;
                }
            }
            else if (string.IsNullOrWhiteSpace(venda.ClienteNome))
            {
                ModelState.AddModelError("ClienteNome", "Informe o nome do cliente ou selecione um cliente existente.");
            }

            if (ModelState.IsValid)
            {
                stock!.QuantidadeDisponivel -= 1;
                stock.DataAtualizacao = DateTime.Now;

                venda.DataVenda = DateTime.Now;
                _context.VendasMotas.Add(venda);
                _context.Update(stock);

                await _context.SaveChangesAsync();
                TempData["Success"] = "Venda registada e stock atualizado.";
                return RedirectToAction(nameof(Vendas));
            }

            ViewData["IDModelo"] = new SelectList(_context.StockMotas
                .Include(s => s.ModeloMota)
                .Where(s => s.QuantidadeDisponivel > 0)
                .OrderBy(s => s.ModeloMota.Nome)
                .Select(s => new { s.IDModelo, s.ModeloMota.Nome }), "IDModelo", "Nome", venda.IDModelo);
            ViewData["IDCliente"] = new SelectList(_context.Clientes.OrderBy(c => c.Nome), "IDCliente", "Nome", venda.IDCliente);
            return View(venda);
        }

        public async Task<IActionResult> Vendas()
        {
            ViewData["ActiveMenu"] = "VendasMotas";

            var vendas = await _context.VendasMotas
                .Include(v => v.ModeloMota)
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.DataVenda)
                .ToListAsync();

            return View(vendas);
        }

        public async Task<IActionResult> Rentabilidade()
        {
            ViewData["ActiveMenu"] = "RentabilidadeMotas";

            var vendas = await _context.VendasMotas
                .Include(v => v.ModeloMota)
                .Include(v => v.Cliente)
                .OrderByDescending(v => v.DataVenda)
                .Select(v => new RentabilidadeMotaViewModel
                {
                    IDVendaMota = v.IDVendaMota,
                    Modelo = v.ModeloMota != null ? v.ModeloMota.Nome : string.Empty,
                    Cliente = string.IsNullOrWhiteSpace(v.ClienteNome) ? (v.Cliente != null ? v.Cliente.Nome : string.Empty) : v.ClienteNome,
                    NumeroSerie = v.NumeroSerie,
                    Quilometragem = v.Quilometragem,
                    PrecoVenda = v.PrecoVenda,
                    CustoAquisicao = v.CustoAquisicao,
                    DespesasManutencao = v.DespesasManutencao ?? 0,
                    DataVenda = v.DataVenda
                })
                .ToListAsync();

            return View(vendas);
        }
    }
}
