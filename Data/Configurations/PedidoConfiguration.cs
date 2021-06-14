using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CursoEFCore.Data.Configurations
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.IniciadoEm).HasDefaultValueSql("GetDate()").ValueGeneratedOnAdd();
            builder.Property(c => c.StatusPedido).HasConversion<string>();
            builder.Property(c => c.TipoFrete).HasConversion<int>();
            builder.Property(c => c.Observacao).HasColumnType("Varchar(512)");

            builder.HasMany(p => p.Itens)
                .WithOne(p => p.Pedido)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}