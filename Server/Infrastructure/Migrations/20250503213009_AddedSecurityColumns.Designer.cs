﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyManager.Infrastructure.Database;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoneyManager.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250503213009_AddedSecurityColumns")]
    partial class AddedSecurityColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Accounts.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountTypeId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("CreatedOn")
                        .HasColumnType("date");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountTypeId");

                    b.HasIndex("CurrencyId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Accounts.AccountType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AccountType");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a08f5553-379e-4294-a2e5-75e88219433c"),
                            Active = true,
                            Name = "Cash"
                        },
                        new
                        {
                            Id = new Guid("cda2ce07-551e-48cf-988d-270c0d022866"),
                            Active = true,
                            Name = "Debit card"
                        },
                        new
                        {
                            Id = new Guid("6ea1867f-c067-412c-b443-8b9bc2467202"),
                            Active = true,
                            Name = "Credit card"
                        },
                        new
                        {
                            Id = new Guid("b9dcea63-49f4-47bf-ae7b-eb596479ba57"),
                            Active = true,
                            Name = "Deposit account"
                        });
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Brokers.Broker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Broker");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("AssetsValue")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BrokerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BrokerId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("TypeId");

                    b.ToTable("BrokerAccount");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccountSecurity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BrokerAccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("InitialPrice")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SecurityId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BrokerAccountId");

                    b.HasIndex("SecurityId");

                    b.ToTable("BrokerAccountSecurity");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccountType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BrokerAccountType");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Currencies.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Active")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Currency");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c7f31af3-5091-439c-8854-c90872420ecf"),
                            Active = true,
                            Name = "USD"
                        },
                        new
                        {
                            Id = new Guid("08762100-b4e0-4a70-b48c-dd9e9411c4a2"),
                            Active = true,
                            Name = "RUB"
                        },
                        new
                        {
                            Id = new Guid("4c073dc4-20be-44d4-80c9-3f09d4ac12ef"),
                            Active = true,
                            Name = "EUR"
                        });
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Deposits.Deposit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("EstimatedEarn")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("From")
                        .HasColumnType("date");

                    b.Property<decimal>("InitialAmount")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("To")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Deposit");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Securities.Security", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("ActualPrice")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("PriceFetchedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Ticker")
                        .HasColumnType("text");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Security");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Securities.SecurityTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BrokerAccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Commission")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<Guid>("SecurityId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Tax")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BrokerAccountId");

                    b.HasIndex("SecurityId");

                    b.ToTable("SecurityTransaction");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Securities.SecurityType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SecurityType");

                    b.HasData(
                        new
                        {
                            Id = new Guid("23b0a73a-9ac1-4fb5-a763-3c10424ed798"),
                            Name = "Stock"
                        },
                        new
                        {
                            Id = new Guid("16184209-1716-4854-a293-75776e1b4ec0"),
                            Name = "Bond"
                        },
                        new
                        {
                            Id = new Guid("209dcb50-989f-44f7-b886-0d7f5c763593"),
                            Name = "Investment fund unit"
                        });
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Transactions.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Cashback")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<bool>("IsSystem")
                        .HasColumnType("boolean");

                    b.Property<decimal>("MoneyQuantity")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("TransactionType")
                        .HasColumnType("text");

                    b.Property<Guid>("TransactionTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Transactions.TransactionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Extension")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransactionType");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Accounts.Account", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Accounts.AccountType", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyManager.Infrastructure.Entities.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountType");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccount", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Brokers.Broker", "Broker")
                        .WithMany()
                        .HasForeignKey("BrokerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyManager.Infrastructure.Entities.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccountType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Broker");

                    b.Navigation("Currency");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccountSecurity", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccount", "BrokerAccount")
                        .WithMany()
                        .HasForeignKey("BrokerAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyManager.Infrastructure.Entities.Securities.Security", "Security")
                        .WithMany()
                        .HasForeignKey("SecurityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrokerAccount");

                    b.Navigation("Security");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Deposits.Deposit", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Accounts.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Securities.Security", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Securities.SecurityType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Securities.SecurityTransaction", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Brokers.BrokerAccount", "BrokerAccount")
                        .WithMany()
                        .HasForeignKey("BrokerAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MoneyManager.Infrastructure.Entities.Securities.Security", "Security")
                        .WithMany()
                        .HasForeignKey("SecurityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrokerAccount");

                    b.Navigation("Security");
                });

            modelBuilder.Entity("MoneyManager.Infrastructure.Entities.Transactions.Transaction", b =>
                {
                    b.HasOne("MoneyManager.Infrastructure.Entities.Accounts.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });
#pragma warning restore 612, 618
        }
    }
}
