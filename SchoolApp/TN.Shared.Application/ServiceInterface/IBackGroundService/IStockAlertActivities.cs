﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Application.Shared.NotificationDTOs;

namespace TN.Shared.Application.ServiceInterface.IBackGroundService
{
    public interface IStockAlertActivities
    {
        ValueTask AddActivityAsync(StockExpiryDTOs stockExpiryDTOs, CancellationToken cancellationToken = default);
        ValueTask<StockExpiryDTOs?> ReadSingleAsync(CancellationToken cancellationToken = default);
        IAsyncEnumerable<StockExpiryDTOs> ReadAllAsync(CancellationToken cancellationToken);
    }
}
