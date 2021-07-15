using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IrregularVerbs.Models
{
    public partial class IrregularVerbsContext : DbContext
    {
        public IrregularVerbsContext(DbContextOptions<IrregularVerbsContext> options) : base(options)
        {

        }

        public virtual DbSet<Incorrect> Incorrects { get; set; }
        public virtual DbSet<Irregular> Irregulars { get; set; }
        public virtual DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Incorrect>(entity =>
            {
                entity.ToTable("Incorrect");

                entity.Property(e => e.CorrectAnswerFirst).HasMaxLength(255);

                entity.Property(e => e.CorrectAnswerSecond).HasMaxLength(255);

                entity.Property(e => e.GivenVerb).HasMaxLength(255);

                entity.Property(e => e.SubmittedAnswerFirst).HasMaxLength(255);

                entity.Property(e => e.SubmittedAnswerSecond).HasMaxLength(255);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<Irregular>(entity =>
            {
                entity.ToTable("Irregular");

                entity.Property(e => e.BaseForm).HasMaxLength(255);

                entity.Property(e => e.PastParticiple).HasMaxLength(255);

                entity.Property(e => e.PastSimple).HasMaxLength(255);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result");

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
