using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiRoleAuthentication.Areas.Identity.Data;

[assembly: HostingStartup(typeof(MultiRoleAuthentication.Areas.Identity.IdentityHostingStartup))]
namespace MultiRoleAuthentication.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DBContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DBContextConnection")));

                services.AddIdentity<MultiRoleAuthUser, IdentityRole>(options => {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<DBContext>();
            });
        }
    }
}