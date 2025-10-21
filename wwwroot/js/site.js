// O ficheiro site.js já deve ter esta linha de abertura
$(document).ready(function () {

    // --- LÓGICA DO MODAL DE CARREGAMENTO (Página Index) ---

    var consultaForm = $('#consultaForm');

    // Verifica se o formulário #consultaForm existe nesta página
    if (consultaForm.length) {

        consultaForm.on('submit', function () {
            // Verifica se o formulário é válido (requer jQuery Validation)
            if (consultaForm.valid()) {
                // Se for válido, mostra o modal
                $('#loadingModal').show();
            }
        });
    }


    // --- LÓGICA DOS FILTROS DA TABELA DE RESULTADOS (Página Index) ---

    // Função para aplicar os filtros na tabela
    function aplicarFiltrosIndex() {
        // Obter o valor de cada filtro (em minúsculas para comparação)
        var filtroProduto = $('#filterProduto').val().toLowerCase();
        var filtroGtin = $('#filterGtin').val().toLowerCase();
        var filtroMunicipio = $('#filterMunicipio').val().toLowerCase();

        // Para data, pegamos o valor no formato AAAA-MM-DD
        var filtroData = $('#filterDataVenda').val();

        // Se a data for selecionada, convertemos para DD/MM/AAAA para comparar
        var filtroDataFormatada = "";
        if (filtroData) {
            var partes = filtroData.split('-');
            filtroDataFormatada = partes[2] + '/' + partes[1] + '/' + partes[0];
        }

        // Iterar sobre cada linha <tr> no corpo <tbody> da tabela
        $('#resultsTable tbody tr').each(function () {
            var row = $(this);

            // Obter o texto de cada coluna desta linha
            var produto = row.find('td:nth-child(1)').text().toLowerCase();
            var gtin = row.find('td:nth-child(2)').text().toLowerCase();
            var dataVenda = row.find('td:nth-child(4)').text(); // Data está na 4ª coluna
            var municipio = row.find('td:nth-child(6)').text().toLowerCase(); // Município está na 6ª coluna

            // Verificar se a linha corresponde a TODOS os filtros preenchidos
            var mostrar = true;
            if (filtroProduto && !produto.includes(filtroProduto)) mostrar = false;
            if (filtroGtin && !gtin.includes(filtroGtin)) mostrar = false;
            if (filtroMunicipio && !municipio.includes(filtroMunicipio)) mostrar = false;
            if (filtroDataFormatada && !dataVenda.includes(filtroDataFormatada)) mostrar = false;

            // Mostrar ou esconder a linha com base no resultado
            row.toggle(mostrar);
        });
    }

    // Verifica se a tabela #resultsTable existe nesta página
    if ($('#resultsTable').length) {
        // Adicionar o "escutador" de eventos aos inputs de filtro
        // 'keyup' para texto (filtra enquanto digita) e 'change' para data
        $('#filterProduto, #filterGtin, #filterMunicipio').on('keyup', aplicarFiltrosIndex);
        $('#filterDataVenda').on('change', aplicarFiltrosIndex);
    }
    var selectMunicipios = $('#Filtros_CodigosIBGE');

    if (selectMunicipios.length) {

        // Botão "Selecionar Todos"
        $('#btnSelecionarTodosMunicipios').on('click', function () {
            // Seleciona todas as options
            selectMunicipios.find('option').prop('selected', true);
        });

        // Botão "Limpar Seleção"
        $('#btnLimparSelecaoMunicipios').on('click', function () {
            // Remove a seleção de todas as options
            selectMunicipios.find('option').prop('selected', false);
        });
    }
}); // Esta linha de fecho já deve existir no seu site.js