﻿// <auto-generated />
using System;
using CommercialOptimiser.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CommercialOptimiser.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.BreakDemographicTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BreakId")
                        .HasColumnType("int");

                    b.Property<int?>("DemographicId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnName("Rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BreakId");

                    b.HasIndex("DemographicId");

                    b.ToTable("BreakDemographic");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.BreakTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity")
                        .HasColumnName("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("InvalidCommercialTypes")
                        .HasColumnName("InvalidCommercialTypes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("Title")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Break");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.CommercialTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommercialType")
                        .HasColumnName("CommercialType")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("DemographicId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnName("Title")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.HasIndex("DemographicId");

                    b.ToTable("Commercial");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.DemographicTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Title")
                        .HasColumnName("Title")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Demographic");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.UserReportBreakCommercialTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CommercialTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<int?>("UserReportBreakId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserReportBreakId");

                    b.ToTable("UserReportBreakCommercial");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.UserReportBreakTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BreakTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserReportBreak");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.UserTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UniqueUserId")
                        .HasColumnName("UniqueUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.BreakDemographicTable", b =>
                {
                    b.HasOne("CommercialOptimiser.Data.Tables.BreakTable", "Break")
                        .WithMany("BreakDemographics")
                        .HasForeignKey("BreakId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CommercialOptimiser.Data.Tables.DemographicTable", "Demographic")
                        .WithMany()
                        .HasForeignKey("DemographicId");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.CommercialTable", b =>
                {
                    b.HasOne("CommercialOptimiser.Data.Tables.DemographicTable", "Demographic")
                        .WithMany()
                        .HasForeignKey("DemographicId");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.UserReportBreakCommercialTable", b =>
                {
                    b.HasOne("CommercialOptimiser.Data.Tables.UserReportBreakTable", "UserReportBreak")
                        .WithMany("UserReportBreakCommercials")
                        .HasForeignKey("UserReportBreakId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.UserReportBreakTable", b =>
                {
                    b.HasOne("CommercialOptimiser.Data.Tables.UserTable", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
