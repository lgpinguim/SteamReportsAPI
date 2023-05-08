using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteamReports.Domain.Models;

namespace SteamReports.Infra.Data.Mappings
{
    public class ReviewMap : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("stats_steam_reviews");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnType("integer")
                .HasColumnName("Id");

            builder.Property(r => r.Recommended)
                .HasColumnType("boolean")
                .HasColumnName("recommended");

            builder.Property(r => r.UserName)
                .HasColumnType("character")
                .HasColumnName("user_name");

            builder.Property(r => r.ReviewText)
                .HasColumnType("character")
                .HasColumnName("review_text");

            builder.Property(r => r.HoursPlayed)
                .HasColumnType("numeric")
                .HasColumnName("hours_played");

            builder.Property(r => r.ReviewUrl)
                .HasColumnType("character")
                .HasColumnName("review_url");

            builder.Property(r => r.DatePosted)
                .HasColumnType("timestamp")
                .HasColumnName("date_posted");

            builder.Property(r => r.DateUpdated)
                .HasColumnType("timestamp")
                .HasColumnName("date_updated");

            builder.Property(r => r.HelpfulAmount)
                .HasColumnType("integer")
                .HasColumnName("helpful_amount");

            builder.Property(r => r.HelpfulTotal)
                .HasColumnType("integer")
                .HasColumnName("helpful_total");

            builder.Property(r => r.OwnedGamesAmount)
                .HasColumnType("integer")
                .HasColumnName("owned_games_amount");

            builder.Property(r => r.RespondedBy)
                .HasColumnType("bigint")
                .HasColumnName("responded_by");

            builder.Property(r => r.RespondedTimeStamp)
                .HasColumnType("timestamp")
                .HasColumnName("responded_timestamp");

            builder.Property(r => r.IssueList)
                .HasColumnType("bigint[]")
                .HasColumnName("issue_list");

            builder.Property(r => r.CanBeTurned)
                .HasColumnType("boolean")
                .HasColumnName("can_be_turned");

            builder.Property(r => r.SteamAppId)
                .HasColumnType("bigint")
                .HasColumnName("steam_appid");

            builder.Property(r => r.LangKey)
                .HasColumnType("character")
                .HasColumnName("lang_key");

            builder.Property(r => r.ReceivedCompensation)
                .HasColumnType("boolean")
                .HasColumnName("received_compensation");

            builder.HasOne(r => r.SteamApp)
                .WithMany(s => s.Reviews)
                .HasForeignKey(r => r.SteamAppId);
        }
    }
}
