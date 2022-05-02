using System.Collections.Generic;

namespace Nop.Web.Models.PictureVault
{
    public class PictureVaultModel
    {
        public string PONumber { get; }
        public List<PictureVaultGroup> PictureVaultGroups { get; }

        public PictureVaultModel(string poNumber, List<PictureVaultGroup> pictureVaultGroups)
        {
            PONumber = poNumber;
            PictureVaultGroups = pictureVaultGroups;
        }
    }

    public class PictureVaultGroup
    {
        public string PictureVaultGroupTitle { get; }
        public List<PictureVaultImage> PictureVaultImages { get; }

        public PictureVaultGroup(string pictureVaultGroupTitle, List<PictureVaultImage> pictureVaultImages)
        {
            PictureVaultGroupTitle = pictureVaultGroupTitle;
            PictureVaultImages = pictureVaultImages;
        }
    }

    public class PictureVaultImage
    {
        public string Title { get; }
        public string ImgSrc { get; }

        public PictureVaultImage(string title, string imgSrc)
        {
            Title = title;
            ImgSrc = imgSrc;
        }
    }
}