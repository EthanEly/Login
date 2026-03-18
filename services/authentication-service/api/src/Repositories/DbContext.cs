using Microsoft.EntityFrameworkCore;
using Auth.Models.Domains;

namespace Auth.Respositories.DatabaseContext;

public class AuthenticationDbContext : DbContext
{
  public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
  {
  }

  public DbSet<UserAccount> UserAccounts { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<UserAccount>(entity =>
    {
      entity.ToTable("user_accounts", "authentication_service");
      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Email).HasColumnName("email");
      entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
    });
  }

}