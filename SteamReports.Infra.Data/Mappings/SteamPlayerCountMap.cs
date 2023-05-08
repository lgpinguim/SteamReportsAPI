using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteamReports.Domain.Models;

namespace SteamReports.Infra.Data.Mappings
{
    public class SteamPlayerCountMap : IEntityTypeConfiguration<SteamPlayerCount>
    {
        public void Configure(EntityTypeBuilder<SteamPlayerCount> builder)
        {
            builder.ToTable("stats_steam_player_count");

            builder.HasNoKey();

            builder.Property(e => e.PlayerCount)
                .HasColumnName("player_count");

            builder.Property(e => e.SteamAppId)
                .HasColumnType("bigint")
                .HasColumnName("steam_appid");

            builder.Property(e => e.TimeStamp)
                .HasColumnType("timestamp")
                .HasColumnName("time_stamp");

        }
    }
}
