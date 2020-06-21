using Microsoft.EntityFrameworkCore;

namespace Kolokwium.Models
{
    public class DatabaseContext : DbContext
    {
        protected DatabaseContext(){}

        public DatabaseContext(DbContextOptions options) : base(options){}

        public DbSet<Pracownik> Pracownicy { get; set; }
        public DbSet<Klient> Klienci { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<WyrobCukierniczy> WyrobyCukiernicze { get; set; }
        public DbSet<ZamowienieWyrobCukierniczy> ZamowieniaWyrobyCukiernicze { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ZamowienieWyrobCukierniczy>()
                .HasOne(e => e.Zamowienie)
                .WithMany(e => e.ZamowieniaWyrobyCukiernicze)
                .HasForeignKey(e => e.IdZamowienie);

            modelBuilder.Entity<ZamowienieWyrobCukierniczy>()
                .HasOne(e => e.WyrobCukierniczy)
                .WithMany(e => e.ZamowieniaWyrobCukiernicze)
                .HasForeignKey(e => e.IdWyrobCukierniczy);
            
            modelBuilder.Entity<ZamowienieWyrobCukierniczy>()
                .HasKey(e => new {e.IdZamowienie, e.IdWyrobCukierniczy});

            base.OnModelCreating(modelBuilder);
        }
    }
}