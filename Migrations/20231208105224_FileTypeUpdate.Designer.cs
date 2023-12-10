﻿// <auto-generated />
using System;
using DevagramCShrap.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevagramCShrap.Migrations
{
    [DbContext(typeof(DevagramContext))]
    [Migration("20231208105224_FileTypeUpdate")]
    partial class FileTypeUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DevagramCShrap.Models.Comentario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DataComentario")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdPublicacao")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPublicacao");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Curtida", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdPublicacao")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPublicacao");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Curtidas");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Favoritar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("IdPublicacao")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPublicacao");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Favoritar");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Publicacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DataPublicacao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Publicacoes");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Seguidor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("IdUsuarioSeguido")
                        .HasColumnType("int");

                    b.Property<int?>("IdUsuarioSeguidor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuarioSeguido");

                    b.HasIndex("IdUsuarioSeguidor");

                    b.ToTable("Seguidores");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FotoPerfil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Comentario", b =>
                {
                    b.HasOne("DevagramCShrap.Models.Publicacao", "Publicacao")
                        .WithMany()
                        .HasForeignKey("IdPublicacao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevagramCShrap.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publicacao");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Curtida", b =>
                {
                    b.HasOne("DevagramCShrap.Models.Publicacao", "Publicacao")
                        .WithMany()
                        .HasForeignKey("IdPublicacao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevagramCShrap.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publicacao");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Favoritar", b =>
                {
                    b.HasOne("DevagramCShrap.Models.Publicacao", "Publicacao")
                        .WithMany()
                        .HasForeignKey("IdPublicacao")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DevagramCShrap.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Publicacao");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Publicacao", b =>
                {
                    b.HasOne("DevagramCShrap.Models.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("DevagramCShrap.Models.Seguidor", b =>
                {
                    b.HasOne("DevagramCShrap.Models.Usuario", "UsuarioSeguido")
                        .WithMany()
                        .HasForeignKey("IdUsuarioSeguido");

                    b.HasOne("DevagramCShrap.Models.Usuario", "UsuarioSeguidor")
                        .WithMany()
                        .HasForeignKey("IdUsuarioSeguidor");

                    b.Navigation("UsuarioSeguido");

                    b.Navigation("UsuarioSeguidor");
                });
#pragma warning restore 612, 618
        }
    }
}
