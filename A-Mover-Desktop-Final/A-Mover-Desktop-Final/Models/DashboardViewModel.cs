namespace A_Mover_Desktop_Final.Models
{
    public class DashboardViewModel
    {
        // Estatísticas de Produção
        public int TotalMotas { get; set; }
        public int MotasEmProducao { get; set; }
        public int MotasAtivas { get; set; }
        
        // Estatísticas de Encomendas
        public int TotalEncomendas { get; set; }
        public int EncomendasPendentes { get; set; }
        public int EncomendasEmProducao { get; set; }
        public int EncomendasConcluidas { get; set; }
        
        // Estatísticas de Ordens de Produção
        public int TotalOrdens { get; set; }
        public int OrdensPendentes { get; set; }
        public int OrdensEmProducao { get; set; }
        public int OrdensConcluidas { get; set; }
        
        // Estatísticas de Serviços
        public int TotalServicos { get; set; }
        public int ServicosAgendados { get; set; }
        public int ServicosEmCurso { get; set; }
        public int ServicosConcluidos { get; set; }
        
        // Dados para gráficos
        public List<string> MesesProducao { get; set; } = new List<string>();
        public List<int> ValoresProducao { get; set; } = new List<int>();
        
        public List<string> TopModelosNomes { get; set; } = new List<string>();
        public List<int> TopModelosValores { get; set; } = new List<int>();
        
        public List<string> TiposServico { get; set; } = new List<string>();
        public List<int> ValoresServico { get; set; } = new List<int>();
        
        // Atividades recentes (opcional - poderia ser uma lista de atividades)
        public List<dynamic> AtividadesRecentes { get; set; } = new List<dynamic>();
    }
}