﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Shared.Services;

#nullable disable

namespace Shared.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20230924124735_v5")]
    partial class v5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DockerFlow.Domain.Log", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("container_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("execution_id")
                        .HasColumnType("uuid");

                    b.Property<string>("info")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("system")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("logs");
                });

            modelBuilder.Entity("DockerFlow.Domain.SystemLog", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("container_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("execution_id")
                        .HasColumnType("uuid");

                    b.Property<string>("info")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("system")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.ToTable("system_logs");
                });
#pragma warning restore 612, 618
        }
    }
}
