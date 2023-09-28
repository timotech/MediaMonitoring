using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaMonitoring.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(3600);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Identifier> Identifiers { get; set; }
        public DbSet<CodeFile> CodeFiles { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<TopAdvertisers> TopAdvertisers { get; set; }
        public DbSet<TopAdvertisers2> TopAdvertisers2s { get; set; }
        public DbSet<TopAdvertisers3> TopAdvertisers3s { get; set; }
        public DbSet<TopBrandsInCategory> TopBrandsInCategories { get; set; }
        public DbSet<WatchDog> WatchDogs { get; set; }
        public DbSet<WatchDogRadio> WatchDogRadios { get; set; }
        public DbSet<WatchDogOutdoor> WatchDogOutdoors { get; set; }
        public DbSet<WatchDogPress> WatchDogPresses { get; set; }
        public DbSet<WatchDogSummary> WatchDogSummaries { get; set; }
        public DbSet<WatchDogSummaryPress> WatchDogSummaryPresses { get; set; }
        public DbSet<WatchDogSummaryOutdoor> watchDogSummaryOutdoors { get; set; }
        public DbSet<WatchDogStationAudit> WatchDogStationAudits { get; set; }
        public DbSet<WatchDogStationAuditRadio> WatchDogStationAuditRadios { get; set; }
        public DbSet<AdCluster> AdClusters { get; set; }
        public DbSet<AdClusterPress> AdClusterPresses { get; set; }
        public DbSet<AdClusterOutdoor> AdClusterOutdoors { get; set; }
        public DbSet<AdClusterByMonth> AdClusterByMonths { get; set; }
        public DbSet<AdClusterByMonthPress> AdClusterByMonthPresses { get; set; }
        public DbSet<AdClusterByMonthOutdoor> AdClusterByMonthOutdoors { get; set; }
        public DbSet<AdClusterByTimeSegment> AdClusterByTimeSegments { get; set; }
        public DbSet<AdClusterByTimeSegmentPress> AdClusterByTimeSegmentPresses { get; set; }
        public DbSet<AdClusterByTimeSegmentOutdoor> AdClusterByTimeSegmentOutdoors { get; set; }
        public DbSet<AdClusterByWeek> AdClusterByWeeks { get; set; }
        public DbSet<AdClusterByWeekPress> AdClusterByWeekPresses { get; set; }
        public DbSet<AdClusterByWeekOutdoor> AdClusterByWeekOutdoors { get; set; }
        public DbSet<AdClusterByMaterialLength> AdClusterByMaterialLengths { get; set; }
        public DbSet<AdClusterByMaterialLengthPress> AdClusterByMaterialLengthPresses { get; set; }
        public DbSet<AdClusterByMaterialLengthOutdoor> AdClusterByMaterialLengthOutdoors { get; set; }
        public DbSet<AdClusterAllMedia> AdClusterAllMedias { get; set; }
        public DbSet<MediaSpend> MediaSpends { get; set; }
        public DbSet<MediaSpendByBrand> MediaSpendByBrands { get; set; }
        public DbSet<MediaSpendByProduct> MediaSpendByProducts { get; set; }
        public DbSet<ShareOfVoice> ShareOfVoices { get; set; }
        public DbSet<ShareOfExpenditure> ShareOfExpenditures { get; set; }
        public DbSet<ShareOfCompetitiveWatchDog> ShareOfCompetitiveWatchDogs { get; set; }
        public DbSet<ShareOfExpeditureAllMedia> ShareOfExpeditureAllMedias { get; set; }
        public DbSet<ShareOfVoiceAllMedia> ShareOfVoiceAllMedias { get; set; }
        public DbSet<Reconciliation> Reconciliations { get; set; }
        public DbSet<ReconciliationSummary> ReconciliationSummaries { get; set; }
        public DbSet<TopCampaigns> TopCampaigns { get; set; }
        public DbSet<ShareOfCompetitiveWatchDogOutdoor> ShareOfCompetitiveWatchDogOutdoors { get; set; }
        public DbSet<ShareOfCompetitiveWatchDogPress> ShareOfCompetitiveWatchDogPresses { get; set; }
        public DbSet<MediaSpendMix> MediaSpendMixes { get; set; }
        public DbSet<DurationMix> DurationMixes { get; set; }
        public DbSet<CummulativeSOVSOE> CummulativeSOVSOEs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TopAdvertisers>().HasNoKey().ToView("TopAdvertisers");
            modelBuilder.Entity<TopAdvertisers2>().HasNoKey().ToView("TopAdvertisers2");
            modelBuilder.Entity<TopAdvertisers3>().HasNoKey().ToView("TopAdvertisers3");
            modelBuilder.Entity<TopBrandsInCategory>().HasNoKey().ToView("TopBrandsInCategory");
            modelBuilder.Entity<WatchDog>().HasNoKey().ToView("WatchDog");
            modelBuilder.Entity<WatchDogRadio>().HasNoKey().ToView("WatchDogRadio");
            modelBuilder.Entity<WatchDogOutdoor>().HasNoKey().ToView("WatchDogOutdoor");
            modelBuilder.Entity<WatchDogPress>().HasNoKey().ToView("WatchDogPress");
            modelBuilder.Entity<WatchDogSummary>().HasNoKey().ToView("WatchDogSummary");
            modelBuilder.Entity<WatchDogSummaryPress>().HasNoKey().ToView("WatchDogSummaryPress");
            modelBuilder.Entity<WatchDogSummaryOutdoor>().HasNoKey().ToView("WatchDogSummaryOutdoor");
            modelBuilder.Entity<WatchDogStationAudit>().HasNoKey().ToView("WatchDogStationAudit");
            modelBuilder.Entity<WatchDogStationAuditRadio>().HasNoKey().ToView("WatchDogStationAuditRadio");
            modelBuilder.Entity<AdCluster>().HasNoKey().ToView("AdCluster");
            modelBuilder.Entity<AdClusterAllMedia>().HasNoKey().ToView("AdClusterAllMedia");
            modelBuilder.Entity<AdClusterPress>().HasNoKey().ToView("AdClusterPress");
            modelBuilder.Entity<AdClusterOutdoor>().HasNoKey().ToView("AdClusterOutdoor");
            modelBuilder.Entity<AdClusterByMonth>().HasNoKey().ToView("AdClusterByMonth");
            modelBuilder.Entity<AdClusterByMonthPress>().HasNoKey().ToView("AdClusterByMonthPress");
            modelBuilder.Entity<AdClusterByMonthOutdoor>().HasNoKey().ToView("AdClusterByMonthOutdoor");
            modelBuilder.Entity<AdClusterByTimeSegment>().HasNoKey().ToView("AdClusterByTimeSegment");
            modelBuilder.Entity<AdClusterByTimeSegmentPress>().HasNoKey().ToView("AdClusterByTimeSegmentPress");
            modelBuilder.Entity<AdClusterByTimeSegmentOutdoor>().HasNoKey().ToView("AdClusterByTimeSegmentOutdoor");
            modelBuilder.Entity<AdClusterByMaterialLength>().HasNoKey().ToView("AdClusterByMaterialLength");
            modelBuilder.Entity<AdClusterByMaterialLengthPress>().HasNoKey().ToView("AdClusterByMaterialLengthPress");
            modelBuilder.Entity<AdClusterByMaterialLengthOutdoor>().HasNoKey().ToView("AdClusterByMaterialLengthOutdoor");
            modelBuilder.Entity<AdClusterByWeek>().HasNoKey().ToView("AdClusterByWeek");
            modelBuilder.Entity<AdClusterByWeekPress>().HasNoKey().ToView("AdClusterByWeekPress");
            modelBuilder.Entity<AdClusterByWeekOutdoor>().HasNoKey().ToView("AdClusterByWeekOutdoor");
            modelBuilder.Entity<MediaSpend>().HasNoKey().ToView("MediaSpend");
            modelBuilder.Entity<MediaSpendByBrand>().HasNoKey().ToView("MediaSpendByBrand");
            modelBuilder.Entity<MediaSpendByProduct>().HasNoKey().ToView("MediaSpendByProduct");
            modelBuilder.Entity<ShareOfVoice>().HasNoKey().ToView("ShareOfVoice");
            modelBuilder.Entity<ShareOfExpenditure>().HasNoKey().ToView("ShareOfExpenditure");
            modelBuilder.Entity<ShareOfCompetitiveWatchDog>().HasNoKey().ToView("ShareOfCompetitiveWatchDog");
            modelBuilder.Entity<ShareOfExpeditureAllMedia>().HasNoKey().ToView("ShareOfExpeditureAllMedia");
            modelBuilder.Entity<ShareOfVoiceAllMedia>().HasNoKey().ToView("ShareOfVoiceAllMedia");
            modelBuilder.Entity<Reconciliation>().HasNoKey().ToView("Reconciliation");
            modelBuilder.Entity<ReconciliationSummary>().HasNoKey().ToView("ReconciliationSummary");
            modelBuilder.Entity<TopCampaigns>().HasNoKey().ToView("TopCampaigns");

            modelBuilder.Entity<ShareOfCompetitiveWatchDogPress>().HasNoKey().ToView("ShareOfCompetitiveWatchDogPress");

            modelBuilder.Entity<ShareOfCompetitiveWatchDogOutdoor>().HasNoKey().ToView("ShareOfCompetitiveWatchDogOutdoor");

            modelBuilder.Entity<MediaSpendMix>().HasNoKey().ToView("MediaSpendMix");
            modelBuilder.Entity<DurationMix>().HasNoKey().ToView("DurationMix");
            modelBuilder.Entity<CummulativeSOVSOE>().HasNoKey().ToView("CummulativeSOVSOE");
        }
    }

    public class ApplicationUser : IdentityUser
    {
        public string CategoryId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string CompanyId { get; set; }
        public string Brand { get; set; }
        public string PicPath { get; set; }
        public string BillBoardPic { get; set; }
    }
}
