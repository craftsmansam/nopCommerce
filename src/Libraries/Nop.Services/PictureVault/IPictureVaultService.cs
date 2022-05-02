using System;
using System.Collections.Generic;
using Nop.Core.Domain.PictureVault;

namespace Nop.Services.PictureVault
{
    /// <summary>
    /// Tangent material service interface
    /// </summary>
    public interface IPictureVaultService
    {
        List<Tuple<string, DateTime, string>> CustomerListPurchaseOrders(int? salesContactId);

        IList<PictureVaultItem> CustomerListPictureVaultItems(int? salesContactId, string po);
        bool IsPictureForMyCompany(int customerSalesContactId, int imageId);
        string PictureVaultFileNameByShopOrderPictureId(int imageId);
        int PictureVaultSONumberBySOPictureId(int imageId);
    }
}