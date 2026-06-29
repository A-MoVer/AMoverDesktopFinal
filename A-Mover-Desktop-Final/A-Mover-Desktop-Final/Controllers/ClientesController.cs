using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExcelDataReader;
using System.Text;

namespace A_Mover_Desktop_Final.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Clientes";
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.IDCliente == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDCliente,Nome,Tipo,NumeroCliente,Pais,PaisOrigem,Estado")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDCliente,Nome,Tipo,NumeroCliente,Pais,PaisOrigem,Estado")] Cliente cliente)
        {
            if (id != cliente.IDCliente) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IDCliente)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.IDCliente == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
                _context.Clientes.Remove(cliente);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ImportarExcel()
        {
            ViewData["ActiveMenu"] = "Clientes";
            return View();
        }


        // POST: Clientes/ImportarExcel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportarExcel(IFormFile ficheiro)
        {
            if (ficheiro == null || ficheiro.Length == 0)
            {
                TempData["Erro"] = "Por favor selecione um ficheiro Excel válido.";
                return RedirectToAction(nameof(Index));
            }

            var ext = Path.GetExtension(ficheiro.FileName).ToLower();
            if (ext != ".xls" && ext != ".xlsx")
            {
                TempData["Erro"] = "Apenas ficheiros .xls ou .xlsx são suportados.";
                return RedirectToAction(nameof(Index));
            }

            int importados = 0;
            int atualizados = 0;
            int erros = 0;
            var mensagensErro = new List<string>();

            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using var stream = ficheiro.OpenReadStream();
                using var reader = ext == ".xls"
                    ? ExcelReaderFactory.CreateBinaryReader(stream)
                    : ExcelReaderFactory.CreateOpenXmlReader(stream);

                // Percorrer linhas — header na linha 3 (índice 3), dados a partir da linha 5 (índice 5)
                int linhaAtual = 0;
                while (reader.Read())
                {
                    linhaAtual++;
                    if (linhaAtual <= 4) continue; // Ignorar cabeçalho e linhas em branco iniciais

                    try
                    {
                        // Col 1: Número do cliente, Col 2: Nome, Col 3: País morada, Col 4: País origem (código), Col 5: Inativo
                        var numClienteRaw = reader.GetValue(1);
                        var nomeRaw = reader.GetValue(2);
                        var paisRaw = reader.GetValue(3);
                        var paisOrigemRaw = reader.GetValue(4);
                        var inativoRaw = reader.GetValue(5);

                        if (numClienteRaw == null && nomeRaw == null) continue; // linha vazia
                        if (nomeRaw == null) continue;

                        var nome = nomeRaw.ToString()?.Trim();
                        if (string.IsNullOrWhiteSpace(nome)) continue;

                        int? numCliente = null;
                        if (numClienteRaw != null && int.TryParse(numClienteRaw.ToString(), out int n))
                            numCliente = n;

                        var pais = paisRaw?.ToString()?.Trim();
                        var paisOrigem = paisOrigemRaw?.ToString()?.Trim();

                        var inativoStr = inativoRaw?.ToString()?.Trim().ToUpper();
                        var estado = (inativoStr == "SIM") ? EstadoCliente.Inativo : EstadoCliente.Ativo;

                        // Verificar se já existe pelo NumeroCliente ou pelo Nome
                        Cliente? existente = null;
                        if (numCliente.HasValue)
                            existente = await _context.Clientes.FirstOrDefaultAsync(c => c.NumeroCliente == numCliente);

                        if (existente == null)
                            existente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nome == nome);

                        if (existente != null)
                        {
                            existente.NumeroCliente = numCliente;
                            existente.Pais = pais;
                            existente.PaisOrigem = paisOrigem;
                            existente.Estado = estado;
                            existente.DataModificacao = DateTime.Now;
                            _context.Update(existente);
                            atualizados++;
                        }
                        else
                        {
                            var novo = new Cliente
                            {
                                Nome = nome,
                                Tipo = TipoCliente.Empresa,
                                NumeroCliente = numCliente,
                                Pais = pais,
                                PaisOrigem = paisOrigem,
                                Estado = estado,
                                DataCriacao = DateTime.Now
                            };
                            _context.Add(novo);
                            importados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        erros++;
                        mensagensErro.Add($"Linha {linhaAtual}: {ex.Message}");
                    }
                }

                await _context.SaveChangesAsync();

                var msg = $"Importação concluída: {importados} novos, {atualizados} atualizados.";
                if (erros > 0) msg += $" {erros} erros ignorados.";
                TempData["Success"] = msg;
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao processar o ficheiro: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BuscarPorNome(string termo)
        {
            if (string.IsNullOrEmpty(termo) || termo.Length < 2)
                return Json(new List<object>());

            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.Nome.Contains(termo))
                    .Take(10)
                    .Select(c => new { id = c.IDCliente, nome = c.Nome })
                    .ToListAsync();

                return Json(clientes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar clientes: {ex.Message}");
                return Json(new { erro = "Erro ao processar solicitação" });
            }
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IDCliente == id);
        }
    }
}
