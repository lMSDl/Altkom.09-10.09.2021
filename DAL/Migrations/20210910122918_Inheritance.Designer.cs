﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210910122918_Inheritance")]
    partial class Inheritance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.HasSequence<int>("ProductPrice", "sequences")
                .StartsAt(5L)
                .IncrementsBy(2)
                .HasMin(1L)
                .HasMax(10L)
                .IsCyclic();

            modelBuilder.Entity("Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<int>("DaysFromOrder")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("DATEDIFF(DAY,[DateTime],GETDATE())");

                    b.Property<DateTime>("EditedDateTime")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Models.OrderSummary", b =>
                {
                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.ToView("View_OrderSummary");
                });

            modelBuilder.Entity("Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Person");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Person");
                });

            modelBuilder.Entity("Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("N/A");

                    b.Property<DateTime>("EditedDateTime")
                        .IsConcurrencyToken()
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)")
                        .HasComputedColumnSql("[Category] + ': ' + [Name]", true);

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValueSql("NEXT VALUE FOR sequences.ProductPrice");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("_secret")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValueSql("NEWID()");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Models.AddressInCity", b =>
                {
                    b.HasBaseType("Models.Address");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("AddressInCity");
                });

            modelBuilder.Entity("Models.AddressInTown", b =>
                {
                    b.HasBaseType("Models.Address");

                    b.Property<string>("Town")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("AddressInTown");
                });

            modelBuilder.Entity("Models.Student", b =>
                {
                    b.HasBaseType("Models.Person");

                    b.Property<float>("AvegargeDegree")
                        .HasColumnType("real");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("Models.Teacher", b =>
                {
                    b.HasBaseType("Models.Person");

                    b.Property<string>("Specialization")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("Models.Product", b =>
                {
                    b.HasOne("Models.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Models.AddressInCity", b =>
                {
                    b.HasOne("Models.Address", null)
                        .WithOne()
                        .HasForeignKey("Models.AddressInCity", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.AddressInTown", b =>
                {
                    b.HasOne("Models.Address", null)
                        .WithOne()
                        .HasForeignKey("Models.AddressInTown", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Order", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
