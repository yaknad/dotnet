using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public class CustomService : ICustomService
    {
        private ILogger<CustomService> logger;
        public CustomService(ILogger<CustomService> logger)
        {
            this.logger = logger;
        }

        public void DoNothing()
        {
            logger.LogInformation("Done nothing");
        }
    }
}
