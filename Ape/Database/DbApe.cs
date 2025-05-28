using Microsoft.EntityFrameworkCore;
using Ape.Entities;

namespace Ape.Database
{
    public class DbApe : DbContext
    {
        public DbApe (DbContextOptions<DbApe> options) : base(options)
        {

        }

        public DbSet<Perfil> Perfil { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FKs da Tabela UsuarioPerfil
            modelBuilder.Entity<UsuarioPerfil>()
                        .HasOne(u => u.Usuario)
                        .WithMany()
                        .HasForeignKey(u => u.IdUsuario);

            modelBuilder.Entity<UsuarioPerfil>()
                        .HasOne(u => u.Perfil)
                        .WithMany()
                        .HasForeignKey(u => u.IdPerfil);
        }
    }
}