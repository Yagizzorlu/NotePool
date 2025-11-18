using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NotePool.Domain.Entities;
using NotePool.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Persistence.Contexts
{
    public class NotePoolDbContext : IdentityDbContext<User, AppRole, Guid>
    {
        public NotePoolDbContext(DbContextOptions options) : base(options) //Bu Constructor, IoC de doldurulacak. Base'a yolluyor.
        { }

        //public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Domain.Entities.File> Files { get; set; }
        public DbSet<NotePdfFile> NotePdfFiles { get; set; }
        public DbSet<NoteDownload> NoteDownloads { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NotePdfFile>()
                .HasOne(npf => npf.Note)
                .WithMany(n => n.NotePdfFiles)
                .HasForeignKey(npf => npf.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotePdfFile>()
                .Property(npf => npf.FileName)
                .IsRequired();

            modelBuilder.Entity<NotePdfFile>()
                .Property(npf => npf.Path)
                .IsRequired();

            modelBuilder.Entity<Department>()
                .HasOne(d => d.Institution)
                .WithMany(i => i.Departments)
                .HasForeignKey(d => d.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Institution)
                .WithMany()
                .HasForeignKey(u => u.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Institution)
                .WithMany()
                .HasForeignKey(n => n.InstitutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany()
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Department)
                .WithMany()
                .HasForeignKey(n => n.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.Course)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Note)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.Note)
                .WithMany(n => n.Reactions)
                .HasForeignKey(r => r.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.Note)
                .WithMany(n => n.Bookmarks)
                .HasForeignKey(b => b.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteDownload>()
                .HasOne(nd => nd.Note)
                .WithMany()
                .HasForeignKey(nd => nd.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookmarks)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteDownload>()
                .HasOne(nd => nd.User)
                .WithMany(u => u.NoteDownloads)
                .HasForeignKey(nd => nd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.UserId, r.NoteId })
                .IsUnique();

            modelBuilder.Entity<Comment>()
                .HasOne(reply => reply.Parent)
                .WithMany(parent => parent.Replies)
                .HasForeignKey(reply => reply.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteDownload>()
                .HasOne(nd => nd.Note)
                .WithMany()
                .HasForeignKey(nd => nd.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteDownload>()
                .HasOne(nd => nd.User)
                .WithMany(u => u.NoteDownloads)
                .HasForeignKey(nd => nd.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteDownload>()
                .HasIndex(nd => new { nd.UserId, nd.NoteId })
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) //parametresiz
        {
            var datas = ChangeTracker
                .Entries<BaseEntity>();
            foreach (var data in datas)
            {
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    //ChangeTracker : entitylerdeki değişiklikleri veya yeni eklenen verilerin yakalanmasını sağlayan property.Update operasyonlarında track edilen verileri yakalayıp elde etmemizi sağlar.
}
