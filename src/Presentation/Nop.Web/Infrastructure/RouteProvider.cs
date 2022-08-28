﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Domain.Localization;
using Nop.Data;
using Nop.Services.Installation;
using Nop.Services.Localization;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Web.Infrastructure
{
    /// <summary>
    /// Represents provider that provided basic routes
    /// </summary>
    public partial class RouteProvider : BaseRouteProvider, IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            //get language pattern
            //it's not needed to use language pattern in AJAX requests and for actions returning the result directly (e.g. file to download),
            //use it only for URLs of pages that the user can go to
            var pattern = GetLanguageRoutePattern();

            //areas
            endpointRouteBuilder.MapControllerRoute(name: "areaRoute",
                pattern: $"{{area:exists}}/{{controller=Home}}/{{action=Index}}/{{id?}}");

            //home page
            endpointRouteBuilder.MapControllerRoute(name: "Homepage", 
                pattern: $"{pattern}",
                defaults: new { controller = "Home", action = "Index" });

            //login
            endpointRouteBuilder.MapControllerRoute(name: "Login",
                pattern: $"{pattern}/login/",
                defaults: new { controller = "Customer", action = "Login" });

            // multi-factor verification digit code page
            endpointRouteBuilder.MapControllerRoute(name: "MultiFactorVerification",
                pattern: $"{pattern}/multi-factor-verification/",
                defaults: new { controller = "Customer", action = "MultiFactorVerification" });

            //register
            endpointRouteBuilder.MapControllerRoute(name: "Register",
                pattern: $"{pattern}/register/",
                defaults: new { controller = "Customer", action = "Register" });

            //logout
            endpointRouteBuilder.MapControllerRoute(name: "Logout",
                pattern: $"{pattern}/logout/",
                defaults: new { controller = "Customer", action = "Logout" });

            //shopping cart
            endpointRouteBuilder.MapControllerRoute(name: "ShoppingCart",
                pattern: $"{pattern}/cart/",
                defaults: new { controller = "ShoppingCart", action = "Cart" });

            //estimate shipping (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "EstimateShipping", 
                pattern: $"cart/estimateshipping",
                defaults: new { controller = "ShoppingCart", action = "GetEstimateShipping" });

            //wishlist
            endpointRouteBuilder.MapControllerRoute(name: "Wishlist",
                pattern: $"{pattern}/wishlist/{{customerGuid?}}",
                defaults: new { controller = "ShoppingCart", action = "Wishlist" });

            //customer account links
            endpointRouteBuilder.MapControllerRoute("CustomerInfo", $"{pattern}customer/info",
                new { controller = "Customer", action = "Info" });

            endpointRouteBuilder.MapControllerRoute("CustomerAddresses", $"{pattern}customer/addresses",
                new { controller = "Customer", action = "Addresses" });

            endpointRouteBuilder.MapControllerRoute("CustomerOrders", $"{pattern}order/history",
                new { controller = "Order", action = "CustomerOrders" });

            endpointRouteBuilder.MapControllerRoute("CustomerQuotes", $"{pattern}quote/history",
                new { controller = "Quote", action = "CustomerQuotes" });

            //contact us

            endpointRouteBuilder.MapControllerRoute("ContactUs", $"{pattern}about/contact",
                new { controller = "Common", action = "ContactUs" });


            //product search
            endpointRouteBuilder.MapControllerRoute(name: "ProductSearch",
                pattern: $"{pattern}/search/",
                defaults: new { controller = "Catalog", action = "Search" });

            //autocomplete search term (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "ProductSearchAutoComplete", 
                pattern: $"catalog/searchtermautocomplete",
                defaults: new { controller = "Catalog", action = "SearchTermAutoComplete" });

            //change currency
            endpointRouteBuilder.MapControllerRoute(name: "ChangeCurrency",
                pattern: $"{pattern}/changecurrency/{{customercurrency:min(0)}}",
                defaults: new { controller = "Common", action = "SetCurrency" });

            //change language
            endpointRouteBuilder.MapControllerRoute(name: "ChangeLanguage",
                pattern: $"{pattern}/changelanguage/{{langid:min(0)}}",
                defaults: new { controller = "Common", action = "SetLanguage" });

            //change tax
            endpointRouteBuilder.MapControllerRoute(name: "ChangeTaxType",
                pattern: $"{pattern}/changetaxtype/{{customertaxtype:min(0)}}",
                defaults: new { controller = "Common", action = "SetTaxType" });

            //recently viewed products
            endpointRouteBuilder.MapControllerRoute(name: "RecentlyViewedProducts",
                pattern: $"{pattern}/recentlyviewedproducts/",
                defaults: new { controller = "Product", action = "RecentlyViewedProducts" });

            //new products
            endpointRouteBuilder.MapControllerRoute(name: "NewProducts",
                pattern: $"{pattern}/newproducts/",
                defaults: new { controller = "Product", action = "NewProducts" });

            //blog
            endpointRouteBuilder.MapControllerRoute(name: "Blog",
                pattern: $"{pattern}/blog",
                defaults: new { controller = "Blog", action = "List" });

            //news
            endpointRouteBuilder.MapControllerRoute(name: "NewsArchive",
                pattern: $"{pattern}/news",
                defaults: new { controller = "News", action = "List" });

            //forum
            endpointRouteBuilder.MapControllerRoute(name: "Boards",
                pattern: $"{pattern}/boards",
                defaults: new { controller = "Boards", action = "Index" });

            //compare products
            endpointRouteBuilder.MapControllerRoute(name: "CompareProducts",
                pattern: $"{pattern}/compareproducts/",
                defaults: new { controller = "Product", action = "CompareProducts" });

            //product tags
            endpointRouteBuilder.MapControllerRoute(name: "ProductTagsAll",
                pattern: $"{pattern}/producttag/all/",
                defaults: new { controller = "Catalog", action = "ProductTagsAll" });

            //manufacturers
            endpointRouteBuilder.MapControllerRoute(name: "ManufacturerList",
                pattern: $"{pattern}/manufacturer/all/",
                defaults: new { controller = "Catalog", action = "ManufacturerAll" });

            //vendors
            endpointRouteBuilder.MapControllerRoute(name: "VendorList",
                pattern: $"{pattern}/vendor/all/",
                defaults: new { controller = "Catalog", action = "VendorAll" });

            //add product to cart (without any attributes and options). used on catalog pages. (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "AddProductToCart-Catalog",
                pattern: $"addproducttocart/catalog/{{productId:min(0)}}/{{shoppingCartTypeId:min(0)}}/{{quantity:min(0)}}",
                defaults: new { controller = "ShoppingCart", action = "AddProductToCart_Catalog" });

            //add product to cart (with attributes and options). used on the product details pages. (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "AddProductToCart-Details",
                pattern: $"addproducttocart/details/{{productId:min(0)}}/{{shoppingCartTypeId:min(0)}}",
                defaults: new { controller = "ShoppingCart", action = "AddProductToCart_Details" });

            //comparing products (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "AddProductToCompare",
                pattern: $"compareproducts/add/{{productId:min(0)}}",
                defaults: new { controller = "Product", action = "AddProductToCompareList" });

            //product email a friend
            endpointRouteBuilder.MapControllerRoute(name: "ProductEmailAFriend",
                pattern: $"{pattern}/productemailafriend/{{productId:min(0)}}",
                defaults: new { controller = "Product", action = "ProductEmailAFriend" });

            //reviews
            endpointRouteBuilder.MapControllerRoute(name: "ProductReviews",
                pattern: $"{pattern}/productreviews/{{productId}}",
                defaults: new { controller = "Product", action = "ProductReviews" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerProductReviews",
                pattern: $"{pattern}/customer/productreviews",
                defaults: new { controller = "Product", action = "CustomerProductReviews" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerProductReviewsPaged",
                pattern: $"{pattern}/customer/productreviews/page/{{pageNumber:min(0)}}",
                defaults: new { controller = "Product", action = "CustomerProductReviews" });

            //back in stock notifications (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "BackInStockSubscribePopup",
                pattern: $"backinstocksubscribe/{{productId:min(0)}}",
                defaults: new { controller = "BackInStockSubscription", action = "SubscribePopup" });

            endpointRouteBuilder.MapControllerRoute(name: "BackInStockSubscribeSend",
                pattern: $"backinstocksubscribesend/{{productId:min(0)}}",
                defaults: new { controller = "BackInStockSubscription", action = "SubscribePopupPOST" });

            //downloads (file result)
            endpointRouteBuilder.MapControllerRoute(name: "GetSampleDownload",
                pattern: $"download/sample/{{productid:min(0)}}",
                defaults: new { controller = "Download", action = "Sample" });

            //checkout pages
            endpointRouteBuilder.MapControllerRoute(name: "Checkout",
                pattern: $"{pattern}/checkout/",
                defaults: new { controller = "Checkout", action = "Index" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutOnePage",
                pattern: $"{pattern}/onepagecheckout/",
                defaults: new { controller = "Checkout", action = "OnePageCheckout" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutShippingAddress",
                pattern: $"{pattern}/checkout/shippingaddress",
                defaults: new { controller = "Checkout", action = "ShippingAddress" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutSelectShippingAddress",
                pattern: $"{pattern}/checkout/selectshippingaddress",
                defaults: new { controller = "Checkout", action = "SelectShippingAddress" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutBillingAddress",
                pattern: $"{pattern}/checkout/billingaddress",
                defaults: new { controller = "Checkout", action = "BillingAddress" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutSelectBillingAddress",
                pattern: $"{pattern}/checkout/selectbillingaddress",
                defaults: new { controller = "Checkout", action = "SelectBillingAddress" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutShippingMethod",
                pattern: $"{pattern}/checkout/shippingmethod",
                defaults: new { controller = "Checkout", action = "ShippingMethod" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutPaymentMethod",
                pattern: $"{pattern}/checkout/paymentmethod",
                defaults: new { controller = "Checkout", action = "PaymentMethod" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutPaymentInfo",
                pattern: $"{pattern}/checkout/paymentinfo",
                defaults: new { controller = "Checkout", action = "PaymentInfo" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutConfirm",
                pattern: $"{pattern}/checkout/confirm",
                defaults: new { controller = "Checkout", action = "Confirm" });

            endpointRouteBuilder.MapControllerRoute(name: "CheckoutCompleted",
                pattern: $"{pattern}/checkout/completed/{{orderId:int?}}",
                defaults: new { controller = "Checkout", action = "Completed" });

            //subscribe newsletters (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "SubscribeNewsletter",
                pattern: $"subscribenewsletter",
                defaults: new { controller = "Newsletter", action = "SubscribeNewsletter" });

            //email wishlist
            endpointRouteBuilder.MapControllerRoute(name: "EmailWishlist",
                pattern: $"{pattern}/emailwishlist",
                defaults: new { controller = "ShoppingCart", action = "EmailWishlist" });

            //login page for checkout as guest
            endpointRouteBuilder.MapControllerRoute(name: "LoginCheckoutAsGuest",
                pattern: $"{pattern}/login/checkoutasguest",
                defaults: new { controller = "Customer", action = "Login", checkoutAsGuest = true });

            //register result page
            endpointRouteBuilder.MapControllerRoute(name: "RegisterResult",
                pattern: $"{pattern}/registerresult/{{resultId:min(0)}}",
                defaults: new { controller = "Customer", action = "RegisterResult" });

            //check username availability (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "CheckUsernameAvailability",
                pattern: $"customer/checkusernameavailability",
                defaults: new { controller = "Customer", action = "CheckUsernameAvailability" });

            //passwordrecovery
            endpointRouteBuilder.MapControllerRoute(name: "PasswordRecovery",
                pattern: $"{pattern}/passwordrecovery",
                defaults: new { controller = "Customer", action = "PasswordRecovery" });

            //password recovery confirmation
            endpointRouteBuilder.MapControllerRoute(name: "PasswordRecoveryConfirm",
                pattern: $"{pattern}/passwordrecovery/confirm",
                defaults: new { controller = "Customer", action = "PasswordRecoveryConfirm" });

            //topics (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "TopicPopup",
                pattern: $"t-popup/{{SystemName}}",
                defaults: new { controller = "Topic", action = "TopicDetailsPopup" });

            //blog
            endpointRouteBuilder.MapControllerRoute(name: "BlogByTag",
                pattern: $"{pattern}/blog/tag/{{tag}}",
                defaults: new { controller = "Blog", action = "BlogByTag" });

            endpointRouteBuilder.MapControllerRoute(name: "BlogByMonth",
                pattern: $"{pattern}/blog/month/{{month}}",
                defaults: new { controller = "Blog", action = "BlogByMonth" });

            //blog RSS (file result)
            endpointRouteBuilder.MapControllerRoute(name: "BlogRSS",
                pattern: $"blog/rss/{{languageId:min(0)}}",
                defaults: new { controller = "Blog", action = "ListRss" });

            //news RSS (file result)
            endpointRouteBuilder.MapControllerRoute(name: "NewsRSS",
                pattern: $"news/rss/{{languageId:min(0)}}",
                defaults: new { controller = "News", action = "ListRss" });

            //set review helpfulness (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "SetProductReviewHelpfulness",
                pattern: $"setproductreviewhelpfulness",
                defaults: new { controller = "Product", action = "SetProductReviewHelpfulness" });

            //customer account links
            endpointRouteBuilder.MapControllerRoute(name: "CustomerReturnRequests",
                pattern: $"{pattern}/returnrequest/history",
                defaults: new { controller = "ReturnRequest", action = "CustomerReturnRequests" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerDownloadableProducts",
                pattern: $"{pattern}/customer/downloadableproducts",
                defaults: new { controller = "Customer", action = "DownloadableProducts" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerBackInStockSubscriptions",
                pattern: $"{pattern}/backinstocksubscriptions/manage/{{pageNumber:int?}}",
                defaults: new { controller = "BackInStockSubscription", action = "CustomerSubscriptions" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerRewardPoints",
                pattern: $"{pattern}/rewardpoints/history",
                defaults: new { controller = "Order", action = "CustomerRewardPoints" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerRewardPointsPaged",
                pattern: $"{pattern}/rewardpoints/history/page/{{pageNumber:min(0)}}",
                defaults: new { controller = "Order", action = "CustomerRewardPoints" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerChangePassword",
                pattern: $"{pattern}/customer/changepassword",
                defaults: new { controller = "Customer", action = "ChangePassword" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerAvatar",
                pattern: $"{pattern}/customer/avatar",
                defaults: new { controller = "Customer", action = "Avatar" });

            endpointRouteBuilder.MapControllerRoute(name: "AccountActivation",
                pattern: $"{pattern}/customer/activation",
                defaults: new { controller = "Customer", action = "AccountActivation" });

            endpointRouteBuilder.MapControllerRoute(name: "EmailRevalidation",
                pattern: $"{pattern}/customer/revalidateemail",
                defaults: new { controller = "Customer", action = "EmailRevalidation" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerForumSubscriptions",
                pattern: $"{pattern}/boards/forumsubscriptions/{{pageNumber:int?}}",
                defaults: new { controller = "Boards", action = "CustomerForumSubscriptions" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerAddressEdit",
                pattern: $"{pattern}/customer/addressedit/{{addressId:min(0)}}",
                defaults: new { controller = "Customer", action = "AddressEdit" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerAddressAdd",
                pattern: $"{pattern}/customer/addressadd",
                defaults: new { controller = "Customer", action = "AddressAdd" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerMultiFactorAuthenticationProviderConfig",
                pattern: $"{pattern}/customer/providerconfig",
                defaults: new { controller = "Customer", action = "ConfigureMultiFactorAuthenticationProvider" });

            //customer profile page
           endpointRouteBuilder.MapControllerRoute(name: "CustomerProfile",
                pattern: $"{pattern}/profile/{{id:min(0)}}",
                defaults: new { controller = "Profile", action = "Index" });

            endpointRouteBuilder.MapControllerRoute(name: "CustomerProfilePaged",
                pattern: $"{pattern}/profile/{{id:min(0)}}/page/{{pageNumber:min(0)}}",
                defaults: new { controller = "Profile", action = "Index" });


            endpointRouteBuilder.MapControllerRoute("QuoteDetails",
                pattern + "quotedetails/{quoteId:min(0)}",
                new { controller = "Quote", action = "QuoteDetails" });

            endpointRouteBuilder.MapControllerRoute("QuoteReport",
                pattern + "quotereport/{quoteId:min(0)}",
                new { controller = "Quote", action = "QuoteReport" });

            //orders
            endpointRouteBuilder.MapControllerRoute("OrderConf",
                pattern + "shoporderconfirmation/{shopOrderId:min(0)}",
                new { controller = "Order", action = "ShopOrderConfirmation" });

            endpointRouteBuilder.MapControllerRoute("OrderMtrDoc",
                pattern + "ordermtrdoc/{shopOrderMtrId:min(0)}",
                new { controller = "Order", action = "ShopOrderMtrDocument" });

            endpointRouteBuilder.MapControllerRoute("ShopOrderDetails",
                pattern + "shoporderdetails/{shopOrderNumber:min(0)}",
                new { controller = "Order", action = "ShopOrderDetails" });

           endpointRouteBuilder.MapControllerRoute(name: "OrderDetails",
                pattern: $"{pattern}/orderdetails/{{orderId:min(0)}}",
                defaults: new { controller = "Order", action = "Details" });

            endpointRouteBuilder.MapControllerRoute(name: "ShipmentDetails",
                pattern: $"{pattern}/orderdetails/shipment/{{shipmentId}}",
                defaults: new { controller = "Order", action = "ShipmentDetails" });

            endpointRouteBuilder.MapControllerRoute(name: "ReturnRequest",
                pattern: $"{pattern}/returnrequest/{{orderId:min(0)}}",
                defaults: new { controller = "ReturnRequest", action = "ReturnRequest" });

            endpointRouteBuilder.MapControllerRoute(name: "ReOrder",
                pattern: $"{pattern}/reorder/{{orderId:min(0)}}",
                defaults: new { controller = "Order", action = "ReOrder" });

            //pdf invoice (file result)
            endpointRouteBuilder.MapControllerRoute(name: "GetOrderPdfInvoice",
                pattern: $"orderdetails/pdf/{{orderId}}",
                defaults: new { controller = "Order", action = "GetPdfInvoice" });

            endpointRouteBuilder.MapControllerRoute(name: "PrintOrderDetails",
                pattern: $"{pattern}/orderdetails/print/{{orderId}}",
                defaults: new { controller = "Order", action = "PrintOrderDetails" });

            //order downloads (file result)
            endpointRouteBuilder.MapControllerRoute(name: "GetDownload", 
                pattern: $"download/getdownload/{{orderItemId:guid}}/{{agree?}}",
                defaults: new { controller = "Download", action = "GetDownload" });

            endpointRouteBuilder.MapControllerRoute(name: "GetLicense",
                pattern: $"download/getlicense/{{orderItemId:guid}}/",
                defaults: new { controller = "Download", action = "GetLicense" });

            endpointRouteBuilder.MapControllerRoute(name: "DownloadUserAgreement",
                pattern: $"customer/useragreement/{{orderItemId:guid}}",
                defaults: new { controller = "Customer", action = "UserAgreement" });

            endpointRouteBuilder.MapControllerRoute(name: "GetOrderNoteFile",
                pattern: $"download/ordernotefile/{{ordernoteid:min(0)}}",
                defaults: new { controller = "Download", action = "GetOrderNoteFile" });
            //contact vendor
            endpointRouteBuilder.MapControllerRoute(name: "ContactVendor",
                pattern: $"{pattern}/contactvendor/{{vendorId}}",
                defaults: new { controller = "Common", action = "ContactVendor" });

            //apply for vendor account
            endpointRouteBuilder.MapControllerRoute(name: "ApplyVendorAccount",
                pattern: $"{pattern}/vendor/apply",
                defaults: new { controller = "Vendor", action = "ApplyVendor" });

            //vendor info
            endpointRouteBuilder.MapControllerRoute(name: "CustomerVendorInfo",
                pattern: $"{pattern}/customer/vendorinfo",
                defaults: new { controller = "Vendor", action = "Info" });

            //customer GDPR
            endpointRouteBuilder.MapControllerRoute(name: "GdprTools",
                pattern: $"{pattern}/customer/gdpr",
                defaults: new { controller = "Customer", action = "GdprTools" });

            //customer check gift card balance 
            endpointRouteBuilder.MapControllerRoute(name: "CheckGiftCardBalance",
                pattern: $"{pattern}/customer/checkgiftcardbalance",
                defaults: new { controller = "Customer", action = "CheckGiftCardBalance" });

            //customer multi-factor authentication settings 
            endpointRouteBuilder.MapControllerRoute(name: "MultiFactorAuthenticationSettings",
                pattern: $"{pattern}/customer/multifactorauthentication",
                defaults: new { controller = "Customer", action = "MultiFactorAuthentication" });

            //poll vote (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "PollVote",
                pattern: $"poll/vote",
                defaults: new { controller = "Poll", action = "Vote" });

            //comparing products
            endpointRouteBuilder.MapControllerRoute(name: "RemoveProductFromCompareList",
                pattern: $"{pattern}/compareproducts/remove/{{productId}}",
                defaults: new { controller = "Product", action = "RemoveProductFromCompareList" });

            endpointRouteBuilder.MapControllerRoute(name: "ClearCompareList",
                pattern: $"{pattern}/clearcomparelist/",
                defaults: new { controller = "Product", action = "ClearCompareList" });

            //new RSS (file result)
            endpointRouteBuilder.MapControllerRoute(name: "NewProductsRSS",
                pattern: $"newproducts/rss",
                defaults: new { controller = "Product", action = "NewProductsRss" });

            //get state list by country ID (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "GetStatesByCountryId", 
                pattern: $"country/getstatesbycountryid/",
                defaults: new { controller = "Country", action = "GetStatesByCountryId" });

            //EU Cookie law accept button handler (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "EuCookieLawAccept",
                pattern: $"eucookielawaccept",
                defaults: new { controller = "Common", action = "EuCookieLawAccept" });

            //authenticate topic (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "TopicAuthenticate",
                pattern: $"topic/authenticate",
                defaults: new { controller = "Topic", action = "Authenticate" });

            //prepare top menu (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "GetCatalogRoot",
                pattern: $"catalog/getcatalogroot",
                defaults: new { controller = "Catalog", action = "GetCatalogRoot" });

            endpointRouteBuilder.MapControllerRoute(name: "GetCatalogSubCategories",
                pattern: $"catalog/getcatalogsubcategories",
                defaults: new { controller = "Catalog", action = "GetCatalogSubCategories" });

            //Catalog products (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "GetCategoryProducts",
                pattern: $"category/products/",
                defaults: new { controller = "Catalog", action = "GetCategoryProducts" });

            endpointRouteBuilder.MapControllerRoute(name: "GetManufacturerProducts",
                pattern: $"manufacturer/products/",
                defaults: new { controller = "Catalog", action = "GetManufacturerProducts" });

            endpointRouteBuilder.MapControllerRoute(name: "GetTagProducts",
                pattern: $"tag/products",
                defaults: new { controller = "Catalog", action = "GetTagProducts" });

            endpointRouteBuilder.MapControllerRoute(name: "SearchProducts",
                pattern: $"product/search",
                defaults: new { controller = "Catalog", action = "SearchProducts" });

            endpointRouteBuilder.MapControllerRoute(name: "GetVendorProducts",
                pattern: $"vendor/products",
                defaults: new { controller = "Catalog", action = "GetVendorProducts" });

            //product combinations (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "GetProductCombinations",
                pattern: $"product/combinations",
                defaults: new { controller = "Product", action = "GetProductCombinations" });

            //product attributes with "upload file" type (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "UploadFileProductAttribute",
                pattern: $"uploadfileproductattribute/{{attributeId:min(0)}}",
                defaults: new { controller = "ShoppingCart", action = "UploadFileProductAttribute" });

            //checkout attributes with "upload file" type (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "UploadFileCheckoutAttribute",
                pattern: $"uploadfilecheckoutattribute/{{attributeId:min(0)}}",
                defaults: new { controller = "ShoppingCart", action = "UploadFileCheckoutAttribute" });

            //return request with "upload file" support (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "UploadFileReturnRequest",
                pattern: $"uploadfilereturnrequest",
                defaults: new { controller = "ReturnRequest", action = "UploadFileReturnRequest" });

            //forums
            endpointRouteBuilder.MapControllerRoute(name: "ActiveDiscussions",
                pattern: $"{pattern}/boards/activediscussions",
                defaults: new { controller = "Boards", action = "ActiveDiscussions" });

            endpointRouteBuilder.MapControllerRoute(name: "ActiveDiscussionsPaged",
                pattern: $"{pattern}/boards/activediscussions/page/{{pageNumber:int}}",
                defaults: new { controller = "Boards", action = "ActiveDiscussions" });

            //forums RSS (file result)
            endpointRouteBuilder.MapControllerRoute(name: "ActiveDiscussionsRSS",
                pattern: $"boards/activediscussionsrss",
                defaults: new { controller = "Boards", action = "ActiveDiscussionsRSS" });

            endpointRouteBuilder.MapControllerRoute(name: "PostEdit",
                pattern: $"{pattern}/boards/postedit/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "PostEdit" });

            endpointRouteBuilder.MapControllerRoute(name: "PostDelete",
                pattern: $"{pattern}/boards/postdelete/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "PostDelete" });

            endpointRouteBuilder.MapControllerRoute(name: "PostCreate",
                pattern: $"{pattern}/boards/postcreate/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "PostCreate" });

            endpointRouteBuilder.MapControllerRoute(name: "PostCreateQuote",
                pattern: $"{pattern}/boards/postcreate/{{id:min(0)}}/{{quote:min(0)}}",
                defaults: new { controller = "Boards", action = "PostCreate" });

            endpointRouteBuilder.MapControllerRoute(name: "TopicEdit",
                pattern: $"{pattern}/boards/topicedit/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "TopicEdit" });

            endpointRouteBuilder.MapControllerRoute(name: "TopicDelete",
                pattern: $"{pattern}/boards/topicdelete/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "TopicDelete" });

            endpointRouteBuilder.MapControllerRoute(name: "TopicCreate",
                pattern: $"{pattern}/boards/topiccreate/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "TopicCreate" });

            endpointRouteBuilder.MapControllerRoute(name: "TopicMove",
                pattern: $"{pattern}/boards/topicmove/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "TopicMove" });

            //topic watch (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "TopicWatch",
                pattern: $"boards/topicwatch/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "TopicWatch" });

            endpointRouteBuilder.MapControllerRoute(name: "TopicSlug",
                pattern: $"{pattern}/boards/topic/{{id:min(0)}}/{{slug?}}",
                defaults: new { controller = "Boards", action = "Topic" });

            endpointRouteBuilder.MapControllerRoute(name: "TopicSlugPaged",
                pattern: $"{pattern}/boards/topic/{{id:min(0)}}/{{slug?}}/page/{{pageNumber:int}}",
                defaults: new { controller = "Boards", action = "Topic" });

            //forum watch (AJAX)
            endpointRouteBuilder.MapControllerRoute(name: "ForumWatch",
                pattern: $"boards/forumwatch/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "ForumWatch" });

            //forums RSS (file result)
            endpointRouteBuilder.MapControllerRoute(name: "ForumRSS",
                pattern: $"boards/forumrss/{{id:min(0)}}",
                defaults: new { controller = "Boards", action = "ForumRSS" });

            endpointRouteBuilder.MapControllerRoute(name: "ForumSlug",
                pattern: $"{pattern}/boards/forum/{{id:min(0)}}/{{slug?}}",
                defaults: new { controller = "Boards", action = "Forum" });

            endpointRouteBuilder.MapControllerRoute(name: "ForumSlugPaged",
                pattern: $"{pattern}/boards/forum/{{id:min(0)}}/{{slug?}}/page/{{pageNumber:int}}",
                defaults: new { controller = "Boards", action = "Forum" });

            endpointRouteBuilder.MapControllerRoute(name: "ForumGroupSlug",
                pattern: $"{pattern}/boards/forumgroup/{{id:min(0)}}/{{slug?}}",
                defaults: new { controller = "Boards", action = "ForumGroup" });

            endpointRouteBuilder.MapControllerRoute(name: "Search",
                pattern: $"{pattern}/boards/search",
                defaults: new { controller = "Boards", action = "Search" });

            //private messages
            endpointRouteBuilder.MapControllerRoute(name: "PrivateMessages",
                pattern: $"{pattern}/privatemessages/{{tab?}}",
                defaults: new { controller = "PrivateMessages", action = "Index" });

            endpointRouteBuilder.MapControllerRoute(name: "PrivateMessagesPaged",
                pattern: $"{pattern}/privatemessages/{{tab?}}/page/{{pageNumber:min(0)}}",
                defaults: new { controller = "PrivateMessages", action = "Index" });

            endpointRouteBuilder.MapControllerRoute(name: "PrivateMessagesInbox",
                pattern: $"{pattern}/inboxupdate",
                defaults: new { controller = "PrivateMessages", action = "InboxUpdate" });

            endpointRouteBuilder.MapControllerRoute(name: "PrivateMessagesSent",
                pattern: $"{pattern}/sentupdate",
                defaults: new { controller = "PrivateMessages", action = "SentUpdate" });

            endpointRouteBuilder.MapControllerRoute(name: "SendPM",
                pattern: $"{pattern}/sendpm/{{toCustomerId:min(0)}}",
                defaults: new { controller = "PrivateMessages", action = "SendPM" });

            endpointRouteBuilder.MapControllerRoute(name: "SendPMReply",
                pattern: $"{pattern}/sendpm/{{toCustomerId:min(0)}}/{{replyToMessageId:min(0)}}",
                defaults: new { controller = "PrivateMessages", action = "SendPM" });

            endpointRouteBuilder.MapControllerRoute(name: "ViewPM",
                pattern: $"{pattern}/viewpm/{{privateMessageId:min(0)}}",
                defaults: new { controller = "PrivateMessages", action = "ViewPM" });

            endpointRouteBuilder.MapControllerRoute(name: "DeletePM",
                pattern: $"{pattern}/deletepm/{{privateMessageId:min(0)}}",
                defaults: new { controller = "PrivateMessages", action = "DeletePM" });

            //activate newsletters
            endpointRouteBuilder.MapControllerRoute(name: "NewsletterActivation",
                pattern: $"{pattern}/newsletter/subscriptionactivation/{{token:guid}}/{{active}}",
                defaults: new { controller = "Newsletter", action = "SubscriptionActivation" });

            //robots.txt (file result)
            endpointRouteBuilder.MapControllerRoute(name: "robots.txt",
                pattern: $"robots.txt",
                defaults: new { controller = "Common", action = "RobotsTextFile" });

            //sitemap
            endpointRouteBuilder.MapControllerRoute(name: "Sitemap",
                pattern: $"{pattern}/sitemap",
                defaults: new { controller = "Common", action = "Sitemap" });

            //sitemap.xml (file result)
            endpointRouteBuilder.MapControllerRoute(name: "sitemap.xml",
                pattern: $"sitemap.xml",
                defaults: new { controller = "Common", action = "SitemapXml" });

            endpointRouteBuilder.MapControllerRoute(name: "sitemap-indexed.xml",
                pattern: $"sitemap-{{Id:min(0)}}.xml",
                defaults: new { controller = "Common", action = "SitemapXml" });

            //store closed
            endpointRouteBuilder.MapControllerRoute(name: "StoreClosed",
                pattern: $"{pattern}/storeclosed",
                defaults: new { controller = "Common", action = "StoreClosed" });

            //install
            endpointRouteBuilder.MapControllerRoute(name: "Installation",
                pattern: $"{NopInstallationDefaults.InstallPath}",
                defaults: new { controller = "Install", action = "Index" });

            //error page
            endpointRouteBuilder.MapControllerRoute(name: "Error",
                pattern: $"error",
                defaults: new { controller = "Common", action = "Error" });

            //page not found
            endpointRouteBuilder.MapControllerRoute("PageNotFound", $"{pattern}page-not-found",
                new { controller = "Common", action = "PageNotFound" });

            //Albina Bending calculators
            endpointRouteBuilder.MapControllerRoute("BendingCalculators", $"{pattern}/calculators/bending-calculators",
                new { controller = "Calculators", action = "BendingCalculators"});

            //Albina spiral math calculator
            endpointRouteBuilder.MapControllerRoute("SpiralCalculator", $"{pattern}/calculators/spiral-calculator",
                new { controller = "Calculators", action = "SpiralCalculator"});

            //Albina tangent materials
            endpointRouteBuilder.MapControllerRoute("TangentMaterials", $"{pattern}/calculators/tangent-materials",
                new { controller = "Calculators", action = "TangentMaterials"});

            //Albina bending tolerences
            endpointRouteBuilder.MapControllerRoute("BendingTolerances", $"{pattern}/calculators/bending-tolerances",
                new { controller = "Calculators", action = "BendingTolerances"});

            //Albina Browse Picture Vault
            endpointRouteBuilder.MapControllerRoute("BrowsePictureVault", $"{pattern}/secure/browse-picture-vault",
                new {controller = "PictureVault", action = "BrowsePictureVault"});

            //Albina Picture Vault
            endpointRouteBuilder.MapControllerRoute("PictureVault", $"{pattern}/secure/picture-vault",
                new {controller = "PictureVault", action = "PictureVault"});

            //Albina Picture Vault
            endpointRouteBuilder.MapControllerRoute("ShowPicture", $"{pattern}/secure/show-picture",
                new {controller = "PictureVault", action = "ShowPicture"});

            endpointRouteBuilder.MapControllerRoute("ShowVideo", $"{pattern}/secure/show-video",
                new {controller = "PictureVault", action = "ShowVideo"});

            endpointRouteBuilder.MapControllerRoute("ShowVideo", $"{pattern}/secure/video-player",
                new {controller = "PictureVault", action = "VideoPlayer"});

            //Albina Bending and Fabrication
            endpointRouteBuilder.MapControllerRoute("BendingAndFabrication", $"{pattern}/quote-request/bending-and-fabrication",
                new {controller = "QuoteRequest", action = "BendingAndFabrication"});

            //Albina request account
            endpointRouteBuilder.MapControllerRoute("RequestAccount", "request-account",
                new {controller = "Common", action = "RequestAccount"});

            //Albina Bending and Fabrication success
            endpointRouteBuilder.MapControllerRoute("BendingAndFabrication", $"{pattern}/quote-request/bending-and-fabrication-success",
                new {controller = "QuoteRequest", action = "BendingAndFabricationSuccess"});

            //Albina Bending and Fabrication Google
            endpointRouteBuilder.MapControllerRoute("BendingAndFabrication", $"{pattern}/quote-request/bending-and-fabrication-google",
                new {controller = "QuoteRequest", action = "BendingAndFabricationGoogle"});

            //Albina Download Spiral Math Report
            endpointRouteBuilder.MapControllerRoute("DownloadSpiralReport", $"{pattern}/download-spiral-math",
                new {controller = "Calculators", action = "DownloadSpiralReport"});
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 0;

        #endregion
    }
}