using System.ComponentModel.DataAnnotations;

namespace PortalWebEconomiza.ViewModels
{
    public class PesquisaViewModel
    {
        // Filtros de Produto
        public string? Gtin { get; set; }
        public string? Gpc { get; set; }
        public string? Ncm { get; set; }

        [Display(Name = "Descrição do Produto")]
        public string? Descricao { get; set; } // NOVO CAMPO ADICIONADO

        // Filtros de Estabelecimento
        public string? Cnpj { get; set; }

        [Display(Name = "Município(s)")]
        public List<string>? CodigosIBGE { get; set; }

        // Filtro de Período
        [Display(Name = "Período (dias)")]
        [Range(1, 90, ErrorMessage = "O período deve ser entre 1 e 90 dias.")]
        public int Dias { get; set; } = 7;
    }
}