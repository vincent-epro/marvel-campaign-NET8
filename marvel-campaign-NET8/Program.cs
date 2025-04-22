
using marvel_campaign_NET8.Models;
using Microsoft.EntityFrameworkCore;

namespace marvel_campaign_NET8
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Disable camelCase
            });

            // Add DbContext using environment variable
            var ConnectionString_campaignCRM = Environment.GetEnvironmentVariable("ConnectionString_campaignCRM");
            if (string.IsNullOrEmpty(ConnectionString_campaignCRM))
            {
                throw new InvalidOperationException("The 'ConnectionString_XXXXXX' environment variable is not set.");
            }


            // Add DbContext
            builder.Services.AddDbContext<ScrmDbContext>(options =>
                options.UseSqlServer(ConnectionString_campaignCRM));

            builder.Services.AddDbContext<Scrm_SP_DbContext>(options =>
                options.UseSqlServer(ConnectionString_campaignCRM));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //   UseSwagger

            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
