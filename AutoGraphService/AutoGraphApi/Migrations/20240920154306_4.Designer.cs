﻿// <auto-generated />
using System;
using AutoGraphApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoGraphApi.Migrations
{
    [DbContext(typeof(AutoGraphContext))]
    [Migration("20240920154306_4")]
    partial class _4
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AutoGraphApi.Models.AutoGraphDriverEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("DriverFullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("driver_full_name");

                    b.Property<Guid>("DriverId")
                        .HasColumnType("uuid")
                        .HasColumnName("driver_id");

                    b.Property<string>("DriverStringId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("driver_string_id");

                    b.HasKey("Id");

                    b.ToTable("auto_graph_drivers");
                });

            modelBuilder.Entity("AutoGraphApi.Models.AutoGraphMachinesEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("MachineId")
                        .HasColumnType("uuid")
                        .HasColumnName("machine_id");

                    b.Property<string>("MachineName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("machine_name");

                    b.Property<string>("MachineRegNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("machine_reg_number");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uuid")
                        .HasColumnName("parent_id");

                    b.HasKey("Id");

                    b.ToTable("auto_graph_machines");
                });

            modelBuilder.Entity("AutoGraphApi.Models.AutoGraphSchemaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("SchemaId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("schema_id");

                    b.Property<string>("SchemaName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("schema_name");

                    b.HasKey("Id");

                    b.ToTable("auto_graph_schemas");
                });

            modelBuilder.Entity("AutoGraphApi.Models.AutoGraphUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.Property<string>("TelegramChatId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("telegram_chat_id");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.ToTable("auto_graph_users");
                });
#pragma warning restore 612, 618
        }
    }
}
