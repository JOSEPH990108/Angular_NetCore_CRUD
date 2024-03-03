using CRUDAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Skillset> Skillsets { get; set; }
    public DbSet<Hobby> Hobbies { get; set; }
    public DbSet<UserHobby> UserHobbies { get; set; }
    public DbSet<UserSkillset> UserSkillsets { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserHobby>()
            .HasKey(uh => new { uh.UserId, uh.HobbyId });

        modelBuilder.Entity<UserSkillset>()
            .HasKey(us => new { us.UserId, us.SkillsetId });

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserHobbies)
            .WithOne(uh => uh.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.UserSkillsets)
            .WithOne(us => us.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserHobby>()
        .HasOne(uh => uh.User)
        .WithMany(u => u.UserHobbies)
        .HasForeignKey(uh => uh.UserId)
        .OnDelete(DeleteBehavior.Cascade); // This line is added to cascade delete

        modelBuilder.Entity<UserHobby>()
            .HasOne(uh => uh.Hobby)
            .WithMany(h => h.UserHobbies)
            .HasForeignKey(uh => uh.HobbyId)
            .OnDelete(DeleteBehavior.Cascade); // This line is added to cascade delete

        modelBuilder.Entity<UserSkillset>()
            .HasOne(us => us.User)
            .WithMany(u => u.UserSkillsets)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade); // This line is added to cascade delete

        modelBuilder.Entity<UserSkillset>()
            .HasOne(us => us.Skillset)
            .WithMany(s => s.UserSkillsets)
            .HasForeignKey(us => us.SkillsetId)
            .OnDelete(DeleteBehavior.Cascade); // This line is added to cascade delete
    }
}
