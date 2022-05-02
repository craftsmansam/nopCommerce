using System.Collections.Generic;
using System.Text;
using CraftsmanMvc.Models;
using Microsoft.AspNetCore.Html;

namespace CraftsmanMvc.Controls
{
    public class CarouselNetCore
    {
        private readonly List<DescriptorHrefImgAltTitleText> _images;
        private readonly string _idPrefix;
        private readonly string _imagePath;
        private readonly string _controlID;

        public static HtmlString Render(string controlID, List<DescriptorHrefImgAltTitleText> images, string idPrefix, string imagePath)
        {
            var control = new CarouselNetCore(controlID, images, idPrefix, imagePath);
            return control.Render();
        }
        public static HtmlString RenderOriginal(string controlID, List<DescriptorHrefImgAltTitleText> images, string idPrefix, string imagePath)
        {
            var control = new CarouselNetCore(controlID, images, idPrefix, imagePath);
            return control.RenderOriginal();
        }

        public static HtmlString RenderScript(string idPrefix)
        {
            var control = new CarouselNetCore(null, null, idPrefix, null);
            return control.RenderScript();
        }

        private CarouselNetCore(string controlID, List<DescriptorHrefImgAltTitleText> images, string idPrefix, string imagePath)
        {
            _images = images;
            _idPrefix = idPrefix;
            _imagePath = imagePath;
            _controlID = controlID;
        }

        private HtmlString Render()
        {
            var carIdx = 0;
            var modIdx = 0;
            var output = new StringBuilder($@"
                <div class=""carousel-center"">
                <div class=""owl-carousel"" id=""{_idPrefix}_carousel"">");

            foreach (var image in _images)
            {
                output.Append($@"<div data-toggle=""modal"" data-target=""#{GetModalImageDivId(carIdx)}"" style=""cursor: pointer;"">
                    <img class=""lazyload lazyOwl"" data-src=""{_imagePath}{image.ImgSrc}"" alt=""{image.AltTitle}"" title=""{image.AltTitle}"" /><br />
                    <p class=""carousel-caption"">{image.Text}</p>
                    </div>");
                carIdx++;
            }

            output.Append(@"</div>
                </div>");

            foreach (var image in _images)
            {
                output.Append($@"
                <div class=""modal fade"" id=""{GetModalImageDivId(modIdx)}"" tabindex=""-1"" role=""dialog"">
                    <div class=""vertical-alignment-helper"">
                    <div class=""modal-dialog carousel-modal vertical-align-center"" role=""document"">
                    <div class=""modal-content"">
                    <button type = ""button"" class=""close modal-button"" data-dismiss=""modal"" aria-label=""Close""><img class=""modal-button"" src=""/images/close.png"" alt=""close button"" /></button>
                    <div class=""modal-body"">
                    <img class=""carousel-modal-image img-responsive"" data-src=""{_imagePath}{image.ImgSrc}"" alt=""{image.AltTitle}"" title=""{image.AltTitle}""/><br/>
                    <p class=""modal-caption"">{image.Text}</p>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>");
                modIdx++;
            }

            return new HtmlString(output.ToString());
        }
        private HtmlString RenderScript()
        {
            var script = new StringBuilder("");
            script.Append($@"$(document).on(""albinaLoaded"", function() {{
                $(""#{_idPrefix}_carousel"").owlCarousel({{
                    navigation: true,
                    lazyLoad: false,
                    items: 3,
                    itemsDesktop: false,
                    loop: true
                }});

                $('#{_idPrefix}_carousel').find('img.lazyOwl').on(""load"", loadCarouselModalSource);
            }});

            
            ");

            return new HtmlString(script.ToString());
        }

        private HtmlString RenderOriginal()
        {
            var carIdx = 0;
            var modIdx = 0;
            var output = new StringBuilder($@"
                <div class=""carousel-center"">
                <div class=""owl-carousel"" id=""{_idPrefix}_carousel"">");

            foreach (var image in _images)
            {
                output.Append($@"<div data-toggle=""modal"" data-target=""#{GetModalImageDivId(carIdx)}"" style=""cursor: pointer;"">
                    <img class=""lazyload lazyOwl"" data-src=""{_imagePath}{image.ImgSrc}"" alt=""{image.AltTitle}"" title=""{image.AltTitle}"" /><br />
                    <p class=""carousel-caption"">{image.Text}</p>
                    </div>");
                carIdx++;
            }

            output.Append(@"</div>
                </div>");

            foreach (var image in _images)
            {
                output.Append($@"
                <div class=""modal fade"" id=""{GetModalImageDivId(modIdx)}"" tabindex=""-1"" role=""dialog"">
                    <div class=""vertical-alignment-helper"">
                    <div class=""modal-dialog carousel-modal vertical-align-center"" role=""document"">
                    <div class=""modal-content"">
                    <button type = ""button"" class=""close modal-button"" data-dismiss=""modal"" aria-label=""Close""><img class=""modal-button"" src=""/images/close.png"" alt=""close button"" /></button>
                    <div class=""modal-body"">
                    <img class=""carousel-modal-image img-responsive"" data-src=""{_imagePath}{image.ImgSrc}"" alt=""{image.AltTitle}"" title=""{image.AltTitle}""/><br/>
                    <p class=""modal-caption"">{image.Text}</p>
                    </div>
                    </div>
                    </div>
                    </div>
                    </div>");
                modIdx++;
            }

            output.Append($@"<script type=""text/javascript"">
                           jQuery(document).on(""albinaLoaded"", function() {{
                $(""#{_idPrefix}_carousel"").owlCarousel({{
                    navigation: true,
                    lazyLoad: false,
                    items: 3,
                    itemsDesktop: false,
                    loop: true
                }});

                $('#{_idPrefix}_carousel').find('img.lazyOwl').on(""load"", loadCarouselModalSource);
            }});

            
            </script>");

            return new HtmlString(output.ToString());
        }

        protected string GetModalImageDivId(int index)
        {
            return $"image_{_controlID}_{index}";
        }
    }
}
