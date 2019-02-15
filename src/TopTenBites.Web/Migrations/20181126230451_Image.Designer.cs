﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TopTenBites.Web.Data;

namespace TopTenBites.Web.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20181126230451_Image")]
    partial class Image
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Business", b =>
                {
                    b.Property<int>("BusinessId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("YelpBusinessAlias");

                    b.Property<string>("YelpBusinessId");

                    b.HasKey("BusinessId");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int>("MenuItemId");

                    b.Property<int>("Sentiment");

                    b.Property<string>("Text");

                    b.HasKey("CommentId");

                    b.HasIndex("MenuItemId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<int>("MenuItemId");

                    b.Property<string>("Name");

                    b.HasKey("ImageId");

                    b.HasIndex("MenuItemId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Like", b =>
                {
                    b.Property<long>("LikeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<bool>("IsLike");

                    b.Property<int>("MenuItemId");

                    b.Property<string>("UserFingerPrintHash");

                    b.HasKey("LikeId");

                    b.HasIndex("MenuItemId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.MenuItem", b =>
                {
                    b.Property<int>("MenuItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BusinessId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetDate()");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("MenuItemId");

                    b.HasIndex("BusinessId");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Comment", b =>
                {
                    b.HasOne("TopTenBites.Web.ApplicationCore.Models.MenuItem", "MenuItem")
                        .WithMany("Comments")
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Image", b =>
                {
                    b.HasOne("TopTenBites.Web.ApplicationCore.Models.MenuItem")
                        .WithMany("Images")
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.Like", b =>
                {
                    b.HasOne("TopTenBites.Web.ApplicationCore.Models.MenuItem", "MenuItem")
                        .WithMany("Likes")
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TopTenBites.Web.ApplicationCore.Models.MenuItem", b =>
                {
                    b.HasOne("TopTenBites.Web.ApplicationCore.Models.Business", "Business")
                        .WithMany("MenuItems")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
