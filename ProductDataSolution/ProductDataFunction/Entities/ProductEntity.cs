using Microsoft.WindowsAzure.Storage.Table;
using System;

public class ProductEntity : TableEntity
{
    private static int idCounter = 0; // Static counter for generating IDs
    public int Id { get; private set; } // Auto-incrementing ID property
    public string ProductNo { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public DateTime CreatedAt { get; private set; }

    public ProductEntity(string partitionKey, string rowKey)
    {
        this.PartitionKey = partitionKey;
        this.RowKey = rowKey;
        this.Id = ++idCounter; // Increment the counter and assign as ID
        this.CreatedAt = DateTime.Now;
    }

    public ProductEntity()
    {
        this.Id = ++idCounter; // Increment the counter and assign as ID
        this.CreatedAt = DateTime.Now;
    }
}