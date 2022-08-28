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

    public class VideoPlayerModel
    {
        public string VideoUrl { get; }
        public int Width { get; }
        public int Height { get; }

        public VideoPlayerModel(string videoUrl, int width, int height)
        {
            VideoUrl = videoUrl;
            Width = width;
            Height = height;
        }
    }

    public class PictureVaultImage
    {
        public string Title { get; }
        public string ImgSrc { get; }
        public string VideoUrl { get; }
        public int? VideoHeight { get; }
        public int? VideoWidth { get; }

        public PictureVaultImage(string title, string imgSrc, string videoUrl, int? videoWidth, int? videoHeight)
        {
            Title = title;
            ImgSrc = imgSrc;
            VideoUrl = videoUrl;
            VideoHeight = videoHeight;
            VideoWidth = videoWidth;
        }
    }
}