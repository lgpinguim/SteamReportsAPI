using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteamReports.Domain.Models;

namespace SteamReports.Infra.Data.Mappings
{
    public class SteamAppMap : IEntityTypeConfiguration<SteamApp>
    {
        public void Configure(EntityTypeBuilder<SteamApp> builder)
        {
            builder.ToTable("stats_steam_games");

            builder.HasKey(g => g.SteamAppId);

            builder.Property(g => g.SteamAppId)
                .HasColumnType("bigint")
                .HasColumnName("steam_appid");

            builder.Property(g => g.DisplayName)
                .HasColumnType("character")
                .HasColumnName("display_name");

            builder.HasMany(s => s.Reviews)
                .WithOne(r => r.SteamApp)
                .HasForeignKey(r => r.SteamAppId);


        }
    }
}
