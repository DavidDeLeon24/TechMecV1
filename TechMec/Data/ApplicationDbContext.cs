using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechMec.Models;

namespace TechMec.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Handover> Handovers { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure the FromUser relationship
            builder.Entity<Handover>()
                .HasOne(h => h.FromUser)
                .WithMany(u => u.SentHandovers)
                .HasForeignKey(h => h.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the ToUser relationship
            builder.Entity<Handover>()
                .HasOne(h => h.ToUser)
                .WithMany(u => u.ReceivedHandovers)
                .HasForeignKey(h => h.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TaskItem>()
                .HasOne(t => t.Handover)
                .WithMany()
                .HasForeignKey(t => t.HandoverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Note>()
                .HasOne(n => n.Patient)
                .WithMany()
                .HasForeignKey(n => n.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}