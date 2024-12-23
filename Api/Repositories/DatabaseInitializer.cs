using Microsoft.AspNetCore.Identity;
using PromerceCRM.API.Identity;

namespace PromerceCRM.API.Repository
{
    public class DatabaseInitializer
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseInitializer(UserManager<UserModel> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public ApplicationDbContext _dbContext { get; }

        public async Task InitializeAsync()
        {
            // Ensure admin role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Check if the admin user exists
            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new UserModel
                {
                    UserName = "admin",
                    Email = "admin@user.com",
                    EmailConfirmed = true,
                    TenantCode = "ADMIN"
                };
                await _userManager.CreateAsync(adminUser, "Admin123!");  // Set a secure password
            }

            // Assign the user to the role if not already
            if (!await _userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
            //
            await SeedProjects();
            //
            if (!_dbContext.SystemModules.Any())
            {
                var project = _dbContext.Projects.FirstOrDefault(v => v.Name == "PROMERCE ERP DESKTOP").Id;
                if (project != null)
                {
                    await SeedSystemModulesFull(project); //DeskTop                                                          
                    await SeedSystemModulesSettings(project);//settings
                }
                project = _dbContext.Projects.FirstOrDefault(v => v.Name == "PROMERCE MOB APP").Id;
                if (project != null)
                {
                    await SeedSystemModulesFull(project); //MobileApp
                    await SeedSystemModulesSettings(project);//settings
                }

                project = _dbContext.Projects.FirstOrDefault(v => v.Name == "PROMERCE WEB APP").Id;
                if (project != null)
                {
                    await SeedSystemModulesWeb(); //Web
                    await SeedSystemModulesSettings(project);//settings
                }

                project = _dbContext.Projects.FirstOrDefault(v => v.Name == "NOP E-COMMERCE").Id;
                if (project != null)
                {
                    await SeedSystemModulesECommerce(); //Nop Commerce
                    await SeedSystemModulesSettings(project);//settings
                }

            }
        }

        private async Task SeedProjects()
        {
            if (!_dbContext.Projects.Any())
            {
                await _dbContext.Projects.AddAsync(new Models.CRM.Project
                {
                    Name = "PROMERCE ERP DESKTOP",
                    IsActive = true,
                });
                await _dbContext.Projects.AddAsync(new Models.CRM.Project
                {
                    Name = "PROMERCE MOB APP",
                    IsActive = true,
                });
                await _dbContext.Projects.AddAsync(new Models.CRM.Project
                {
                    Name = "PROMERCE WEB APP",
                    IsActive = true,
                });
                await _dbContext.Projects.AddAsync(new Models.CRM.Project
                {
                    Name = "NOP E-COMMERCE",
                    IsActive = true,
                });
            }
            _dbContext.SaveChanges();
        }
        private async Task SeedSystemModulesFull(int projectId)
        {
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Sales",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Purchase",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Warehouses",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Finance",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "HR",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });

            _dbContext.SaveChanges();
            //under Sales
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Sales Invoice",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Sales" && v.ProjectId == projectId).Id,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Sales Return",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Sales" && v.ProjectId == projectId).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();
            //under purchase
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Purchase Invoice",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Purchase" && v.ProjectId == projectId).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Purchase Return",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Purchase" && v.ProjectId == projectId).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();
            //under Warehouses
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Transfer",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Warehouses" && v.ProjectId == projectId).Id,
                Priority = 2,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Orders",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Warehouses" && v.ProjectId == projectId).Id,
                Priority = 2,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();
            //under Finance
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Journals",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Finance" && v.ProjectId == projectId).Id,
                Priority = 2,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Parties",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Finance" && v.ProjectId == projectId).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Reports",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Finance" && v.ProjectId == projectId).Id,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();
            //under HR
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Employees",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "HR" && v.ProjectId == projectId).Id,
                Priority = 2,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Payrolls",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "HR" && v.ProjectId == projectId).Id,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Attendance",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "HR" && v.ProjectId == projectId).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();
        }
        private async Task SeedSystemModulesECommerce()
        {
            var project_id = _dbContext.Projects.FirstOrDefault(v => v.Name == "NOP E-COMMERCE").Id;
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Main",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = _dbContext.Projects.FirstOrDefault(v => v.Name == "NOP E-COMMERCE").Id
            });
            _dbContext.SaveChanges();

            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Payment",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Main" && v.ProjectId == project_id).Id,
                Priority = 2,
                IsActive = true,
                ProjectId = project_id
            });
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Sales Order",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Main" && v.ProjectId == project_id).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = project_id
            });
            _dbContext.SaveChanges();
        }
        private async Task SeedSystemModulesWeb()
        {
            var project_id = _dbContext.Projects.FirstOrDefault(v => v.Name == "PROMERCE WEB APP").Id;
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Sales",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = project_id
            });
            _dbContext.SaveChanges();

            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "POS",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Sales" && v.ProjectId == project_id).Id,
                Priority = 2,
                IsActive = true,
                ProjectId = project_id
            });
            _dbContext.SaveChanges();
        }

        private async Task SeedSystemModulesSettings(int projectId)
        {
            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "Settings",
                ParentId = 0,
                Priority = 0,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();

            await _dbContext.SystemModules.AddAsync(new Models.CRM.SystemModule
            {
                Name = "General",
                ParentId = _dbContext.SystemModules.FirstOrDefault(v => v.Name == "Settings" && v.ProjectId == projectId).Id,
                Priority = 1,
                IsActive = true,
                ProjectId = projectId
            });
            _dbContext.SaveChanges();
        }
    }

}
