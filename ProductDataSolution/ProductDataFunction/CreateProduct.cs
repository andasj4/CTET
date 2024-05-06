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
//using Microsoft.Azure.Cosmos.Table;
using Microsoft.WindowsAzure.Storage;

namespace ProductDataFunction
{
    public static class CreateProduct
    {
        [FunctionName("CreateProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true;DevelopmentStorageProxyUri=http://127.0.0.1:10002");

            //// Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            //// Get a reference to the table
            CloudTable table = tableClient.GetTableReference("Products");

            // Create the table if it doesn't exist
            table.CreateIfNotExists();


            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //var newProduct = JsonConvert.DeserializeObject<Product>(requestBody);


            string partitionKey = "partitionKey";
            string rowKey = "rowKey";

            var productEntity = new ProductEntity(partitionKey, rowKey)
            {
                ProductNo = "1234",
                ProductName = "Tangentbord",
                Description = "Att skriva med",
                Price = 149
            };
                 


            return new OkObjectResult(null);
        }
    }
}
