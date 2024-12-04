using Hello.Api.Configuration;
using Hello.Api.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hello.Api.Services
{
    public class ProductGenerationService : BackgroundService
    {
        private readonly ILogger<ProductGenerationService> _logger;
        private readonly BackgroundServiceSettings _settings;
        private readonly List<Product> _products;
        private static readonly Random _random = new();

        public ProductGenerationService(
            ILogger<ProductGenerationService> logger,
            IOptions<BackgroundServiceSettings> settings,
            List<Product> products) // Inject shared list
        {
            _logger = logger;
            _settings = settings.Value;
            _products = products;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Product Generation Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                GenerateProduct();
                await Task.Delay(TimeSpan.FromSeconds(_settings.DataGenerationInterval), stoppingToken);
            }

            _logger.LogInformation("Product Generation Service is stopping.");
        }

        private void GenerateProduct()
        {
            var newProduct = new Product
            {
                Id = _products.Count + 1,
                Name = $"Product {_products.Count + 1}",
                Price = (decimal)(_random.NextDouble() * 100)
            };

            _products.Add(newProduct);
            _logger.LogInformation($"Generated Product: {newProduct.Name}, Price: {newProduct.Price}");
        }
    }
}
