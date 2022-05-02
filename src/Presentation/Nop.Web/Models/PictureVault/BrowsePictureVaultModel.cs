using System;
using System.Collections.Generic;

namespace Nop.Web.Models.PictureVault
{
    public class BrowsePictureVaultModel
    {
        public List<Tuple<string, DateTime, string>> PurchaseOrderList;
        public string PictureVaultUrlTemplate = "/secure/picture-vault?po={0}";
    }
}