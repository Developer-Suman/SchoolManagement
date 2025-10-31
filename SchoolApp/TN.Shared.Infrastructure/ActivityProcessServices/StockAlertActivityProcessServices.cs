using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.ServiceInterface.IBackGroundService;
using TN.Shared.Domain.IActivityProcessServices;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.ActivityProcessServices
{
    public class StockAlertActivityProcessServices : IStockAlertActivityProcessServices
    {

        private readonly ILogger<StockAlertActivityProcessServices> _logger;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IActivityChannel _channel;


        public StockAlertActivityProcessServices(IActivityChannel channel, ILogger<StockAlertActivityProcessServices> logger, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
 
            _logger = logger;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _channel = channel;
        }


        public async Task ProcessStockExpiryAsync(CancellationToken stoppingToken)
        {

            var schoolId = _tokenService.SchoolId().FirstOrDefault();

            while (_channel.TryRead(out var activity))
            {
                try
                {
                    if (activity.schoolId != schoolId)
                        continue;

  
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing stock expiry for item {ItemName}", activity.itemName);
                }



            }
        }
    }
}
