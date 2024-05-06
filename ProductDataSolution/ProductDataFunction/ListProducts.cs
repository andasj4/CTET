using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductDataFunction.Models;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ProductDataFunction
{
    public static class ListProducts
    {
        [FunctionName("ListProducts")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            var builder = new ConfigurationBuilder()
                       .SetBasePath(Environment.CurrentDirectory)
                       .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables();
            var configuration = builder.Build();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var incomingProduct = JsonConvert.DeserializeObject<Product>(requestBody);
            var storageAccount = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);

            var products = GetProducts(storageAccount, log);

            return new OkObjectResult(products);
        }

        private static ProductModel GetProducts(CloudStorageAccount storageAccount, ILogger log)
        {
            ProductModel products = new ProductModel { Products = new List<Product>() };

            try
            {
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("Products");

                var query = new TableQuery<ProductEntity>();
                var entities = table.ExecuteQuery(query);
                
                foreach(var entity in entities)
                {
                    Product product = new Product
                    {
                        ProductNo = entity.ProductNo,
                        ProductName = entity.ProductName,
                        Price = entity.Price,
                        Description = entity.Description,
                        ModifiedDate = entity.LastUpdated
                    };

                    products.Products.Add(product);
                }

                return products;
            }
            catch (Exception ex)
            {
                log.LogError($"Failed to list products: {ex.InnerException}");
                return null;
            }
        }
    }
}
