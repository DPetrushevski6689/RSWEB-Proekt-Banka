﻿// <auto-generated />
using System;
using Banka.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Banka.Migrations
{
    [DbContext(typeof(BankaContext))]
    [Migration("20200721133208_Initial2")]
    partial class Initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Banka.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Position")
                        .IsRequired();

                    b.Property<DateTime>("birthDate");

                    b.Property<string>("firstName")
                        .IsRequired();

                    b.Property<string>("lastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Banka.Models.EmployeeFirms", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("employeeId");

                    b.Property<int>("kompaniskaSmetkaId");

                    b.Property<string>("tip")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("employeeId");

                    b.HasIndex("kompaniskaSmetkaId");

                    b.ToTable("EmployeeFirms");
                });

            modelBuilder.Entity("Banka.Models.Firma", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("dataOsnovanje");

                    b.Property<string>("firmName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Firma");
                });

            modelBuilder.Entity("Banka.Models.FirmiSopstvenici", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("firmaId");

                    b.Property<int>("sopstvenikId");

                    b.HasKey("Id");

                    b.HasIndex("firmaId");

                    b.HasIndex("sopstvenikId");

                    b.ToTable("FirmiSopstvenici");
                });

            modelBuilder.Entity("Banka.Models.Karticka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("brojNaKarticka")
                        .IsRequired();

                    b.Property<int>("korisnickaSmetkaId");

                    b.Property<string>("paricnaSostojba")
                        .IsRequired();

                    b.Property<string>("tipNaKarticka")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("korisnickaSmetkaId");

                    b.ToTable("Karticka");
                });

            modelBuilder.Entity("Banka.Models.KompaniskaSmetka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("bankarskiBroj")
                        .IsRequired();

                    b.Property<DateTime>("dataIzdavanje");

                    b.Property<int>("firmaId");

                    b.Property<string>("paricnaSostojba")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("firmaId");

                    b.ToTable("KompaniskaSmetka");
                });

            modelBuilder.Entity("Banka.Models.KorisnickaSmetka", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("bankarskiBroj")
                        .IsRequired();

                    b.Property<DateTime>("dataIzdavanje");

                    b.Property<int?>("korisnikId");

                    b.Property<string>("paricnaSostojba")
                        .IsRequired();

                    b.Property<string>("tip")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("korisnikId");

                    b.ToTable("KorisnickaSmetka");
                });

            modelBuilder.Entity("Banka.Models.Korisnik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("birthDate");

                    b.Property<string>("firstName")
                        .IsRequired();

                    b.Property<string>("lastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Korisnik");
                });

            modelBuilder.Entity("Banka.Models.EmployeeFirms", b =>
                {
                    b.HasOne("Banka.Models.Employee", "vrabotenKoordinator")
                        .WithMany("KompaniskiSmetki")
                        .HasForeignKey("employeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Banka.Models.KompaniskaSmetka", "kompaniskaSmetka")
                        .WithMany("vrabotenKoordinator")
                        .HasForeignKey("kompaniskaSmetkaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Banka.Models.FirmiSopstvenici", b =>
                {
                    b.HasOne("Banka.Models.Firma", "Firma")
                        .WithMany("Sopstvenici")
                        .HasForeignKey("firmaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Banka.Models.Korisnik", "Sopstvenik")
                        .WithMany("Firmi")
                        .HasForeignKey("sopstvenikId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Banka.Models.Karticka", b =>
                {
                    b.HasOne("Banka.Models.KorisnickaSmetka", "korisnickSmetka")
                        .WithMany("Karticki")
                        .HasForeignKey("korisnickaSmetkaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Banka.Models.KompaniskaSmetka", b =>
                {
                    b.HasOne("Banka.Models.Firma", "Firma")
                        .WithMany("kompaniskiSmetki")
                        .HasForeignKey("firmaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Banka.Models.KorisnickaSmetka", b =>
                {
                    b.HasOne("Banka.Models.Korisnik", "Korisnik")
                        .WithMany("KorisnickiSmetki")
                        .HasForeignKey("korisnikId");
                });
#pragma warning restore 612, 618
        }
    }
}
