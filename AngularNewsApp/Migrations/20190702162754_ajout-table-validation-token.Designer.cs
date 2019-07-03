﻿// <auto-generated />
using System;
using AngularNewsApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AngularNewsApp.Migrations
{
    [DbContext(typeof(NewsAppContext))]
    [Migration("20190702162754_ajout-table-validation-token")]
    partial class ajouttablevalidationtoken
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AngularNewsApp.Models.Article", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("date");

                    b.Property<string>("description");

                    b.Property<int>("id_rss_link");

                    b.Property<string>("link");

                    b.Property<string>("subtitle");

                    b.Property<string>("title");

                    b.HasKey("id");

                    b.HasIndex("id_rss_link");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("AngularNewsApp.Models.ArticleUser", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("id_article");

                    b.Property<int>("id_user");

                    b.HasKey("id");

                    b.HasIndex("id_article");

                    b.HasIndex("id_user");

                    b.ToTable("ArticleUser");
                });

            modelBuilder.Entity("AngularNewsApp.Models.Category", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("libelle");

                    b.HasKey("id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("AngularNewsApp.Models.RssLink", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("id_category");

                    b.Property<int?>("id_source");

                    b.Property<string>("link");

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.HasIndex("id_category");

                    b.HasIndex("id_source");

                    b.ToTable("RssLink");
                });

            modelBuilder.Entity("AngularNewsApp.Models.Source", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("name");

                    b.HasKey("id");

                    b.ToTable("Source");
                });

            modelBuilder.Entity("AngularNewsApp.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("email");

                    b.Property<string>("login");

                    b.Property<string>("nom");

                    b.Property<string>("password");

                    b.Property<string>("prenom");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AngularNewsApp.Models.UserValidationToken", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("date");

                    b.Property<int>("id_user");

                    b.Property<string>("token");

                    b.HasKey("id");

                    b.HasIndex("id_user");

                    b.ToTable("UserValidationToken");
                });

            modelBuilder.Entity("AngularNewsApp.Models.Article", b =>
                {
                    b.HasOne("AngularNewsApp.Models.RssLink", "Link")
                        .WithMany("Articles")
                        .HasForeignKey("id_rss_link")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AngularNewsApp.Models.ArticleUser", b =>
                {
                    b.HasOne("AngularNewsApp.Models.Article", "Article")
                        .WithMany("ArticleUsers")
                        .HasForeignKey("id_article")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AngularNewsApp.Models.User", "User")
                        .WithMany("ArticleUsers")
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AngularNewsApp.Models.RssLink", b =>
                {
                    b.HasOne("AngularNewsApp.Models.Category", "Category")
                        .WithMany("RssLink")
                        .HasForeignKey("id_category");

                    b.HasOne("AngularNewsApp.Models.Source", "Source")
                        .WithMany("Links")
                        .HasForeignKey("id_source");
                });

            modelBuilder.Entity("AngularNewsApp.Models.UserValidationToken", b =>
                {
                    b.HasOne("AngularNewsApp.Models.User", "User")
                        .WithMany("Validations")
                        .HasForeignKey("id_user")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
