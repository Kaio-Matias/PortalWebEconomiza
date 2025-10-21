using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // Adicionar este
using PortalWebEconomiza.Models;
using PortalWebEconomiza.Services;
using PortalWebEconomiza.Utils; // Adicionar este
using PortalWebEconomiza.ViewModels;

namespace PortalWebEconomiza.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly SefazApiClient _sefazApiClient;
        private readonly ConsultaRepository _consultaRepository;

        [BindProperty(SupportsGet = true)]
        public PesquisaViewModel Filtros { get; set; } = new();

        public PesquisaProdutoResponse? ResultadoPesquisa { get; set; }

        // Adicionar esta propriedade para o dropdown
        public SelectList MunicipiosOptions { get; set; }

        public IndexModel(ILogger<IndexModel> logger, SefazApiClient sefazApiClient, ConsultaRepository consultaRepository)
        {
            _logger = logger;
            _sefazApiClient = sefazApiClient;
            _consultaRepository = consultaRepository;

            // Carrega a lista de municípios para o dropdown
            MunicipiosOptions = new SelectList(MunicipioData.GetMunicipios(), "CodigoIBGE", "Nome");
        }

        public void OnGet()
        {
            // A página apenas carrega
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool hasProductFilter = !string.IsNullOrEmpty(Filtros.Gtin) ||
                                    !string.IsNullOrEmpty(Filtros.Gpc) ||
                                    !string.IsNullOrEmpty(Filtros.Ncm);

            // Ajusta a validação para a nova lista de municípios
            bool hasEstablishmentFilter = !string.IsNullOrEmpty(Filtros.Cnpj) ||
                                          (Filtros.CodigosIBGE != null && Filtros.CodigosIBGE.Any());

            if (!hasProductFilter && !hasEstablishmentFilter)
            {
                ModelState.AddModelError(string.Empty, "É necessário preencher ao menos um filtro de produto (GTIN, GPC, NCM) ou de estabelecimento (CNPJ, Município) para realizar a consulta.");
                return Page();
            }

            try
            {
                ResultadoPesquisa = await _sefazApiClient.ObterProdutosAsync(Filtros);

                if (ResultadoPesquisa != null && ResultadoPesquisa.Conteudo.Any())
                {
                    await _consultaRepository.SalvarConsultas(ResultadoPesquisa.Conteudo);
                }
            }
            catch (Exception ex)
            {
                // O erro de timeout (SocketException) será capturado aqui
                _logger.LogError(ex, "Erro ao consultar a API da Sefaz");
                ModelState.AddModelError(string.Empty, $"Ocorreu um erro ao realizar a consulta: {ex.Message}");
            }

            return Page();
        }
    }
}