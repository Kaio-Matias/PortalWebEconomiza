// Pages/Historico.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalWebEconomiza.Models;
using PortalWebEconomiza.Services;
using System.ComponentModel.DataAnnotations;

namespace PortalWebEconomiza.Pages
{
    public class HistoricoModel : PageModel
    {
        private readonly ConsultaRepository _repository;

        public HistoricoModel(ConsultaRepository repository)
        {
            _repository = repository;
        }

        [BindProperty(SupportsGet = true)]
        [DataType(DataType.Date)]
        public DateTime DataFiltro { get; set; } = DateTime.Today;

        public List<ProdutoConsultado> Consultas { get; set; } = new();

        public async Task OnGetAsync()
        {
            Consultas = await _repository.GetConsultasPorData(DataFiltro);
        }
    }
}