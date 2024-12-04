using Hello.Api.Configuration;
using Hello.Api.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hello.Api.Services
{
    public class ProductRemovalService : BackgroundService
    {
        private readonly ILogger<ProductRemovalService> _logger;
        private readonly BackgroundServiceSettings _settings;
        private readonly List<Product> _products;

        public ProductRemovalService(
            ILogger<ProductRemovalService> logger,
            IOptions<BackgroundServiceSettings> settings,
            List<Product> products) // Inject shared list
        {
            _logger = logger;
            _settings = settings.Value;
            _products = products;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Product Removal Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                RemoveOldProduct();
                await Task.Delay(TimeSpan.FromSeconds(_settings.DataRemovalInterval), stoppingToken);
            }

            _logger.LogInformation("Product Removal Service is stopping.");
        }

        private void RemoveOldProduct()
        {
            if (_products.Any())
            {
                var productToRemove = _products.First();
                _products.Remove(productToRemove);
                _logger.LogInformation($"Removed Product: {productToRemove.Name}");
            }
        }
    }
}
