using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PromerceCRM.API.Identity;
using PromerceCRM.API.Models.CRM;
using PromerceCRM.API.Models.ERP;

namespace PromerceCRM.API.Repository
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<SystemModule> SystemModules { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Tenant>().HasData(new Tenant { Id = 1, TenantCode = "ADMIN", TenantName = "ADMIN" });
        }
    }
}
