﻿// <auto-generated />
using System;
using CivicHub.Persistance.Contexts.CivicHubContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CivicHub.Persistance.Migrations
{
    [DbContext(typeof(CivicHubContext))]
    [Migration("20250210064235_UpdateLengthOfPhoneNumber")]
    partial class UpdateLengthOfPhoneNumber
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CivicHub.Domain.Cities.City", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                    b.HasKey("Code");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Code = "TB",
                            Name = "Tbilisi"
                        },
                        new
                        {
                            Code = "BT",
                            Name = "Batumi"
                        });
                });

            modelBuilder.Entity("CivicHub.Domain.Persons.Entities.PersonConnections.PersonConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("ConnectedPersonId")
                        .HasColumnType("bigint");

                    b.Property<string>("ConnectionType")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ConnectedPersonId");

                    b.HasIndex("PersonId", "ConnectedPersonId", "ConnectionType")
                        .IsUnique()
                        .HasFilter("[ConnectionType] IS NOT NULL");

                    b.ToTable("PersonConnections");
                });

            modelBuilder.Entity("CivicHub.Domain.Persons.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CityCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                        .HasAnnotation("MinLength", 2);

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .UseCollation("SQL_Latin1_General_CP1_CI_AS")
                        .HasAnnotation("MinLength", 2);

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("PictureFullPath")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.HasIndex("CityCode");

                    b.HasIndex("PersonalNumber")
                        .IsUnique();

                    b.HasIndex("FirstName", "LastName", "PersonalNumber");

                    b.HasIndex("FirstName", "LastName", "PersonalNumber", "Gender", "BirthDate");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("CivicHub.Domain.Persons.Entities.PersonConnections.PersonConnection", b =>
                {
                    b.HasOne("CivicHub.Domain.Persons.Person", "ConnectedPerson")
                        .WithMany("ConnectedTo")
                        .HasForeignKey("ConnectedPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CivicHub.Domain.Persons.Person", "Person")
                        .WithMany("Connections")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ConnectedPerson");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("CivicHub.Domain.Persons.Person", b =>
                {
                    b.HasOne("CivicHub.Domain.Cities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("CivicHub.Domain.Persons.ValueObjects.PhoneNumbers.PhoneNumber", "PhoneNumbers", b1 =>
                        {
                            b1.Property<long>("PersonId")
                                .HasColumnType("bigint");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasMaxLength(50)
                                .HasColumnType("nvarchar(50)");

                            b1.Property<int>("Type")
                                .HasColumnType("int");

                            b1.HasKey("PersonId", "Id");

                            b1.ToTable("PhoneNumber");

                            b1.WithOwner()
                                .HasForeignKey("PersonId");
                        });

                    b.Navigation("City");

                    b.Navigation("PhoneNumbers");
                });

            modelBuilder.Entity("CivicHub.Domain.Persons.Person", b =>
                {
                    b.Navigation("ConnectedTo");

                    b.Navigation("Connections");
                });
#pragma warning restore 612, 618
        }
    }
}
