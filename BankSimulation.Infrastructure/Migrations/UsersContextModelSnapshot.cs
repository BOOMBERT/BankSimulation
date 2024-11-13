﻿// <auto-generated />
using System;
using BankSimulation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankSimulation.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class UsersContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankSimulation.Domain.Entities.BankAccount", b =>
                {
                    b.Property<string>("Number")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<decimal>("Balance")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Number");

                    b.HasIndex("Number")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.Deposit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankAccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountNumber");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(88)
                        .HasColumnType("nvarchar(88)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.SecurityQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("SecurityQuestions");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<decimal>("RecipientAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RecipientBankAccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)");

                    b.Property<decimal>("SenderAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SenderBankAccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)");

                    b.HasKey("Id");

                    b.HasIndex("RecipientBankAccountNumber");

                    b.HasIndex("SenderBankAccountNumber");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessRoles")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.Withdraw", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("BankAccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(6)");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountNumber");

                    b.ToTable("Withdraws");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.BankAccount", b =>
                {
                    b.HasOne("BankSimulation.Domain.Entities.User", "User")
                        .WithMany("BankAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.Deposit", b =>
                {
                    b.HasOne("BankSimulation.Domain.Entities.BankAccount", "BankAccount")
                        .WithMany("Deposits")
                        .HasForeignKey("BankAccountNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("BankSimulation.Domain.Entities.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("BankSimulation.Domain.Entities.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.SecurityQuestion", b =>
                {
                    b.HasOne("BankSimulation.Domain.Entities.User", "User")
                        .WithOne("SecurityQuestion")
                        .HasForeignKey("BankSimulation.Domain.Entities.SecurityQuestion", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.Transfer", b =>
                {
                    b.HasOne("BankSimulation.Domain.Entities.BankAccount", "RecipientBankAccount")
                        .WithMany("ReceivedTransfers")
                        .HasForeignKey("RecipientBankAccountNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("BankSimulation.Domain.Entities.BankAccount", "SenderBankAccount")
                        .WithMany("SentTransfers")
                        .HasForeignKey("SenderBankAccountNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("RecipientBankAccount");

                    b.Navigation("SenderBankAccount");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.Withdraw", b =>
                {
                    b.HasOne("BankSimulation.Domain.Entities.BankAccount", "BankAccount")
                        .WithMany("Withdraws")
                        .HasForeignKey("BankAccountNumber")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.BankAccount", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("ReceivedTransfers");

                    b.Navigation("SentTransfers");

                    b.Navigation("Withdraws");
                });

            modelBuilder.Entity("BankSimulation.Domain.Entities.User", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("RefreshToken")
                        .IsRequired();

                    b.Navigation("SecurityQuestion")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
