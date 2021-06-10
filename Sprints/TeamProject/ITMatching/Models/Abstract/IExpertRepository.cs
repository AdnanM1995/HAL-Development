﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITMatching.Models.Abstract
{
    public interface IExpertRepository : IRepository<Expert>
    {
        Task<Expert> GetByItmUserIdAsync(int itmUserId);
        Task SetStatusAsync(int expertId, bool isAvailable);
        Task ToggleStatusAsync(int expertId);
        Task DeleteServicesAsync(int expertId);
    }
}
