using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccaProduction.Models
{
    public partial class AccaCandidatesContext : IdentityDbContext
    {
        public AccaCandidatesContext()
        {
        }

        public AccaCandidatesContext(DbContextOptions<AccaCandidatesContext> options)
            : base(options)
        {
        }

        
        public virtual DbSet<Ispit> Ispit { get; set; }
        public virtual DbSet<Kandidat> Kandidat { get; set; }
        public virtual DbSet<Polaganja> Polaganja { get; set; }
        public virtual DbSet<Rok> Rok { get; set; }
        public virtual DbSet<StatusPrijave> StatusPrijave { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-PFK93A1\\SQLEXPRESS;Database=Acca Candidates;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.Entity<Ispit>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NewCode)
                    .IsRequired()
                    .HasColumnName("New_code")
                    .HasMaxLength(3);

                entity.Property(e => e.OldCode)
                    .IsRequired()
                    .HasColumnName("Old_code")
                    .HasMaxLength(2);
            });

            modelBuilder.Entity<Kandidat>(entity =>
            {
                entity.HasKey(e => e.IdAccaNumber);

                entity.Property(e => e.IdAccaNumber)
                    .HasColumnName("ID(ACCA_Number)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Drzava)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Odeljenje)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Prezime)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Polaganja>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IspitId).HasColumnName("Ispit_ID");

                entity.Property(e => e.KandidatId).HasColumnName("Kandidat_ID");

                entity.Property(e => e.PotrebneKnjige).HasColumnName("Potrebne_Knjige");

                entity.Property(e => e.RokId).HasColumnName("Rok_ID");

                entity.Property(e => e.StatusId).HasColumnName("Status_ID");

                entity.Property(e => e.RequestDate).HasColumnName("RequestDate").HasColumnType("date");

                entity.Property(e => e.StudyLeaveEndDate)
                    .HasColumnName("StudyLeave_EndDate")
                    .HasColumnType("date");

                entity.Property(e => e.StudyLeaveStartDate)
                    .HasColumnName("StudyLeave_StartDate")
                    .HasColumnType("date");

                entity.HasOne(d => d.Ispit)
                    .WithMany(p => p.Polaganja)
                    .HasForeignKey(d => d.IspitId)
                    .HasConstraintName("FK_Polaganja_Ispit");

                entity.HasOne(d => d.Kandidat)
                    .WithMany(p => p.Polaganja)
                    .HasForeignKey(d => d.KandidatId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Polaganja_Kandidat");

                entity.HasOne(d => d.Rok)
                    .WithMany(p => p.Polaganja)
                    .HasForeignKey(d => d.RokId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Polaganja_Rok");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Polaganja)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Polaganja_Status_Prijave");
            });

            modelBuilder.Entity<Rok>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NazivRoka)
                    .IsRequired()
                    .HasMaxLength(15);
            });

            modelBuilder.Entity<StatusPrijave>(entity =>
            {
                entity.ToTable("Status_Prijave");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.StatusName)
                    .IsRequired()
                    .HasColumnName("Status_Name")
                    .HasMaxLength(50);
            });
        }
    }
}
