using MEC.Models;
using Microsoft.EntityFrameworkCore;

namespace MEC.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        
        public DbSet<Produto> Produtos { get; set; }

        public DbSet<ChapaMDF> ChapasMDF { get; set; }
        public DbSet<MaterialLinear> MateriaisLineares { get; set; }
        public DbSet<MaterialUnidade> MateriaisUnidade { get; set; }
        public DbSet<CorteMaterial> CortesMateriais { get; set; }
        public DbSet<MovimentoEstoque> MovimentosEstoque { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<ItemProjeto> ItensProjeto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.CPF)
                .IsUnique();

            // Fornecedor - CNPJ único
            modelBuilder.Entity<Fornecedor>()
                .HasIndex(f => f.CNPJ)
                .IsUnique();

            // Configuração de herança (TPH - Table Per Hierarchy)
            modelBuilder.Entity<Produto>()
                .HasDiscriminator<TipoProduto>("TipoProduto")
                .HasValue<ChapaMDF>(TipoProduto.ChapaMDF)
                .HasValue<MaterialLinear>(TipoProduto.MaterialLinear)
                .HasValue<MaterialUnidade>(TipoProduto.MaterialUnidade);

            // Relacionamento Produto-Fornecedor
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.SetNull); // Se fornecedor for deletado, seta null no produto

            // Configurações para cortes
            modelBuilder.Entity<CorteMaterial>()
                .HasOne(c => c.MaterialLinear)
                .WithMany(m => m.Cortes)
                .HasForeignKey(c => c.MaterialLinearId);

            // Relacionamento MovimentoEstoque-Produto
            modelBuilder.Entity<MovimentoEstoque>()
                .HasOne(m => m.Produto)
                .WithMany(p => p.Movimentos)
                .HasForeignKey(m => m.ProdutoId);

            modelBuilder.Entity<ItemProjeto>()
                .HasOne(i => i.Projeto)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.ProjetoId);

            modelBuilder.Entity<ItemProjeto>()
                .HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId);

        }
    }
}