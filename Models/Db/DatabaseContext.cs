using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace TestTest.Models.Db;

public partial class DatabaseContext : DbContext
{


        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Grupy> Grupy { get; set; } = null!;
        public virtual DbSet<KategoriaPytania> KategoriaPytania { get; set; } = null!;
        public virtual DbSet<ListaPytan> ListaPytan { get; set; } = null!;
        public virtual DbSet<Odpowiedz> Odpowiedz { get; set; } = null!;
        public virtual DbSet<Pytanie> Pytanie { get; set; } = null!;
        public virtual DbSet<Rozwiazanie> Rozwiazanie { get; set; } = null!;
        public virtual DbSet<RozwiazanieDoPytan> RozwiazanieDoPytan { get; set; } = null!;
        public virtual DbSet<Status> Status { get; set; } = null!;
        public virtual DbSet<Test> Test { get; set; } = null!;
        public virtual DbSet<TypPytania> TypPytania { get; set; } = null!;
        public virtual DbSet<Uczestnicy> Uczestnicy { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DB_Login");
            }
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Grupy>(entity =>
        {
            entity.HasKey(e => e.IdGrupy)
                .HasName("PK__Grupy__EC597A91075872A5");

            entity.ToTable("Grupy");

            entity.Property(e => e.IdGrupy)
                .ValueGeneratedNever()
                .HasColumnName("idGrupy");

            entity.Property(e => e.IdNauczyciela).HasColumnName("idNauczyciela");

            entity.Property(e => e.Nazwa)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");
        });
        modelBuilder.Entity<Grupy>().ToTable("Grupy");

        modelBuilder.Entity<KategoriaPytania>(entity =>
        {
            entity.HasKey(e => e.IdKategoriaPytania)
                .HasName("PK__Kategori__D603FA56976D8D88");

            entity.Property(e => e.IdKategoriaPytania)
                .ValueGeneratedNever()
                .HasColumnName("idKategoriaPytania");

            entity.Property(e => e.Nazwa)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");

            entity.Property(e => e.Opis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("opis");
        });

        modelBuilder.Entity<KategoriaPytania>().ToTable("KategoriaPytania");

        modelBuilder.Entity<ListaPytan>(entity =>
        {
            entity.HasKey(e => e.IdListaPytan)
                .HasName("PK__ListaPyt__9538E8B2B27FED04");

            entity.ToTable("ListaPytan");

            entity.Property(e => e.IdListaPytan)
                .ValueGeneratedNever()
                .HasColumnName("idListaPytan");

            entity.Property(e => e.IdPytanie).HasColumnName("idPytanie");

            entity.Property(e => e.IdTest).HasColumnName("idTest");

            entity.HasOne(d => d.IdPytanieNavigation)
                .WithMany(p => p.ListaPytan)
                .HasForeignKey(d => d.IdPytanie)
                .HasConstraintName("FK__ListaPyta__idPyt__236943A5");

            entity.HasOne(d => d.IdTestNavigation)
                .WithMany(p => p.ListaPytan)
                .HasForeignKey(d => d.IdTest)
                .HasConstraintName("FK__ListaPyta__idTes__22751F6C");
        });
        modelBuilder.Entity<ListaPytan>().ToTable("ListaPytan");

        modelBuilder.Entity<Odpowiedz>(entity =>
        {
            entity.HasKey(e => e.IdOdpowiedz)
                .HasName("PK__Odpowied__8191FEDD18BA74B6");

            entity.ToTable("Odpowiedz");

            entity.Property(e => e.IdOdpowiedz)
                .ValueGeneratedNever()
                .HasColumnName("idOdpowiedz");

            entity.Property(e => e.CzyPoprawny).HasColumnName("czyPoprawny");

            entity.Property(e => e.IdPytanie).HasColumnName("idPytanie");

            entity.Property(e => e.TrescOdpowiedzi)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trescOdpowiedzi");

            entity.HasOne(d => d.IdPytanieNavigation)
                .WithMany(p => p.Odpowiedz)
                .HasForeignKey(d => d.IdPytanie)
                .HasConstraintName("FK__Odpowiedz__idPyt__1BC821DD");
        });
        modelBuilder.Entity<Odpowiedz>().ToTable("Odpowiedz");



        modelBuilder.Entity<Pytanie>(entity =>
        {
            entity.HasKey(e => e.IdPytanie)
                .HasName("PK__Pytanie__113F4174C806F9BE");

            entity.ToTable("Pytanie");

            entity.Property(e => e.IdPytanie)
                .ValueGeneratedNever()
                .HasColumnName("idPytanie");

            entity.Property(e => e.IdKategoriaPytania).HasColumnName("idKategoriaPytania");

            entity.Property(e => e.IdNauczyciela).HasColumnName("idNauczyciela");

            entity.Property(e => e.IdTypPytania).HasColumnName("idTypPytania");

            entity.Property(e => e.Tresc)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tresc");

            entity.HasOne(d => d.IdKategoriaPytaniaNavigation)
                .WithMany(p => p.Pytanie)
                .HasForeignKey(d => d.IdKategoriaPytania)
                .HasConstraintName("FK__Pytanie__idKateg__17F790F9");


            entity.HasOne(d => d.IdTypPytaniaNavigation)
                .WithMany(p => p.Pytanie)
                .HasForeignKey(d => d.IdTypPytania)
                .HasConstraintName("FK__Pytanie__idTypPy__18EBB532");
        });
        modelBuilder.Entity<Pytanie>().ToTable("Pytanie");

        modelBuilder.Entity<Rozwiazanie>(entity =>
        {
            entity.HasKey(e => e.IdRozwiazanie)
                .HasName("PK__Rozwiaza__22559DFBF3E2098A");

            entity.ToTable("Rozwiazanie");

            entity.Property(e => e.IdRozwiazanie)
                .ValueGeneratedNever()
                .HasColumnName("idRozwiazanie");

            entity.Property(e => e.IdTest).HasColumnName("idTest");

            entity.Property(e => e.IdUcznia).HasColumnName("idUcznia");

            entity.Property(e => e.LiczbaPunktow).HasColumnName("liczbaPunktow");

            entity.HasOne(d => d.IdTestNavigation)
                .WithMany(p => p.Rozwiazanie)
                .HasForeignKey(d => d.IdTest)
                .HasConstraintName("FK__Rozwiazan__idTes__2739D489");

        });
        modelBuilder.Entity<Rozwiazanie>().ToTable("Rozwiazanie");

        modelBuilder.Entity<RozwiazanieDoPytan>(entity =>
        {
            entity.HasKey(e => e.IdRozwiazanieDoPytan)
                .HasName("PK__Rozwiaza__A8AE837DE95B2289");

            entity.ToTable("RozwiazanieDoPytan");

            entity.Property(e => e.IdRozwiazanieDoPytan)
                .ValueGeneratedNever()
                .HasColumnName("idRozwiazanieDoPytan");

            entity.Property(e => e.IdOdpowiedz).HasColumnName("idOdpowiedz");

            entity.Property(e => e.IdRozwiazanie).HasColumnName("idRozwiazanie");

            entity.HasOne(d => d.IdOdpowiedzNavigation)
                .WithMany(p => p.RozwiazanieDoPytan)
                .HasForeignKey(d => d.IdOdpowiedz)
                .HasConstraintName("FK__Rozwiazan__idOdp__2A164134");

            entity.HasOne(d => d.IdRozwiazanieNavigation)
                .WithMany(p => p.RozwiazanieDoPytan)
                .HasForeignKey(d => d.IdRozwiazanie)
                .HasConstraintName("FK__Rozwiazan__idRoz__2B0A656D");
        });
        modelBuilder.Entity<RozwiazanieDoPytan>().ToTable("RozwiazanieDoPytan");

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus)
                .HasName("PK__Status__01936F74DC087EC8");

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus)
                .ValueGeneratedNever()
                .HasColumnName("idStatus");

            entity.Property(e => e.Nazwa)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");

            entity.Property(e => e.Opis)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("opis");
        });
        modelBuilder.Entity<Status>().ToTable("Status");

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.IdTest)
                .HasName("PK__Test__BCD9141ACEA52A53");

            entity.ToTable("Test");

            entity.Property(e => e.IdTest)
                .ValueGeneratedNever()
                .HasColumnName("idTest");

            entity.Property(e => e.CzasTrwania).HasColumnName("czasTrwania");

            entity.Property(e => e.CzyWidoczny).HasColumnName("czyWidoczny");

            entity.Property(e => e.DataRozpoczecia)
                .HasColumnType("datetime")
                .HasColumnName("dataRozpoczecia");

            entity.Property(e => e.DataUtworzenia)
                .HasColumnType("date")
                .HasColumnName("dataUtworzenia");

            entity.Property(e => e.DataZakonczenia)
                .HasColumnType("datetime")
                .HasColumnName("dataZakonczenia");

            entity.Property(e => e.IdGrupy).HasColumnName("idGrupy");

            entity.Property(e => e.IdNauczyciela).HasColumnName("idNauczyciela");

            entity.Property(e => e.Opis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("opis");

            entity.Property(e => e.Tytul)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("tytul");

            entity.HasOne(d => d.IdGrupyNavigation)
                .WithMany(p => p.Test)
                .HasForeignKey(d => d.IdGrupy)
                .HasConstraintName("FK__Test__idGrupy__1EA48E88");

        });
        modelBuilder.Entity<Test>().ToTable("Test");

        modelBuilder.Entity<TypPytania>(entity =>
        {
            entity.HasKey(e => e.IdTypPytania)
                .HasName("PK__TypPytan__DAB940EFA2DC3E05");

            entity.Property(e => e.IdTypPytania)
                .ValueGeneratedNever()
                .HasColumnName("idTypPytania");

            entity.Property(e => e.Nazwa)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");

            entity.Property(e => e.Opis)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("opis");
        });
        modelBuilder.Entity<TypPytania>().ToTable("TypPytania");

        modelBuilder.Entity<Uczestnicy>(entity =>
        {
            entity.HasKey(e => e.IdUczestnicy)
                .HasName("PK__Uczestni__BCB318FB22C3A301");

            entity.ToTable("Uczestnicy");

            entity.Property(e => e.IdUczestnicy)
                .ValueGeneratedNever()
                .HasColumnName("idUczestnicy");

            entity.Property(e => e.IdGrupy).HasColumnName("idGrupy");

            entity.Property(e => e.IdUcznia).HasColumnName("idUcznia");

            entity.HasOne(d => d.IdGrupyNavigation)
                .WithMany(p => p.Uczestnicy)
                .HasForeignKey(d => d.IdGrupy)
                .HasConstraintName("FK__Uczestnic__idGru__0F624AF8");

        });
        modelBuilder.Entity<Uczestnicy>().ToTable("Uczestnicy");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
