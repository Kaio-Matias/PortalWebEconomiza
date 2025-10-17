namespace PortalWebEconomiza.Models
{
    public class PesquisaProdutoResponse
    {
        public int TotalRegistros { get; set; }
        public List<Registro> Conteudo { get; set; } = [];
    }

    public class Registro
    {
        public Produto? Produto { get; set; }
        public Estabelecimento? Estabelecimento { get; set; }
    }

    public class Produto
    {
        public string? DescricaoSefaz { get; set; }
        public string? Gtin { get; set; }
        public string? Ncm { get; set; }
        public string? Gpc { get; set; }
        public string? UnidadeMedida { get; set; }
        public Venda? Venda { get; set; }
    }

    public class Venda
    {
        public DateTime DataVenda { get; set; }
        public decimal ValorDeclarado { get; set; }
        public decimal ValorVenda { get; set; }
    }

    public class Estabelecimento
    {
        public string? Cnpj { get; set; }
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Telefone { get; set; }
        public Endereco? Endereco { get; set; }
    }

    public class Endereco
    {
        public string? NomeLogradouro { get; set; }
        public string? NumeroImovel { get; set; }
        public string? Bairro { get; set; }
        public string? Cep { get; set; }
        public int CodigoIBGE { get; set; }
        public string? Municipio { get; set; }
    }
}
