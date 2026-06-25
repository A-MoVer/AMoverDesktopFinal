namespace A_Mover_Desktop_Final.ViewModels
{
    public class RentabilidadeMotaViewModel
    {
        public int IDVendaMota { get; set; }
        public string Modelo { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string NumeroSerie { get; set; } = string.Empty;
        public double Quilometragem { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal CustoAquisicao { get; set; }
        public decimal DespesasManutencao { get; set; }
        public decimal Lucro => PrecoVenda - (CustoAquisicao + DespesasManutencao);
        public decimal Margem => PrecoVenda > 0 ? Math.Round(Lucro / PrecoVenda * 100, 2) : 0;
        public decimal CustoPorKm => Quilometragem > 0 ? Math.Round((CustoAquisicao + DespesasManutencao) / (decimal)Quilometragem, 2) : 0;
        public DateTime DataVenda { get; set; }
    }
}
