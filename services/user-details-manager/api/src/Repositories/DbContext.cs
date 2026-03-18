using Microsoft.EntityFrameworkCore;
using User.Models.Domains;

namespace User.Respositories.DatabaseContext;

public class UserDbContext : DbContext
{
  public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
  {
  }

  public DbSet<UserEntity> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<UserEntity>(entity =>
    {
      entity.ToTable("user_details", "users");
      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Email).HasColumnName("email");
      entity.Property(e => e.FirstName).HasColumnName("first_name");
      entity.Property(e => e.LastName).HasColumnName("last_name");
    });
  }

}