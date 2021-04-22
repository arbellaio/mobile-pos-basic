using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RecompildPOS.Models.Audit;
using SQLite;

namespace RecompildPOS.Models.Products
{
    public class ProductSync : AuditEntity
    {
        [PrimaryKey]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string Picture { get; set; }
        public string BarCode1 { get; set; }
        public string BarCode2 { get; set; }
        public string QrCode { get; set; }
        public string SkuCodeImagePath { get; set; }
        public string QrCodeImagePath { get; set; }
        public decimal SellPrice { get; set; }
        public decimal? BuyPrice { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Tax { get; set; }
        public string ProductNotes { get; set; }
        public bool IsDeleted { get; set; }
        public int  BusinessId { get; set; }

    }

    public class Product : AuditDb
    {
        [PrimaryKey, AutoIncrement]
        public int LocalProductId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SkuCode { get; set; }
        public string BarCode1 { get; set; }
        public string BarCode2 { get; set; }
        public string QrCode { get; set; }
        public decimal SellPrice { get; set; }
        public decimal? BuyPrice { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Tax { get; set; }
        public string ProductNotes { get; set; }
        public bool IsDeleted { get; set; }
        public int BusinessId { get; set; }

    }
}
