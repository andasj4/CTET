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

namespace ProductDataFunction
{
    public static class CreateProduct
    {
        [FunctionName("CreateProduct")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            var configuration = builder.Build();
                        
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var incomingProduct = JsonConvert.DeserializeObject<Product>(requestBody);
            var storageAccount = CloudStorageAccount.Parse(configuration["StorageConnectionString"]);
            
            InsertEntity(storageAccount, incomingProduct, log);

            return new OkObjectResult(incomingProduct);
        }

        private static async void InsertEntity(CloudStorageAccount storageAccount, Product product, ILogger log)
        {
            try
            {
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("Products");
                table.CreateIfNotExists();

                string partitionKey = "Product";
                string rowKey = product.ProductNo;

                var productEntity = new ProductEntity(partitionKey, rowKey)
                {
                    ProductNo = product.ProductNo,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Price = product.Price,                    
                    LastUpdated = product.ModifiedDate
                };

                var tableOperation = TableOperation.InsertOrMerge(productEntity);
                await table.ExecuteAsync(tableOperation);
            }
            catch (Exception ex)
            {
                log.LogError($"Insert failure: {ex.InnerException}");
            }            
        }
    }    
}
