using Microsoft.Azure.Cosmos.Table;
using System;

public class ProductEntity : TableEntity
{
    public ProductEntity(string partitionKey, string rowKey)
    {
        PartitionKey = partitionKey;
        RowKey = rowKey;
    }

    public ProductEntity() { }

    public string ProductNo { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }    
    public DateTime LastUpdated { get; set; }
}