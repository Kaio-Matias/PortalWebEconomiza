using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using PortalWebEconomiza.Config;
using PortalWebEconomiza.Models;
using PortalWebEconomiza.ViewModels;
using Microsoft.Extensions.Options;

namespace PortalWebEconomiza.Services
{
    public class SefazApiClient
    {
        private readonly HttpClient _client;
        private readonly ApiConfig _apiConfig;
        private readonly ILogger<SefazApiClient> _logger; // 1. ADICIONADO LOGGER

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        // 1. ADICIONADO ILogger<SefazApiClient> logger
        public SefazApiClient(HttpClient client, IOptions<ApiConfig> apiConfig, ILogger<SefazApiClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _apiConfig = apiConfig.Value ?? throw new ArgumentNullException(nameof(apiConfig));
            _logger = logger; // 1. ADICIONADO

            if (!_client.DefaultRequestHeaders.Contains("AppToken"))
            {
                _client.DefaultRequestHeaders.Add("AppToken", _apiConfig.AppToken);
            }
        }

        public async Task<PesquisaProdutoResponse> ObterProdutosAsync(PesquisaViewModel filtros)
        {
            var municipiosParaConsultar = new List<int?>();

            // 2. CORREÇÃO DO BUG DE MUNICÍPIO
            if (filtros.CodigosIBGE != null && filtros.CodigosIBGE.Any())
            {
                var validCodes = filtros.CodigosIBGE
                    .Where(c => !string.IsNullOrEmpty(c)) // Filtra a string vazia
                    .Select(c => (int?)int.Parse(c));

                municipiosParaConsultar.AddRange(validCodes);
            }

            // Se, depois de filtrar, a lista ficou vazia, age como "todos"
            if (!municipiosParaConsultar.Any())
            {
                municipiosParaConsultar.Add(null);
            }
            // FIM DA CORREÇÃO 2

            var respostaAgregada = new PesquisaProdutoResponse
            {
                Conteudo = new List<Registro>(),
                TotalRegistros = 0
            };

            foreach (var codigoIbge in municipiosParaConsultar)
            {
                var payload = new
                {
                    produto = new
                    {
                        gtin = string.IsNullOrEmpty(filtros.Gtin) ? null : filtros.Gtin,
                        ncm = string.IsNullOrEmpty(filtros.Ncm) ? null : filtros.Ncm,
                        gpc = string.IsNullOrEmpty(filtros.Gpc) ? null : filtros.Gpc,
                        descricao = string.IsNullOrEmpty(filtros.Descricao) ? null : filtros.Descricao
                    },
                    estabelecimento = new
                    {
                        cnpj = string.IsNullOrEmpty(filtros.Cnpj) ? null : filtros.Cnpj,
                        municipio = codigoIbge.HasValue ? new { codigoIBGE = codigoIbge.Value } : null
                    },
                    dias = filtros.Dias,
                    pagina = 1,
                    registrosPorPagina = 5000
                };

                var payloadJson = JsonSerializer.Serialize(payload, _jsonSerializerOptions);
                var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

                // Loga o JSON exato que estamos a enviar
                _logger.LogInformation("Enviando payload para Sefaz: {Payload}", payloadJson);

                var response = await _client.PostAsync("produto/pesquisa", content);

                // 3. CAPTURA DE ERRO MELHORADA
                if (!response.IsSuccessStatusCode)
                {
                    // Tenta ler a resposta de erro da API
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Sefaz retornou um erro {StatusCode}. Resposta: {ErrorBody}", response.StatusCode, errorBody);

                    // Lança uma exceção mais informativa que será mostrada no ecrã
                    throw new HttpRequestException($"A API Sefaz retornou um erro ({response.StatusCode}). Detalhes: {errorBody}");
                }
                // FIM DA CAPTURA 3

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var pesquisaResponse = JsonSerializer.Deserialize<PesquisaProdutoResponse>(jsonResponse, _jsonSerializerOptions);

                if (pesquisaResponse != null)
                {
                    respostaAgregada.Conteudo.AddRange(pesquisaResponse.Conteudo ?? new List<Registro>());
                    respostaAgregada.TotalRegistros += pesquisaResponse.TotalRegistros;
                }
            }

            return respostaAgregada;
        }
    }
}