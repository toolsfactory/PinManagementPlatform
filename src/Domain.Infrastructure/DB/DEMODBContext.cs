﻿using Microsoft.EntityFrameworkCore;

namespace PinPlatform.Domain.Infrastructure.DB
{
    public partial class DEMODBContext : DbContext
    {
        public DEMODBContext(DbContextOptions<DEMODBContext> options)
            : base(options)
        {            
        }

        public virtual DbSet<Pins> Pins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pins>(entity =>
            {
                entity.HasKey(e => new {e.OpcoId, e.HouseholdId, e.ProfileId, e.PinType })
                    .HasName("PRIMARY");

                entity.Property(e => e.OpcoId)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsRequired();

                entity.Property(e => e.HouseholdId)
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.ProfileId).HasColumnType("int(11)");

                entity.Property(e => e.PinType).HasColumnType("int(11)");

                entity.Property(e => e.Comments).IsRequired();

                entity.Property(e => e.PinHash)
                    .IsRequired()
                    .HasMaxLength(64)
                    .IsUnicode(false);

                entity.Property(e => e.PinLocked).HasColumnType("tinyint(3) unsigned");

                entity.Property(e => e.PinSalt)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });
        }
    }
}
