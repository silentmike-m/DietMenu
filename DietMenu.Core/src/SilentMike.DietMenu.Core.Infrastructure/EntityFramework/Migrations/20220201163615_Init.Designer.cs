﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    [DbContext(typeof(DietMenuDbContext))]
    [Migration("20220201163615_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("SilentMike")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.FamilyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Families", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.IngredientEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Exchanger")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UnitSymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.HasIndex("FamilyId", "InternalName")
                        .IsUnique();

                    b.HasIndex("FamilyId", "Name")
                        .IsUnique();

                    b.ToTable("Ingredients", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.IngredientTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId", "InternalName")
                        .IsUnique();

                    b.HasIndex("FamilyId", "Name")
                        .IsUnique();

                    b.ToTable("IngredientTypes", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.MealTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId", "InternalName")
                        .IsUnique();

                    b.HasIndex("FamilyId", "Name")
                        .IsUnique();

                    b.ToTable("MealTypes", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.IngredientEntity", b =>
                {
                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.FamilyEntity", "FamilyEntity")
                        .WithMany()
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.IngredientTypeEntity", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FamilyEntity");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.IngredientTypeEntity", b =>
                {
                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.FamilyEntity", "FamilyEntity")
                        .WithMany()
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FamilyEntity");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.MealTypeEntity", b =>
                {
                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.FamilyEntity", "FamilyEntity")
                        .WithMany()
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FamilyEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
