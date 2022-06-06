﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;

#nullable disable

namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Migrations
{
    [DbContext(typeof(DietMenuDbContext))]
    partial class DietMenuDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("SilentMike")
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.CoreEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Versions")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.HasKey("Id");

                    b.ToTable("Core", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.CoreIngredientEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Exchanger")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UnitSymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("CoreIngredients", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.CoreIngredientTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CoreIngredientTypes", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.CoreMealTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CoreMealTypes", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.FamilyEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Versions")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

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

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

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

                    b.Property<bool>("IsActive")
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

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

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

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.RecipeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Carbohydrates")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Energy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Fat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("MealTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Protein")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("FamilyId");

                    b.HasIndex("MealTypeId");

                    b.ToTable("Recipes", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.RecipeIngredientEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IngredientId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid?>("RecipeEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RecipeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("IngredientId");

                    b.HasIndex("RecipeEntityId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeIngredients", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models.IngredientRow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Exchanger")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitSymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IngredientRows", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models.IngredientTypeRow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IngredientTypeRows", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models.MealTypeRow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("MealTypeRows", "SilentMike");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.CoreIngredientEntity", b =>
                {
                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.CoreIngredientTypeEntity", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
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

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.RecipeEntity", b =>
                {
                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.FamilyEntity", "FamilyEntity")
                        .WithMany()
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.MealTypeEntity", "MealType")
                        .WithMany()
                        .HasForeignKey("MealTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FamilyEntity");

                    b.Navigation("MealType");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.RecipeIngredientEntity", b =>
                {
                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.IngredientEntity", "Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.RecipeEntity", null)
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeEntityId");

                    b.HasOne("SilentMike.DietMenu.Core.Domain.Entities.RecipeEntity", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("SilentMike.DietMenu.Core.Domain.Entities.RecipeEntity", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}
