using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyField.Models;

namespace MyField.Data
{
    public class Ksans_SportsDbContext : IdentityDbContext
    {
        public Ksans_SportsDbContext(DbContextOptions<Ksans_SportsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys here
            modelBuilder.Entity<Standing>().HasKey(c => c.StandingId);
            modelBuilder.Entity<Club>().HasKey(c => c.ClubId);
            modelBuilder.Entity<Fixture>().HasKey(f => f.FixtureId);
            modelBuilder.Entity<MatchResults>().HasKey(r => r.ResultsId);
            modelBuilder.Entity<SportNews>().HasKey(n => n.NewsId);
            modelBuilder.Entity<PlayerTransferMarket>().HasKey(n => n.PlayerTransferMarketId);
            modelBuilder.Entity<Transfer>().HasKey(n => n.TransferId);

            modelBuilder.Entity<ClubAdministrator>()
                 .HasBaseType<UserBaseModel>();

            modelBuilder.Entity<ClubManager>()
                 .HasBaseType<UserBaseModel>();

            modelBuilder.Entity<Player>()
                 .HasBaseType<UserBaseModel>();

            // Configure relationships
            modelBuilder.Entity<Fixture>()
                .HasOne(f => f.HomeTeam)
                .WithMany()
                .HasForeignKey(f => f.HomeTeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Club>()
                .HasOne(c => c.ClubManager)
                .WithOne(cm => cm.Club)
                .HasForeignKey<ClubManager>(cm => cm.ClubId)
                .HasPrincipalKey<Club>(c => c.ClubId)
                .IsRequired(false);

            modelBuilder.Entity<Club>()
               .HasOne(c => c.ClubManager)
               .WithOne(cm => cm.Club)
               .HasForeignKey<ClubManager>(cm => cm.ClubId)
               .HasPrincipalKey<Club>(c => c.ClubId)
               .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Fixture>()
                .HasOne(f => f.AwayTeam)
                .WithMany()
                .HasForeignKey(f => f.AwayTeamId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<PlayerTransferMarket>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<PlayerTransferMarket>("BaseType")
                .HasValue<PlayerTransferMarketArchive>("ArchiveType");

            modelBuilder.Entity<UserBaseModel>()
                 .HasDiscriminator<string>("UserType")
                 .HasValue<Player>("Player")
                 .HasValue<ClubAdministrator>("ClubAdministrator")
                 .HasValue<ClubManager>("ClubManager");

            modelBuilder.Entity<MatchFormation>()
                 .HasDiscriminator<string>("Discriminator")
                 .HasValue<MatchFormation>("MatchFormation")
                 .HasValue<MatchFormation_Archive>("MatchFormation_Archive");




            modelBuilder.Entity<Fixture>()
                .HasIndex(f => f.KickOffDate);
        }

        public DbSet<MyField.Models.Club> Club { get; set; } = default!;
        public DbSet<MyField.Models.Standing> Standing { get; set; } = default!;
        public DbSet<MyField.Models.SportNews> SportNew { get; set; } = default!;
        public DbSet<MyField.Models.Fixture> Fixture { get; set; } = default!;
        public DbSet<MyField.Models.MatchResults> MatchResult { get; set; } = default!;

        public DbSet<MyField.Models.UserBaseModel> SystemUsers { get; set; } = default!;

        public DbSet<MyField.Models.Tournament> Tournament { get; set; } = default!;

        public DbSet<MyField.Models.HeadTohead> HeadToHead { get; set; } = default!;

        public DbSet<MyField.Models.Meeting> Meeting { get; set; } = default!;

        public DbSet<MyField.Models.LineUpXIHolder> LineUpXIHolder { get; set; } = default!;

        public DbSet<MyField.Models.LineUpXI> LineUpXI { get; set; } = default!;

        public DbSet<MyField.Models.LineUpSubstitutesHolder> LineUpSubstitutesHolder { get; set; } = default!;

        public DbSet<MyField.Models.LineUpSubstitutes> LineUpSubstitutes { get; set; } = default!;

        public DbSet<MyField.Models.LineUp> LineUp { get; set; } = default!;

        public DbSet<MyField.Models.Player> Player { get; set; } = default!;
        public DbSet<MyField.Models.ClubAdministrator> ClubAdministrator { get; set; } = default!;

        public DbSet<MyField.Models.ClubManager> ClubManager { get; set; } = default!;

        public DbSet<MyField.Models.SystemAdministrator> SystemAdministrator{ get; set; } = default!;

        public DbSet<MyField.Models.SportsMember> SportMember { get; set; } = default!;

        public DbSet<MyField.Models.Officials> Officials { get; set; } = default!;

        public DbSet<MyField.Models.MatchOfficials> MatchOfficials { get; set; } = default!;

        public DbSet<MyField.Models.Formation> Formations { get; set; } = default!;

        public DbSet<MyField.Models.MatchFormation> MatchFormation{ get; set; } = default!;

        public DbSet<MyField.Models.Comment> Comments { get; set; } = default!;
        public DbSet<MyField.Models.Fine> Fines { get; set; } = default!;

        public DbSet<MyField.Models.Maintainance> Maintainances { get; set; } = default!;

        public DbSet<MyField.Models.Fan> Fans { get; set; } = default!;

        public DbSet<MyField.Models.ClubWarning> ClubWarnings { get; set; } = default!;

        public DbSet<MyField.Models.Warning> Warnings { get; set; } = default!;

        public DbSet<MyField.Models.League> League { get; set; } = default!;

        public DbSet<MyField.Models.Standings_Archive> Standings_Archive { get; set; } = default!;

        public DbSet<MyField.Models.MatchResults_Archive> MatchResults_Archive { get; set; } = default!;

        public DbSet<MyField.Models.Fixtures_Archive> Fixtures_Archive { get; set; } = default!;

        public DbSet<MyField.Models.Clubs_Archive> Clubs_Archive { get; set; } = default!;

        public DbSet<MyField.Models.Transfer> Transfer { get; set; } = default!;

        public DbSet<MyField.Models.PlayerTransferMarket> PlayerTransferMarket { get; set; } = default!;

        public DbSet<MyField.Models.Payment> Payments { get; set; } = default!;

        public DbSet<MyField.Models.Invoice> Invoices { get; set; } = default!;

        public DbSet<MyField.Models.DeviceInfo> DeviceInfo { get; set; } = default!;

        public DbSet<MyField.Models.PlayerTransferMarketArchive> PlayerTransferMarketArchive { get; set; } = default!;

        public DbSet<MyField.Models.ActivityLog> ActivityLogs { get; set; } = default!;

        public DbSet<MyField.Models.UserBaseModel> UserBaseModel { get; set; } = default!;

        public DbSet<MyField.Models.TransferPeriod> TransferPeriod { get; set; } = default!;

        public DbSet<MyField.Models.MatchFormation_Archive> MatchFormation_Archive { get; set; } = default!;

        public DbSet<MyField.Models.Reports> Reports { get; set; } = default!;

        public DbSet<MyField.Models.MatchReports> MatchReports { get; set; } = default!;

        public DbSet<MyField.Models.TransfersReports> TransfersReports { get; set; } = default!;

        public DbSet<MyField.Models.MatchResultsReports> MatchResultsReports { get; set; } = default!;

        public DbSet<MyField.Models.MatchReports_Archive> MatchReportsArchive { get; set; } = default!;

        public DbSet<MyField.Models.MatchResultsReports_Archive> MatchResultsReports_Archive { get; set; } = default!;

        public DbSet<MyField.Models.TransfersReports_Archive> TransfersReports_Archive { get; set; } = default!;
        public DbSet<MyField.Models.ClubPerformanceReport> ClubPerformanceReports { get; set; } = default!;
        public DbSet<MyField.Models.ClubTransferReport> ClubTransferReports { get; set; } = default!;

        public DbSet<MyField.Models.ClubTransferReports_Archive> ClubTransferReports_Archive { get; set; } = default!;
        public DbSet<MyField.Models.ClubPerformanceReports_Archive> ClubPerformanceReports_Archive { get; set; } = default!;

        public DbSet<MyField.Models.TestUserFeedback> TestUserFeedbacks { get; set; } = default!;
    }
}
