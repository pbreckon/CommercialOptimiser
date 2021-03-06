﻿// <auto-generated />
using System;
using CommercialOptimiser.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CommercialOptimiser.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20200714220722_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.BreakDemographicTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BreakId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("DemographicId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rating")
                        .HasColumnName("Rating")
                        .HasColumnType("INTEGER");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("Capacity")
                        .HasColumnName("Capacity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InvalidCommercialTypes")
                        .HasColumnName("InvalidCommercialTypes")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("Title")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Break");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.CommercialTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CommercialType")
                        .HasColumnName("CommercialType")
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<int?>("DemographicId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnName("Title")
                        .HasColumnType("TEXT")
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
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnName("Title")
                        .HasColumnType("TEXT")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Demographic");
                });

            modelBuilder.Entity("CommercialOptimiser.Data.Tables.UserReportBreakTable", b =>
                {
                    b.Property<string>("UserUniqueId")
                        .HasColumnName("UserUniqueId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Report")
                        .HasColumnType("TEXT");

                    b.HasKey("UserUniqueId");

                    b.ToTable("UserReportBreak");
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
#pragma warning restore 612, 618
        }
    }
}
