using Library.Data;
using Library.Data.Users;
using Library.Mapping;
using Library.Mapping.Users;
using Library.Models.Users;
using Library.Services.Database;
using Library.Utils;

namespace AculocityPracticalInterview
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // NOTES:
            // Mostly singletons used to keep one connection open to the database as the system could be flooded otherwise.
            // Use of mutex lock is important for the single threaded checking of connections. 

            // Depencancy injection
            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc().AddControllersAsServices();

            //Custom logger
            ILoggerFactory sawMill = new LoggerFactory();
            ILogger logger = sawMill.CreateLogger("MainLogger");

            //Custom database in the place of entity framework
            #region Database
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            var dbContext = new SQLServer(connectionString, logger);
            builder.Services.AddSingleton<IDatabase>(dbContext);
            using (DatabaseGenerator dg = new DatabaseGenerator(dbContext))
            {
                dg.GenerateTables(new List<TableConstructObject>
                {
                    new TableConstructObject(new UsersDto(), "Users")
                });
            }
            #endregion

            //custom mappers
            #region Mappers
            builder.Services.AddSingleton<IMapper<UsersDto, UsersModel>>(new UsersMapper());
            #endregion

            //custom data repository
            #region DataRepositories
            builder.Services.AddSingleton<IDataRepository<UsersDto>>(new UsersDataRepository(dbContext));
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}