﻿// <auto-generated />
using BlackJackDAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlackJackDAL.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20231026114846_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UtilityLib.Game.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerId"));

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
                    b.Navigation("Stats")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
