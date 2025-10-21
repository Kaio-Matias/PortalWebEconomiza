using PortalWebEconomiza.Models;

namespace PortalWebEconomiza.Services
{
    public interface IProdutoApiService
    {
        /// <summary>
        /// Pesquisa produtos na API Sefaz de forma assíncrona.
        /// </summary>
        /// <param name="gpc">Filtro GPC (Global Product Classification)</param>
        /// <param name="gtin">Filtro GTIN (Global Trade Item Number)</param>
        /// <param name="ncm">Filtro NCM (Nomenclatura Comum do Mercosul)</param>
        /// <param name="descricao">Filtro por descrição do produto</param>
        /// <returns>Um objeto PesquisaProdutoResponse ou null se ocorrer um erro.</returns>
        Task<PesquisaProdutoResponse?> PesquisarProdutosAsync(string? gpc, string? gtin, string? ncm, string? descricao);
    }
}