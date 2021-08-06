using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using IBFSNetwork.Data;

namespace IBFSNetwork.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20160805035338_AddedEmail")]
    partial class AddedEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Alert", b =>
                {
                    b.Property<int>("AlertId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AlertDate");

                    b.Property<string>("ApplicationUserId")
                        .HasAnnotation("MaxLength", 450);

                    b.Property<int?>("BankSizeId");

                    b.Property<int?>("BankTypeId");

                    b.Property<string>("City")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int?>("CountryId");

                    b.Property<int>("FraudTypeId");

                    b.Property<int?>("LocationByCircuitId");

                    b.Property<int>("LocationId");

                    b.Property<int?>("LocationStateId");

                    b.Property<int>("LostAmount");

                    b.Property<string>("Notes");

                    b.HasKey("AlertId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("BankSizeId");

                    b.HasIndex("BankTypeId");

                    b.HasIndex("CountryId");

                    b.HasIndex("FraudTypeId");

                    b.HasIndex("LocationByCircuitId");

                    b.HasIndex("LocationId");

                    b.HasIndex("LocationStateId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.BankSize", b =>
                {
                    b.Property<int>("BankSizeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("BankSizeId");

                    b.ToTable("BankSizes");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.BankType", b =>
                {
                    b.Property<int>("BankTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("BankTypeId");

                    b.ToTable("BankTypes");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("CountryId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlertId");

                    b.Property<byte[]>("Content");

                    b.Property<string>("Contentype")
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("DocName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("DocumentId");

                    b.HasIndex("AlertId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Fraudster", b =>
                {
                    b.Property<int>("FraudsterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .HasAnnotation("MaxLength", 512);

                    b.Property<int>("AlertId");

                    b.Property<DateTime>("BOD");

                    b.Property<string>("Company")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Gender")
                        .HasAnnotation("MaxLength", 10);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("MiddleName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PhoneNumber")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("isMain");

                    b.HasKey("FraudsterId");

                    b.HasIndex("AlertId");

                    b.ToTable("Fraudsters");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.FraudsterID", b =>
                {
                    b.Property<int>("PasportId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("DateOfIssue");

                    b.Property<DateTime?>("ExpirationDate");

                    b.Property<int>("FraudsterId");

                    b.Property<int>("IDTypeId");

                    b.Property<string>("IssuingAuthority")
                        .HasAnnotation("MaxLength", 128);

                    b.Property<string>("IssuingCountry")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("PasportId");

                    b.HasIndex("FraudsterId");

                    b.HasIndex("IDTypeId");

                    b.ToTable("FraudsterIDs");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.FraudType", b =>
                {
                    b.Property<int>("FraudTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("FraudTypeId");

                    b.ToTable("FraudTypes");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.IDType", b =>
                {
                    b.Property<int>("IDTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IDTypeName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("IDTypeId");

                    b.ToTable("IDTypes");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.LocationByCircuit", b =>
                {
                    b.Property<int>("LocationByCircuitId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("LocationByCircuitId");

                    b.ToTable("LocationByCircuits");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.LocationState", b =>
                {
                    b.Property<int>("LocationStateId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Caption")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 128);

                    b.HasKey("LocationStateId");

                    b.ToTable("LocationStates");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ApplicationClient", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientCode")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 10);

                    b.Property<int>("UserCount");

                    b.HasKey("ClientId");

                    b.ToTable("AplicationClients");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<int?>("ClientId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("IsSetUpEmailAlert");

                    b.Property<string>("LastName")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Location")
                        .HasAnnotation("MaxLength", 258);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<int?>("UserNumberCod");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("IBFSNetwork.Models.FaqModels.FAQ", b =>
                {
                    b.Property<int>("FAQId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Question")
                        .IsRequired();

                    b.Property<string>("Replay");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 450);

                    b.HasKey("FAQId");

                    b.HasIndex("UserId");

                    b.ToTable("FAQ");
                });

            modelBuilder.Entity("IBFSNetwork.Models.FeedModels.Feed", b =>
                {
                    b.Property<int>("FeedId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Desc")
                        .HasAnnotation("MaxLength", 255);

                    b.Property<int>("FeedType");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 255);

                    b.HasKey("FeedId");

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("IBFSNetwork.Models.FeedModels.UserFeed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FeedId");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 450);

                    b.HasKey("Id");

                    b.HasIndex("FeedId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFeeds");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.Forum", b =>
                {
                    b.Property<int>("ForumId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int?>("RootForumId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 511);

                    b.HasKey("ForumId");

                    b.HasIndex("RootForumId");

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<DateTime>("DatePosted");

                    b.Property<int?>("ForumId");

                    b.Property<int>("ReplyCount");

                    b.Property<int?>("SubThreadId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 511);

                    b.Property<int>("ViewCount");

                    b.HasKey("QuestionId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ForumId");

                    b.HasIndex("SubThreadId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.Reply", b =>
                {
                    b.Property<int>("ReplyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AppUserId");

                    b.Property<int?>("QuestionId");

                    b.Property<DateTime>("ReplyDate");

                    b.Property<string>("ReplyMsg")
                        .IsRequired();

                    b.HasKey("ReplyId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.RootForum", b =>
                {
                    b.Property<int>("RootForumId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 511);

                    b.HasKey("RootForumId");

                    b.ToTable("RootForums");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.SubThread", b =>
                {
                    b.Property<int>("SubThreadId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("ForumId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 511);

                    b.HasKey("SubThreadId");

                    b.HasIndex("ForumId");

                    b.ToTable("SubThreads");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Alert", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.BankSize", "BankSize")
                        .WithMany()
                        .HasForeignKey("BankSizeId");

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.BankType", "BankType")
                        .WithMany()
                        .HasForeignKey("BankTypeId");

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.FraudType", "FraudType")
                        .WithMany()
                        .HasForeignKey("FraudTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.LocationByCircuit", "LocationByCircuit")
                        .WithMany()
                        .HasForeignKey("LocationByCircuitId");

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.LocationState", "LocationState")
                        .WithMany()
                        .HasForeignKey("LocationStateId");
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Document", b =>
                {
                    b.HasOne("IBFSNetwork.Models.AlertViewModels.Alert", "Alert")
                        .WithMany("Documents")
                        .HasForeignKey("AlertId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.Fraudster", b =>
                {
                    b.HasOne("IBFSNetwork.Models.AlertViewModels.Alert", "Alert")
                        .WithMany("Fraudsters")
                        .HasForeignKey("AlertId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IBFSNetwork.Models.AlertViewModels.FraudsterID", b =>
                {
                    b.HasOne("IBFSNetwork.Models.AlertViewModels.Fraudster", "frauster")
                        .WithMany("FraudsterIDs")
                        .HasForeignKey("FraudsterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IBFSNetwork.Models.AlertViewModels.IDType", "IDType")
                        .WithMany()
                        .HasForeignKey("IDTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IBFSNetwork.Models.FaqModels.FAQ", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IBFSNetwork.Models.FeedModels.UserFeed", b =>
                {
                    b.HasOne("IBFSNetwork.Models.FeedModels.Feed", "Feed")
                        .WithMany()
                        .HasForeignKey("FeedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IBFSNetwork.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.Forum", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ForumViewModels.RootForum", "RootForum")
                        .WithMany()
                        .HasForeignKey("RootForumId");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.Question", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ApplicationUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("IBFSNetwork.Models.ForumViewModels.Forum", "Forum")
                        .WithMany()
                        .HasForeignKey("ForumId");

                    b.HasOne("IBFSNetwork.Models.ForumViewModels.SubThread", "SubThread")
                        .WithMany()
                        .HasForeignKey("SubThreadId");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.Reply", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ApplicationUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("IBFSNetwork.Models.ForumViewModels.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("IBFSNetwork.Models.ForumViewModels.SubThread", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ForumViewModels.Forum", "Forum")
                        .WithMany()
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("IBFSNetwork.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("IBFSNetwork.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
