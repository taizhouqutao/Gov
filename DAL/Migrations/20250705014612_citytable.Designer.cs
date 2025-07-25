﻿// <auto-generated />
using System;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(webapplicationContext))]
    [Migration("20250705014612_citytable")]
    partial class citytable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DAL.Modles.BizLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("ActionDesc")
                        .HasColumnType("longtext")
                        .HasColumnName("ActionDesc");

                    b.Property<string>("ActionJson")
                        .HasColumnType("longtext")
                        .HasColumnName("ActionJson");

                    b.Property<string>("ActionRemark")
                        .HasColumnType("longtext")
                        .HasColumnName("ActionRemark");

                    b.Property<string>("ActionType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ActionType");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<string>("ModelTitle")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ModelTitle");

                    b.Property<string>("OptUserName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("OptUserName");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("BizLogs");
                });

            modelBuilder.Entity("DAL.Modles.Carousel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<int?>("CityId")
                        .HasColumnType("int")
                        .HasColumnName("CityId");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ImageUrl");

                    b.Property<int>("IsPublic")
                        .HasColumnType("int")
                        .HasColumnName("IsPublic");

                    b.Property<string>("LinkUrl")
                        .HasColumnType("longtext")
                        .HasColumnName("LinkUrl");

                    b.Property<DateTime?>("PublicTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("PublicTime");

                    b.Property<int?>("PublicUserId")
                        .HasColumnType("int")
                        .HasColumnName("PublicUserId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Title");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("Carousels");
                });

            modelBuilder.Entity("DAL.Modles.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("CityName");

                    b.Property<int>("CitySort")
                        .HasColumnType("int")
                        .HasColumnName("CitySort");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("DAL.Modles.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Content");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("FatherCommentId")
                        .HasColumnType("int")
                        .HasColumnName("FatherComment");

                    b.Property<int>("IfDeal")
                        .HasColumnType("int")
                        .HasColumnName("IfDeal");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<int>("IsShow")
                        .HasColumnType("int")
                        .HasColumnName("IsShow");

                    b.Property<int>("NewId")
                        .HasColumnType("int")
                        .HasColumnName("NewId");

                    b.Property<string>("PersonCellphone")
                        .HasColumnType("longtext")
                        .HasColumnName("PersonCellphone");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("PersonName");

                    b.Property<int>("RoleType")
                        .HasColumnType("int")
                        .HasColumnName("RoleType");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("DAL.Modles.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<string>("Cellphone")
                        .HasColumnType("longtext")
                        .HasColumnName("Cellphone");

                    b.Property<int?>("CityId")
                        .HasColumnType("int")
                        .HasColumnName("CityId");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<string>("Depent")
                        .HasColumnType("longtext")
                        .HasColumnName("Depent");

                    b.Property<string>("Desc")
                        .HasColumnType("longtext")
                        .HasColumnName("Desc");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<string>("PersonHead")
                        .HasColumnType("longtext")
                        .HasColumnName("PersonHead");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("PersonName");

                    b.Property<string>("Post")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Post");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("DAL.Modles.ContactMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<int>("ContactId")
                        .HasColumnType("int")
                        .HasColumnName("ContactId");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Content");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<int>("FatherContactMessageId")
                        .HasColumnType("int")
                        .HasColumnName("FatherContactMessageId");

                    b.Property<int>("IfDeal")
                        .HasColumnType("int")
                        .HasColumnName("IfDeal");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<int?>("IsShow")
                        .HasColumnType("int")
                        .HasColumnName("IsShow");

                    b.Property<string>("PersonCellphone")
                        .HasColumnType("longtext")
                        .HasColumnName("PersonCellphone");

                    b.Property<string>("PersonName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("PersonName");

                    b.Property<int>("RoleType")
                        .HasColumnType("int")
                        .HasColumnName("RoleType");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.ToTable("ContactMessages");
                });

            modelBuilder.Entity("DAL.Modles.Duty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<bool>("AllDay")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("AllDay");

                    b.Property<int?>("CityId")
                        .HasColumnType("int")
                        .HasColumnName("CityId");

                    b.Property<int>("ContactId")
                        .HasColumnType("int")
                        .HasColumnName("ContactId");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("CreateTime");

                    b.Property<int>("CreateUserId")
                        .HasColumnType("int")
                        .HasColumnName("CreateUserId");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("EndDate");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("StartDate");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("Dutys");
                });

            modelBuilder.Entity("DAL.Modles.New", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    b.Property<int?>("CityId")
                        .HasColumnType("int")
                        .HasColumnName("CityId");

                    b.Property<int>("CommentCount")
                        .HasColumnType("int")
                        .HasColumnName("CommentCount");

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

                    b.Property<int?>("NewTypeId")
                        .HasColumnType("int")
                        .HasColumnName("NewTypeId");

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

                    b.Property<int>("ViewCount")
                        .HasColumnType("int")
                        .HasColumnName("ViewCount");

                    b.HasKey("Id");

                    b.ToTable("News");
                });

            modelBuilder.Entity("DAL.Modles.NewType", b =>
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

                    b.Property<string>("NewTypeName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("NewTypeName");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.HasKey("Id");

                    b.ToTable("NewTypes");
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

                    b.Property<int>("Enable")
                        .HasColumnType("int")
                        .HasColumnName("Enable");

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

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("UserEmail");

                    b.Property<string>("UserHead")
                        .HasColumnType("longtext")
                        .HasColumnName("UserHead");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("UserName");

                    b.Property<string>("UserPost")
                        .HasColumnType("longtext")
                        .HasColumnName("UserPost");

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

            modelBuilder.Entity("DAL.Modles.ViewLog", b =>
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

                    b.Property<int>("NewId")
                        .HasColumnType("int")
                        .HasColumnName("NewId");

                    b.Property<int>("NewTypeId")
                        .HasColumnType("int")
                        .HasColumnName("NewTypeId");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.Property<int>("ViewCount")
                        .HasColumnType("int")
                        .HasColumnName("ViewCount");

                    b.Property<DateTime>("ViewData")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("ViewData");

                    b.Property<int>("ViewType")
                        .HasColumnType("int")
                        .HasColumnName("ViewType");

                    b.HasKey("Id");

                    b.ToTable("ViewLogs");
                });

            modelBuilder.Entity("DAL.Modles.Weather", b =>
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

                    b.Property<decimal?>("High")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("High");

                    b.Property<int>("IfDel")
                        .HasColumnType("int")
                        .HasColumnName("IfDel");

                    b.Property<decimal?>("Low")
                        .HasColumnType("decimal(65,30)")
                        .HasColumnName("Low");

                    b.Property<string>("TextDay")
                        .HasColumnType("longtext")
                        .HasColumnName("TextDay");

                    b.Property<string>("TextNight")
                        .HasColumnType("longtext")
                        .HasColumnName("TextNight");

                    b.Property<DateTime?>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("UpdateTime");

                    b.Property<int?>("UpdateUserId")
                        .HasColumnType("int")
                        .HasColumnName("UpdateUserId");

                    b.Property<string>("WcDay")
                        .HasColumnType("longtext")
                        .HasColumnName("WcDay");

                    b.Property<string>("WcNight")
                        .HasColumnType("longtext")
                        .HasColumnName("WcNight");

                    b.Property<string>("WdDay")
                        .HasColumnType("longtext")
                        .HasColumnName("WdDay");

                    b.Property<string>("WdNight")
                        .HasColumnType("longtext")
                        .HasColumnName("WdNight");

                    b.Property<DateTime>("WeatherDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("WeatherDate");

                    b.Property<string>("Week")
                        .HasColumnType("longtext")
                        .HasColumnName("Week");

                    b.HasKey("Id");

                    b.ToTable("Weathers");
                });
#pragma warning restore 612, 618
        }
    }
}
