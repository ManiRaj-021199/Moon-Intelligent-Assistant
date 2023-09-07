using Microsoft.EntityFrameworkCore;

namespace MoonIntelligentAssistant.Data.DBContext;

public partial class MoonIaContext : DbContext
{
    #region Properties
    public virtual DbSet<AuthUser> AuthUsers { get; set; }

    public virtual DbSet<NonAuthUser> NonAuthUsers { get; set; }
    #endregion

    #region Constructors
    public MoonIaContext()
    {
    }

    public MoonIaContext(DbContextOptions<MoonIaContext> options)
        : base(options)
    {
    }
    #endregion

    #region Protecteds
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(ConnectionStrings.DbConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthUser>(entity =>
                                      {
                                          entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF7A7EDFF");

                                          entity.ToTable("AuthUsers", "User");

                                          entity.Property(e => e.UserEmail).HasMaxLength(75);
                                          entity.Property(e => e.UserName).HasMaxLength(50);
                                      });

        modelBuilder.Entity<NonAuthUser>(entity =>
                                         {
                                             entity.HasKey(e => e.UserId).HasName("PK__NonAuthU__1788CC4CFBC7ECD8");

                                             entity.ToTable("NonAuthUsers", "User");

                                             entity.Property(e => e.AuthCode).HasMaxLength(50);
                                             entity.Property(e => e.UserEmail).HasMaxLength(75);
                                             entity.Property(e => e.UserName).HasMaxLength(50);
                                         });

        OnModelCreatingPartial(modelBuilder);
    }
    #endregion

    #region Privates
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    #endregion
}