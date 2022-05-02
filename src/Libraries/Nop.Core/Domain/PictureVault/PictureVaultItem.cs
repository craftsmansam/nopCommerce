using System;

namespace Nop.Core.Domain.PictureVault
{
    public class PictureVaultItem
    {
        public string Filename { get; set; }
        public string UploadedByName { get; set; }
        public DateTime DateTimeUploaded { get; set; }
        public int ShopOrderNumber { get; set; }
        public string EstimatorEmail { get; set; }
        public string ResponsibleInternalEmployeeEmail { get; set; }
        public string ProcessName { get; set; }
        public int PictureVaultProcessID { get; set; }
        public string Notes { get; set; }
        public int ShopOrderPictureID { get; set; }
        public int CustomerID { get; set; }
        public string PurchaseOrder { get; set; }
        public string ProjectName { get; set; }
        public bool Internal { get; set; }
    }
}