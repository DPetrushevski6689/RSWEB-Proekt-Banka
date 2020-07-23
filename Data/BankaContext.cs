using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Banka.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Banka.Data
{
    public class BankaContext : IdentityDbContext<AppUser>
    {
        public BankaContext (DbContextOptions<BankaContext> options)
            : base(options)
        {
        }

        public DbSet<Banka.Models.Korisnik> Korisnik { get; set; }

        public DbSet<Banka.Models.Firma> Firma { get; set; }

        public DbSet<Banka.Models.Employee> Employee { get; set; }

        public DbSet<Banka.Models.KorisnickaSmetka> KorisnickaSmetka { get; set; }

        public DbSet<Banka.Models.KompaniskaSmetka> KompaniskaSmetka { get; set; }

        public DbSet<Banka.Models.Karticka> Karticka { get; set; }

        public DbSet<Banka.Models.EmployeeFirms> EmployeeFirms { get; set; }

        public DbSet<Banka.Models.FirmiSopstvenici> FirmiSopstvenici { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FirmiSopstvenici>()
                .HasOne<Korisnik>(p => p.Sopstvenik)
                .WithMany(p => p.Firmi)
                .HasForeignKey(p => p.sopstvenikId);

            builder.Entity<FirmiSopstvenici>()
                .HasOne<Firma>(p => p.Firma)
                .WithMany(p => p.Sopstvenici)
                .HasForeignKey(p => p.firmaId);

            builder.Entity<EmployeeFirms>()
                .HasOne<Employee>(e => e.vrabotenKoordinator)
                .WithMany(e => e.KompaniskiSmetki)
                .HasForeignKey(e => e.employeeId);

            builder.Entity<EmployeeFirms>()
                .HasOne<KompaniskaSmetka>(e => e.kompaniskaSmetka)
                .WithMany(e => e.vrabotenKoordinator)
                .HasForeignKey(e => e.kompaniskaSmetkaId);

            builder.Entity<KorisnickaSmetka>()
                .HasOne<Korisnik>(k => k.Korisnik)
                .WithMany(k => k.KorisnickiSmetki)
                .HasForeignKey(k => k.korisnikId);

            builder.Entity<Karticka>()
                .HasOne<KorisnickaSmetka>(k => k.korisnickSmetka)
                .WithMany(k => k.Karticki)
                .HasForeignKey(k => k.korisnickaSmetkaId);

            builder.Entity<KompaniskaSmetka>()
                .HasOne<Firma>(k => k.Firma)
                .WithMany(k => k.kompaniskiSmetki)
                .HasForeignKey(k => k.firmaId);


        }
    }
}
