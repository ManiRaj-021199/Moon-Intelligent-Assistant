﻿using Microsoft.EntityFrameworkCore;
using MoonIntelligentAssistant.Data.Entities;

namespace MoonIntelligentAssistant.Data.DBContext;

public partial class MoonIaContext : DbContext
{
    #region Properties
    public virtual DbSet<User>? Users { get; set; }
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
        modelBuilder.Entity<User>(entity =>
                                  {
                                      entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C8AE607C0");

                                      entity.ToTable("Users", "User");

                                      entity.Property(e => e.Email).HasMaxLength(75);
                                      entity.Property(e => e.FirstName).HasMaxLength(50);
                                      entity.Property(e => e.LastName).HasMaxLength(50);
                                  });

        OnModelCreatingPartial(modelBuilder);
    }
    #endregion

    #region Privates
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    #endregion
}