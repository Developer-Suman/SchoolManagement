using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Application.Shared.NotificationDTOs;
using TN.Shared.Infrastructure.ActivityProcessServices;
using TN.Shared.Infrastructure.Data;
using TN.Shared.Infrastructure.Repository.BackGroundServices;

namespace TN.Shared.Infrastructure.Repository.BackGroundServicesHub
{
    public class StockExpiryAlertBackGroundServices : BackgroundService
    {
        private readonly IActivityChannel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StockExpiryAlertBackGroundServices> _logger;
        private readonly IHttpContextAccessor _httpContext;


        public StockExpiryAlertBackGroundServices(IActivityChannel activityChannel, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, ILogger<StockExpiryAlertBackGroundServices> logger)
        {
            _channel = activityChannel;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpContext = httpContextAccessor;

        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Checking stock expiry at: {time}", DateTime.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                        var activityChannel = scope.ServiceProvider.GetRequiredService<IStockAlertActivities>();
                        var stockActivityProcess = scope.ServiceProvider.GetRequiredService<StockAlertActivityProcessServices>();


                        var today = DateTime.Now.Date;
                        var next7Days = today.AddDays(7);

                        var expiringSoonItems = await dbContext.Items
                            .Where(i => i.ManufacturingAndExpiries
                                .Any(m => m.ExpiredDate.HasValue &&
                                          m.ExpiredDate.Value.Date >= today &&
                                          m.ExpiredDate.Value.Date <= next7Days))
                            .Select(i => new
                            {
                                Item = i,
                                ExpiringManufacturingAndExpiries = i.ManufacturingAndExpiries
                                    .Where(m => m.ExpiredDate.HasValue &&
                                                m.ExpiredDate.Value.Date >= today &&
                                                m.ExpiredDate.Value.Date <= next7Days)
                                    .ToList()
                            })
                            .AsNoTracking()
                            .ToListAsync(stoppingToken);

                        foreach (var item in expiringSoonItems)
                        {


                            //Add Activity to Channel
                            await _channel.AddActivityAsync(new StockExpiryDTOs(
                             item.ExpiringManufacturingAndExpiries.Select(x => x.ExpiredDate).FirstOrDefault(),
                             item.Item.Name,
                             item.Item.SchoolId,
                             item.Item.StockCenterId
                             ));

                        }

                    }




                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking stock expiry");
                    Console.WriteLine($"Error checking stock expiry: {ex.Message}");
                }
                _logger.LogInformation("Next check scheduled at: {time}", DateTime.Now.AddMinutes(2));
                 //Wait for 2 minutes before checking again
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }

        }
    }
}
