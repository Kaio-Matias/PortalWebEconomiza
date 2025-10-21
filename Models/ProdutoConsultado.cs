// Models/ProdutoConsultado.cs
using System.ComponentModel.DataAnnotations;

namespace PortalWebEconomiza.Models
{
    public class ProdutoConsultado
    {
        public int Id { get; set; }
        public string? DescricaoSefaz { get; set; }
        public string? Gtin { get; set; }
        public string? Ncm { get; set; }
        public string? Gpc { get; set; }
        public string? UnidadeMedida { get; set; }
        public DateTime DataVenda { get; set; }
        public decimal ValorDeclarado { get; set; }
        public decimal ValorVenda { get; set; }
        public string? Cnpj { get; set; }
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Telefone { get; set; }
        public string? NomeLogradouro { get; set; }
        public string? NumeroImovel { get; set; }
        public string? Bairro { get; set; }
        public string? Cep { get; set; }
        public int CodigoIBGE { get; set; }
        public string? Municipio { get; set; }

        [Display(Name = "Data da Consulta")]
        public DateTime DataConsulta { get; set; }
    }
}