﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Litmus.Entities;

namespace Litmus.Migrations
{
    [DbContext(typeof(LitmusDbContext))]
    [Migration("20160708235517_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Litmus.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("BirthYear");

                    b.Property<string>("DisplayLastChanged");

                    b.Property<string>("Expiration");

                    b.Property<bool>("HasBarcode");

                    b.Property<bool>("HasMagstripe");

                    b.Property<string>("IdNumber");

                    b.Property<bool>("IsDamaged");

                    b.Property<bool>("IsPaper");

                    b.Property<DateTime>("LastChanged");

                    b.Property<string>("Location");

                    b.Property<string>("Notes");

                    b.Property<string>("Orientation");

                    b.Property<string>("State");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Litmus.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Litmus.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CardId");

                    b.Property<string>("CardIdNumber");

                    b.Property<DateTime>("DateChanged");

                    b.Property<string>("DisplayDateChanged");

                    b.Property<string>("NewCard");

                    b.Property<string>("OldCard");

                    b.Property<string>("User");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });
        }
    }
}
