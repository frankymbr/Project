﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Project_API.Datos;

#nullable disable

namespace Project_API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230326150315_updtablevilla")]
    partial class updtablevilla
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Project_API.Models.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateCreation")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Detail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Dimension")
                        .HasColumnType("int");

                    b.Property<string>("ImagenUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Occupants")
                        .HasColumnType("int");

                    b.Property<double?>("Tarifa")
                        .IsRequired()
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            DateCreation = new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8366),
                            DateUpdate = new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8383),
                            Detail = "Detalle de la villa...",
                            Dimension = 50,
                            ImagenUrl = "",
                            Name = "Villa Real",
                            Occupants = 5,
                            Tarifa = 200.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            DateCreation = new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8387),
                            DateUpdate = new DateTime(2023, 3, 26, 10, 3, 15, 456, DateTimeKind.Local).AddTicks(8389),
                            Detail = "Detalle de la villa Caraz...",
                            Dimension = 100,
                            ImagenUrl = "",
                            Name = "Villa Caraz",
                            Occupants = 10,
                            Tarifa = 400.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
