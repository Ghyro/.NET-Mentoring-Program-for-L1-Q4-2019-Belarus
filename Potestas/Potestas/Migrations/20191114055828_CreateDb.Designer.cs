﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Potestas.Context;

namespace Potestas.Migrations
{
    [DbContext(typeof(ObservationContext))]
    [Migration("20191114055828_CreateDb")]
    partial class CreateDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Potestas.Observations.Wrappers.CoordinatesWrapper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("X");

                    b.Property<double>("Y");

                    b.HasKey("Id");

                    b.ToTable("CoordinatesWrapper");
                });

            modelBuilder.Entity("Potestas.Observations.Wrappers.FlashObservationWrapper", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CoordinatesId");

                    b.Property<int>("DurationMs");

                    b.Property<double>("EstimatedValue");

                    b.Property<double>("Intensity");

                    b.Property<DateTime>("ObservationTime");

                    b.HasKey("Id");

                    b.HasIndex("CoordinatesId");

                    b.ToTable("FlashObservationWrapper");
                });

            modelBuilder.Entity("Potestas.Observations.Wrappers.FlashObservationWrapper", b =>
                {
                    b.HasOne("Potestas.Observations.Wrappers.CoordinatesWrapper", "ObservationPoint")
                        .WithMany("FlashObservations")
                        .HasForeignKey("CoordinatesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
