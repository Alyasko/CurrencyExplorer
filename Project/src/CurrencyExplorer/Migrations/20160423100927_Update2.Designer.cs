using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Migrations
{
    [DbContext(typeof(CurrencyDataContext))]
    [Migration("20160423100927_Update2")]
    partial class Update2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.CurrencyCodeEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.CurrencyDataEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ActualDate");

                    b.Property<int?>("DbCurrencyCodeEntryId");

                    b.Property<double>("Value");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.CurrencyDataEntry", b =>
                {
                    b.HasOne("CurrencyExplorer.Models.Entities.Database.CurrencyCodeEntry")
                        .WithMany()
                        .HasForeignKey("DbCurrencyCodeEntryId");
                });
        }
    }
}
