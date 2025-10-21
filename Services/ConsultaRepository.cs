using Microsoft.EntityFrameworkCore;
using PortalWebEconomiza.Data;
using PortalWebEconomiza.Models;

namespace PortalWebEconomiza.Services
{
    public class ConsultaRepository
    {
        private readonly EconomizaDbContext _context;

        public ConsultaRepository(EconomizaDbContext context)
        {
            _context = context;
        }

        public async Task SalvarConsultas(List<Registro> registros)
        {
            var dataConsultaAtual = DateTime.UtcNow; // Consistência na data/hora
            var produtosConsultados = new List<ProdutoConsultado>();

            foreach (var registro in registros)
            {
                // Mapeamento cuidadoso para evitar erros de referência nula
                var produto = new ProdutoConsultado
                {
                    DescricaoSefaz = registro.Produto?.DescricaoSefaz,
                    Gtin = registro.Produto?.Gtin,
                    Ncm = registro.Produto?.Ncm,
                    Gpc = registro.Produto?.Gpc,
                    UnidadeMedida = registro.Produto?.UnidadeMedida,
                    DataVenda = registro.Produto?.Venda?.DataVenda ?? DateTime.MinValue,
                    ValorDeclarado = registro.Produto?.Venda?.ValorDeclarado ?? 0,
                    ValorVenda = registro.Produto?.Venda?.ValorVenda ?? 0,
                    Cnpj = registro.Estabelecimento?.Cnpj,
                    RazaoSocial = registro.Estabelecimento?.RazaoSocial,
                    NomeFantasia = registro.Estabelecimento?.NomeFantasia,
                    Telefone = registro.Estabelecimento?.Telefone,
                    NomeLogradouro = registro.Estabelecimento?.Endereco?.NomeLogradouro,
                    NumeroImovel = registro.Estabelecimento?.Endereco?.NumeroImovel,
                    Bairro = registro.Estabelecimento?.Endereco?.Bairro,
                    Cep = registro.Estabelecimento?.Endereco?.Cep,
                    CodigoIBGE = registro.Estabelecimento?.Endereco?.CodigoIBGE ?? 0,
                    Municipio = registro.Estabelecimento?.Endereco?.Municipio,
                    DataConsulta = dataConsultaAtual,
                    // ProdutoValedourado = false // Valor padrão
                };
                produtosConsultados.Add(produto);
            }

            // Adiciona todos os novos registros de uma vez
            await _context.ProdutosConsultados.AddRangeAsync(produtosConsultados);

            // Salva todas as mudanças no banco de dados em uma única transação
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProdutoConsultado>> GetConsultasPorData(DateTime data)
        {
            // Filtra para pegar todos os registros do dia informado
            return await _context.ProdutosConsultados
                .Where(p => p.DataConsulta.Date == data.Date)
                .OrderByDescending(p => p.DataConsulta)
                .AsNoTracking() // Melhora a performance para consultas "read-only"
                .ToListAsync();
        }
    }
}