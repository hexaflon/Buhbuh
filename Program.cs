using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjektInzynierski.Utils;
using TestTest.Models.Db;

namespace TestTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var AppName = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            builder.Services.AddRazorPages();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<IdentityDatabaseContext>(options =>
            options.UseSqlServer(connectionString));

            builder.Services.AddDefaultIdentity<Person>(options =>
            options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDatabaseContext>();
            builder.Services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(AppName.GetSection("ConnectionStrings")["DB_Login"]));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }



            app.UseStaticFiles();

            app.UseRouting();


            app.MapRazorPages();
            app.MapFallbackToPage("/Index");

            app.UseAuthentication();
            app.UseAuthorization();

            //u¿ycie Observera

            //Przyk³ad 1 ad2
            //BaseLogger logger = new BufferedLogger(new ColoredLogStrategy());
            //Przyk³ad 2 ad2 
            /*
            ILogStrategy strategy= new ColoredLogStrategy();
            BaseLogger logger = Logger.getInstance();
            logger.SetStrategy(strategy);
            */
            //Przyk³ad 3 ad2
            BaseLogger logger = Logger.getInstance();
            ILogObserver observer = new ConsoleLogObserver();
            

            logger.Attach(new ConsoleLogObserver());
            logger.Attach(new FileLogObserver());

            logger.Log("START.");

            app.Run();
        }
    }
}

