using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Migrations
{
    [DbContext(typeof(CurrencyDataContext))]
    [Migration("20160409134012_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.CurrencyCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("Value");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.CurrencyData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActualDate");

                    b.Property<string>("ActualDateString");

                    b.Property<int?>("CurrencyCodeId");

                    b.Property<string>("Name");

                    b.Property<string>("ShortName");

                    b.Property<double>("Value");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.CurrencyData", b =>
                {
                    b.HasOne("CurrencyExplorer.Models.Entities.CurrencyCode")
                        .WithMany()
                        .HasForeignKey("CurrencyCodeId");
                });
        }
    }
}