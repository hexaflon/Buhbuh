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

        public virtual DbSet<Group> Group { get; set; } = null!;
        public virtual DbSet<QuestionCategory> QuestionCategory { get; set; } = null!;
        public virtual DbSet<QuestionList> QuestionList { get; set; } = null!;
        public virtual DbSet<Answer> Answer { get; set; } = null!;
        public virtual DbSet<Question> Question { get; set; } = null!;
        public virtual DbSet<Result> Result { get; set; } = null!;
        public virtual DbSet<QuestionResult> QuestionResult { get; set; } = null!;
        public virtual DbSet<Status> Status { get; set; } = null!;
        public virtual DbSet<Test> Test { get; set; } = null!;
        public virtual DbSet<QuestionType> QuestionType { get; set; } = null!;
        public virtual DbSet<Participant> Participant { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DB_Login");
            }
        }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Grupy__EC597A91075872A5");

            entity.ToTable("Grupy");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idGrupy");

            entity.Property(e => e.TeacherId).HasColumnName("idNauczyciela");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");
        });
        modelBuilder.Entity<Group>().ToTable("Grupy");

        modelBuilder.Entity<QuestionCategory>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Kategori__D603FA56976D8D88");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idKategoriaPytania");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("opis");
        });

        modelBuilder.Entity<QuestionCategory>().ToTable("KategoriaPytania");

        modelBuilder.Entity<QuestionList>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__ListaPyt__9538E8B2B27FED04");

            entity.ToTable("ListaPytan");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idListaPytan");

            entity.Property(e => e.QuestionId).HasColumnName("idPytanie");

            entity.Property(e => e.TestId).HasColumnName("idTest");

            entity.HasOne(d => d.Questions)
                .WithMany(p => p.QuestionList)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__ListaPyta__idPyt__236943A5");

            entity.HasOne(d => d.Tests)
                .WithMany(p => p.Questions)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK__ListaPyta__idTes__22751F6C");
        });
        modelBuilder.Entity<QuestionList>().ToTable("ListaPytan");

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Odpowied__8191FEDD18BA74B6");

            entity.ToTable("Odpowiedz");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idOdpowiedz");

            entity.Property(e => e.IsCorrect).HasColumnName("czyPoprawny");

            entity.Property(e => e.QuestionId).HasColumnName("idPytanie");

            entity.Property(e => e.Text)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("trescOdpowiedzi");

            entity.HasOne(d => d.Questions)
                .WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Odpowiedz__idPyt__1BC821DD");
        });
        modelBuilder.Entity<Answer>().ToTable("Odpowiedz");



        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Pytanie__113F4174C806F9BE");

            entity.ToTable("Pytanie");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idPytanie");

            entity.Property(e => e.CategoryId).HasColumnName("idKategoriaPytania");

            entity.Property(e => e.TeacherId).HasColumnName("idNauczyciela");

            entity.Property(e => e.TypeId).HasColumnName("idTypPytania");

            entity.Property(e => e.Text)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("tresc");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Question)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Pytanie__idKateg__17F790F9");


            entity.HasOne(d => d.Type)
                .WithMany(p => p.Question)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK__Pytanie__idTypPy__18EBB532");
        });
        modelBuilder.Entity<Question>().ToTable("Pytanie");

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Rozwiaza__22559DFBF3E2098A");

            entity.ToTable("Rozwiazanie");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idRozwiazanie");

            entity.Property(e => e.TestId).HasColumnName("idTest");

            entity.Property(e => e.StudentId).HasColumnName("idUcznia");

            entity.Property(e => e.Points).HasColumnName("liczbaPunktow");

            entity.HasOne(d => d.Tests)
                .WithMany(p => p.Result)
                .HasForeignKey(d => d.TestId)
                .HasConstraintName("FK__Rozwiazan__idTes__2739D489");

        });
        modelBuilder.Entity<Result>().ToTable("Rozwiazanie");

        modelBuilder.Entity<QuestionResult>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Rozwiaza__A8AE837DE95B2289");

            entity.ToTable("RozwiazanieDoPytan");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idRozwiazanieDoPytan");

            entity.Property(e => e.AnswerId).HasColumnName("idOdpowiedz");

            entity.Property(e => e.ResultId).HasColumnName("idRozwiazanie");

            entity.HasOne(d => d.Answers)
                .WithMany(p => p.QuestionResults)
                .HasForeignKey(d => d.AnswerId)
                .HasConstraintName("FK__Rozwiazan__idOdp__2A164134");

            entity.HasOne(d => d.Results)
                .WithMany(p => p.QuestionResult)
                .HasForeignKey(d => d.ResultId)
                .HasConstraintName("FK__Rozwiazan__idRoz__2B0A656D");
        });
        modelBuilder.Entity<QuestionResult>().ToTable("RozwiazanieDoPytan");

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Status__01936F74DC087EC8");

            entity.ToTable("Status");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idStatus");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");

            entity.Property(e => e.Description)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("opis");
        });
        modelBuilder.Entity<Status>().ToTable("Status");

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Test__BCD9141ACEA52A53");

            entity.ToTable("Test");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idTest");

            entity.Property(e => e.Duration).HasColumnName("czasTrwania");

            entity.Property(e => e.IsVisible).HasColumnName("czyWidoczny");

            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("dataRozpoczecia");

            entity.Property(e => e.CreationDate)
                .HasColumnType("date")
                .HasColumnName("dataUtworzenia");

            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("dataZakonczenia");

            entity.Property(e => e.GroupId).HasColumnName("idGrupy");

            entity.Property(e => e.TeacherId).HasColumnName("idNauczyciela");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("opis");

            entity.Property(e => e.Title)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("tytul");

            entity.HasOne(d => d.Groups)
                .WithMany(p => p.Test)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Test__idGrupy__1EA48E88");

        });
        modelBuilder.Entity<Test>().ToTable("Test");

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__TypPytan__DAB940EFA2DC3E05");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idTypPytania");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("nazwa");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("opis");
        });
        modelBuilder.Entity<QuestionType>().ToTable("TypPytania");

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__Uczestni__BCB318FB22C3A301");

            entity.ToTable("Uczestnicy");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("idUczestnicy");

            entity.Property(e => e.GroupId).HasColumnName("idGrupy");

            entity.Property(e => e.StudentId).HasColumnName("idUcznia");

            entity.HasOne(d => d.Groups)
                .WithMany(p => p.Participants)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__Uczestnic__idGru__0F624AF8");

        });
        modelBuilder.Entity<Participant>().ToTable("Uczestnicy");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
