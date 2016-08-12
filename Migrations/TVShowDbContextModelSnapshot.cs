using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WhatNotToWatch.Models;

namespace WhatNotToWatch.Migrations
{
    [DbContext(typeof(TVShowDbContext))]
    partial class TVShowDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WhatNotToWatch.Models.TVShow", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Genre");

                    b.Property<string>("Name");

                    b.Property<int>("Rating");

                    b.Property<string>("Review");

                    b.HasKey("ID");

                    b.ToTable("TVShows");
                });
        }
    }
}
