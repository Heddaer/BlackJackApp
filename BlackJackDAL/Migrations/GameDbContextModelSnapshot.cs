﻿// <auto-generated />
using System;
using BlackJackDAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlackJackDAL.Migrations
{
    [DbContext(typeof(GameDbContext))]
    partial class GameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UtilityLib.Game.BettingData", b =>
                {
                    b.Property<int>("BettingDataID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BettingDataID"));

                    b.Property<double>("Bet")
                        .HasColumnType("float");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("PlayerID")
                        .HasColumnType("int");

                    b.HasKey("BettingDataID");

                    b.HasIndex("PlayerID");

                    b.ToTable("BettingData");
                });

            modelBuilder.Entity("UtilityLib.Game.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerId"));

                    b.Property<double>("Coins")
                        .HasColumnType("float");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("UtilityLib.Game.PlayerStats", b =>
                {
                    b.Property<int>("PlayerStatsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerStatsId"));

                    b.Property<int>("Losses")
                        .HasColumnType("int");

                    b.Property<int>("PlayerID")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("PlayerStatsId");

                    b.HasIndex("PlayerID")
                        .IsUnique();

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("UtilityLib.Game.BettingData", b =>
                {
                    b.HasOne("UtilityLib.Game.Player", "Player")
                        .WithMany("BettingData")
                        .HasForeignKey("PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("UtilityLib.Game.PlayerStats", b =>
                {
                    b.HasOne("UtilityLib.Game.Player", "Player")
                        .WithOne("Stats")
                        .HasForeignKey("UtilityLib.Game.PlayerStats", "PlayerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("UtilityLib.Game.Player", b =>
                {
                    b.Navigation("BettingData");

                    b.Navigation("Stats")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
