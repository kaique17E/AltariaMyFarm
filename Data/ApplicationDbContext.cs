using Microsoft.EntityFrameworkCore;
using DsiVendas.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users {get; set;}
    public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<ItemVenda> ItemsVendas { get; set; }
        public DbSet<ItemCompra> ItemsCompra { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<FormaPagamento> FormaPagamentos { get; set; }
        public DbSet<NFEntrada> NFEntradas { get; set; }
        public DbSet<NFSaida> NFSaidas { get; set; }
        public DbSet<ItemNFE> ItemsNFE { get; set; }
        public DbSet<ItemNFS> ItemsNFS { get; set; }
        public DbSet<AreaPlantio> AreaPlantios { get; set; }
        public DbSet<Plantio> Plantios { get; set; }
        public DbSet<ItemRecurso> ItemsRecursos { get; set; }
        public DbSet<Fazenda> Fazendas { get; set; }
        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Manutencao> Manutencoes { get; set; }
        public DbSet<Parcela> Parcelas {get; set;}
        public object Usuarios { get; internal set; }
}
