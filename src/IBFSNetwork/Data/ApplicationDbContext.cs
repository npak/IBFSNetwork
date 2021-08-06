using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IBFSNetwork.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;

namespace IBFSNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.  
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ApplicationClient> AplicationClients { get; set; }
        public DbSet<Models.AlertViewModels.Alert> Alerts { get; set; }
        public DbSet<Models.AlertViewModels.FraudType> FraudTypes { get; set; }
        public DbSet<Models.AlertViewModels.Fraudster> Fraudsters { get; set; }
        public DbSet<Models.AlertViewModels.IDType> IDTypes { get; set; }
        public DbSet<Models.AlertViewModels.Document> Documents { get; set; }
        // public DbSet<Models.AlertViewModels.DocumentType> DocumentTypes { get; set; }
        public DbSet<Models.AlertViewModels.BankType> BankTypes { get; set; }
        public DbSet<Models.AlertViewModels.BankSize> BankSizes { get; set; }
        public DbSet<Models.AlertViewModels.Location> Locations { get; set; }
        public DbSet<Models.ForumViewModels.Question> Questions { get; set; }
        public DbSet<Models.ForumViewModels.Reply> Replies { get; set; }

        public DbSet<Models.FeedModels.Feed> Feeds { get; set; }
        public DbSet<Models.FeedModels.UserFeed> UserFeeds { get; set; }
        public DbSet<Models.FaqModels.FAQ> FAQ { get; set; }
        public DbSet<Models.AlertViewModels.FraudsterID> FraudsterIDs { get; set; }
        public DbSet<Models.AlertViewModels.LocationByCircuit> LocationByCircuits { get; set; }
        public DbSet<Models.AlertViewModels.LocationState> LocationStates { get; set; }
        public DbSet<Models.AlertViewModels.Country> Countries { get; set; }
        public DbSet<Models.ForumViewModels.RootForum> RootForums { get; set; }
        public DbSet<Models.ForumViewModels.Forum> Forums { get; set; }
        public DbSet<Models.ForumViewModels.SubThread> SubThreads { get; set; }
        public DbSet<Models.Questionnaire> Questionnaires { get; set; }
        public DbSet<Models.QuestionnaireAnswer> QuestionnaireAnswers { get; set; }
        public DbSet<Models.QuestionnaireModel> QuestionnaireModels { get; set; }
        public DbSet<Models.AlertViewModels.AlertFraudster> AlertFraudsters { get; set; }


    }


    public class ApplicationAdoContext : DbContext
    {
        private string _connectionString;

    public ApplicationAdoContext(DbContextOptions<ApplicationAdoContext> options) : base(options)  
    {
             _connectionString = ((SqlServerOptionsExtension)options.Extensions.Last()).ConnectionString;

        //    this._connectionString =
        //    //    "Server=.\\SQLEXPRESS;Database=newibfsdb;Trusted_Connection=True;MultipleActiveResultSets=true";
        //    "Server=sql5028.Smarterasp.net,1433;Database=DB_A06703_ibfs; user Id = ibfswebuser; Password=ghjktnfHBB_1712;Min Pool Size=0;Max Pool Size=200;Pooling=true;";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            // _connectionString = ((SqlServerOptionsExtension)optionsBuilder.Options.Extensions.First()).ConnectionString;
            //Console.WriteLine($"ApplicationContext{_connectionString}");
        }

        public DbSet<Models.ForumViewModels.SubThread> SubThreads { get; set; }
    }

}
