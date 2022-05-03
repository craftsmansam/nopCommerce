using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core.Domain.PictureVault;

namespace Nop.Services.PictureVault
{
    /// <summary>
    /// Tangent material service interface
    /// </summary>
    public interface IPictureVaultService
    {
        Task<List<Tuple<string, DateTime, string>>> CustomerListPurchaseOrdersAsync(int? salesContactId);

        Task<IList<PictureVaultItem>> CustomerListPictureVaultItemsAsync(int? salesContactId, string po);
        Task<bool> IsPictureForMyCompanyAsync(int customerSalesContactId, int imageId);
        Task<string> PictureVaultFileNameByShopOrderPictureIdAsync(int imageId);
        Task<int> PictureVaultSONumberBySOPictureIdAsync(int imageId);
    }
}