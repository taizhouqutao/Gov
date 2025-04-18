﻿// <auto-generated />
using System;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(webapplicationContext))]
    partial class webapplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DAL.Modles.New", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<int>("IsPublic")
                        .HasColumnType("int")
                        .HasColumnName("IsPublic");

                    b.Property<string>("NewContent")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("NewContent");

                    b.Property<string>("NewTitle")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("NewTitle");

                    b.Property<DateTime?>("PublicTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("PublicTime");

                    b.Property<int?>("PublicUserId")
                        .HasColumnType("int")
                        .HasColumnName("PublicUserId");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("DAL.Modles.Right", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("FatherId")
                        .HasColumnType("int")
                        .HasColumnName("FatherId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<string>("RightCode")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("RightCode");

                    b.Property<string>("RightName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("RightName");

                    b.Property<string>("RightType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("RightType");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("Rights");
                });

            modelBuilder.Entity("DAL.Modles.RightRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<int>("RightId")
                        .HasColumnType("int")
                        .HasColumnName("RightId");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("RoleId");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("RightRoles");
                });

            modelBuilder.Entity("DAL.Modles.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("RoleName");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DAL.Modles.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("PassWord");

                    b.Property<string>("RealName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("RealName");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL.Modles.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("RoleId");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
