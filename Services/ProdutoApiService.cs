using PortalWebEconomiza.Models;
using Microsoft.AspNetCore.WebUtilities; // Necessário para QueryHelpers
using System.Text.Json;

namespace PortalWebEconomiza.Services
{
    public class ProdutoApiService : IProdutoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProdutoApiService> _logger;

        // O HttpClient é injetado automaticamente pelo .NET (configurado no Program.cs)
        public ProdutoApiService(HttpClient httpClient, ILogger<ProdutoApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PesquisaProdutoResponse?> PesquisarProdutosAsync(string? gpc, string? gtin, string? ncm, string? descricao)
        {
            // --- INÍCIO DA CORREÇÃO: Construção Dinâmica da Query String ---

            // 1. Criar um dicionário para os parâmetros
            var queryParams = new Dictionary<string, string?>();

            // 2. Adicionar ao dicionário APENAS os filtros que não são nulos ou vazios
            if (!string.IsNullOrEmpty(gpc))
            {
                queryParams.Add("gpc", gpc);
            }
            if (!string.IsNullOrEmpty(gtin))
            {
                queryParams.Add("gtin", gtin);
            }
            if (!string.IsNullOrEmpty(ncm))
            {
                queryParams.Add("ncm", ncm);
            }
            if (!string.IsNullOrEmpty(descricao))
            {
                queryParams.Add("descricao", descricao);
            }

            // 3. Se nenhum filtro foi preenchido, não faz sentido fazer a chamada.
            //    (Pode alterar esta lógica se uma chamada vazia for válida)
            if (queryParams.Count == 0)
            {
                _logger.LogInformation("Nenhum filtro fornecido. A chamada à API não será realizada.");
                return null;
            }

            // 4. Usar o QueryHelpers para construir a URL final de forma segura.
            //    Estou a assumir que o endpoint é "produto". 
            //    Se for outro, altere o "produto" abaixo.
            string requestUri = QueryHelpers.AddQueryString("produto", queryParams);

            _logger.LogInformation("Requisitando API Sefaz: {RequestUri}", requestUri);

            // --- FIM DA CORREÇÃO ---

            try
            {
                // O _httpClient já tem a BaseUrl e o AppToken configurados
                var response = await _httpClient.GetAsync(requestUri);

                // Lança uma exceção se a resposta não for 2xx (sucesso)
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                // Desserializa a resposta
                return JsonSerializer.Deserialize<PesquisaProdutoResponse>(jsonString,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Erro ao chamar a API Sefaz. URI: {RequestUri}", requestUri);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao processar resposta da API.");
                return null;
            }
        }
    }
}