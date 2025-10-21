using Microsoft.EntityFrameworkCore;
using PortalWebEconomiza.Models;

namespace PortalWebEconomiza.Data
{
    public class EconomizaDbContext : DbContext
    {
        public EconomizaDbContext(DbContextOptions<EconomizaDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProdutoConsultado> ProdutosConsultados { get; set; }

        /// <summary>
        /// Configura o modelo de dados para o Entity Framework,
        /// aplicando otimizações e restrições para o banco de dados.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura a entidade/tabela 'ProdutoConsultado'
            modelBuilder.Entity<ProdutoConsultado>(entity =>
            {
                // --- AJUSTE 1: PRECISÃO DOS DECIMAIS ---
                // Garante que os valores monetários sejam armazenados com precisão,
                // evitando perda de dados.
                entity.Property(p => p.ValorDeclarado).HasColumnType("decimal(18, 2)");
                entity.Property(p => p.ValorVenda).HasColumnType("decimal(18, 2)");

                // --- AJUSTE 2: TAMANHO MÁXIMO PARA CAMPOS DE TEXTO ---
                // Otimiza o armazenamento e a performance definindo limites
                // para campos com tamanho conhecido.
                entity.Property(p => p.Gtin).HasMaxLength(14);
                entity.Property(p => p.Cnpj).HasMaxLength(18); // 14 para números + 4 para máscara (xx.xxx.xxx/xxxx-xx)
                entity.Property(p => p.Cep).HasMaxLength(9); // 8 para números + 1 para hífen

                // --- AJUSTE 3: ÍNDICES PARA MELHORAR PERFORMANCE ---
                // Acelera as consultas que filtram por estas colunas. Essencial
                // para a página de Histórico e futuras funcionalidades de busca.
                entity.HasIndex(p => p.DataConsulta).HasName("IX_ProdutosConsultados_DataConsulta");
                entity.HasIndex(p => p.Gtin).HasName("IX_ProdutosConsultados_Gtin");
                entity.HasIndex(p => p.Cnpj).HasName("IX_ProdutosConsultados_Cnpj");
            });
        }
    }
}

