﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISyncService
    {
        Task SyncClientsAsync();
        Task SyncProductsAsync();
        Task SyncUsersAsync();
    }
}
