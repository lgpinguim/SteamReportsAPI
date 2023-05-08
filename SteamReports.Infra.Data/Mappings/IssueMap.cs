using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SteamReports.Domain.Models;

namespace SteamReports.Infra.Data.Mappings
{
    public class IssueMap : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.ToTable("stats_steam_review_issues");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id)
                .HasColumnType("bigint")
                .HasColumnName("Id");

            builder.Property(i => i.Name)
                .HasColumnType("character")
                .HasColumnName("name");

            builder.Property(i => i.ResolvedStatus)
                .HasColumnType("bigint")
                .HasColumnName("resolved_status");


        }
    }
}
