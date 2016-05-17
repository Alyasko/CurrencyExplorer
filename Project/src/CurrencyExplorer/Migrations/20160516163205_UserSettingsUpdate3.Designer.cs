using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using CurrencyExplorer.Models.Entities.Database;

namespace CurrencyExplorer.Migrations
{
    [DbContext(typeof(CurrencyDataContext))]
    [Migration("20160516163205_UserSettingsUpdate3")]
    partial class UserSettingsUpdate3
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

                    b.Property<int?>("UserSettingsEntryId");

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

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.UserLanguageEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Language");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.UserSettingsEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ChartBeginTime");

                    b.Property<DateTime>("ChartEndTime");

                    b.Property<long>("CookieUid");

                    b.Property<int?>("LanguageId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.CurrencyCodeEntry", b =>
                {
                    b.HasOne("CurrencyExplorer.Models.Entities.Database.UserSettingsEntry")
                        .WithMany()
                        .HasForeignKey("UserSettingsEntryId");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.CurrencyDataEntry", b =>
                {
                    b.HasOne("CurrencyExplorer.Models.Entities.Database.CurrencyCodeEntry")
                        .WithMany()
                        .HasForeignKey("DbCurrencyCodeEntryId");
                });

            modelBuilder.Entity("CurrencyExplorer.Models.Entities.Database.UserSettingsEntry", b =>
                {
                    b.HasOne("CurrencyExplorer.Models.Entities.Database.UserLanguageEntry")
                        .WithMany()
                        .HasForeignKey("LanguageId");
                });
        }
    }
}
