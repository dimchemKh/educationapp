﻿// <auto-generated />
using System;
using EducationApp.DataAccessLayer.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EducationApp.DataAccessLayer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.ApplicationUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 476, DateTimeKind.Local).AddTicks(4924),
                            IsRemoved = false,
                            Name = "Tom Jackson"
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(5995),
                            IsRemoved = false,
                            Name = "Jack Jill"
                        },
                        new
                        {
                            Id = 3,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(6012),
                            IsRemoved = false,
                            Name = "Dan Bolson"
                        },
                        new
                        {
                            Id = 4,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(6015),
                            IsRemoved = false,
                            Name = "Mark Avreliy"
                        },
                        new
                        {
                            Id = 5,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(6018),
                            IsRemoved = false,
                            Name = "Bob Tomhson"
                        },
                        new
                        {
                            Id = 6,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(6018),
                            IsRemoved = false,
                            Name = "Arthur Bang"
                        });
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.AuthorInPrintingEdition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AuthorId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("Data");

                    b.Property<bool>("IsRemoved");

                    b.Property<int?>("PrintingEditionId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PrintingEditionId");

                    b.ToTable("AuthorInPrintingEditions");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<bool>("IsRemoved");

                    b.Property<int?>("PaymentId");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<int>("Count");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("Currency");

                    b.Property<bool>("IsRemoved");

                    b.Property<int?>("PrintingEditionsIdId");

                    b.HasKey("Id");

                    b.HasIndex("PrintingEditionsIdId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("IsRemoved");

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.PrintingEdition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("Currency");

                    b.Property<string>("Description");

                    b.Property<bool>("IsRemoved");

                    b.Property<string>("Name");

                    b.Property<int>("Price");

                    b.Property<int>("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("PrintingEditions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(7764),
                            Currency = 0,
                            Description = "Text",
                            IsRemoved = false,
                            Name = "Babysister",
                            Price = 20,
                            Status = 0,
                            Type = 1
                        },
                        new
                        {
                            Id = 2,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(9184),
                            Currency = 0,
                            Description = "Some Text",
                            IsRemoved = false,
                            Name = "Butcher",
                            Price = 40,
                            Status = 0,
                            Type = 1
                        },
                        new
                        {
                            Id = 3,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(9201),
                            Currency = 0,
                            Description = "Some Descript",
                            IsRemoved = false,
                            Name = "Planet after us",
                            Price = 10,
                            Status = 0,
                            Type = 3
                        },
                        new
                        {
                            Id = 4,
                            CreationDate = new DateTime(2019, 9, 13, 17, 24, 30, 477, DateTimeKind.Local).AddTicks(9204),
                            Currency = 0,
                            Description = "Some descr",
                            IsRemoved = false,
                            Name = "Peppka Shmepka",
                            Price = 30,
                            Status = 0,
                            Type = 2
                        });
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.AuthorInPrintingEdition", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("EducationApp.DataAccessLayer.Entities.PrintingEdition", "PrintingEdition")
                        .WithMany()
                        .HasForeignKey("PrintingEditionId");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.Order", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId");

                    b.HasOne("EducationApp.DataAccessLayer.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("EducationApp.DataAccessLayer.Entities.OrderItem", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.PrintingEdition", "PrintingEditionsId")
                        .WithMany()
                        .HasForeignKey("PrintingEditionsIdId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EducationApp.DataAccessLayer.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("EducationApp.DataAccessLayer.Entities.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
