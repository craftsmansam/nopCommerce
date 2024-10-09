﻿using FluentMigrator;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Localization;
using Nop.Web.Framework.Extensions;

namespace Nop.Web.Framework.Migrations.UpgradeTo480;

[NopUpdateMigration("2024-05-15 00:00:00", "4.80", UpdateMigrationType.Localization)]
public class LocalizationMigration : MigrationBase
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        //do not use DI, because it produces exception on the installation process
        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

        var (languageId, _) = this.GetLanguageData();

        #region Delete locales

        localizationService.DeleteLocaleResources(new List<string>
        {
            //#6977
            "Common.FileUploader.Upload",
            "Common.FileUploader.Upload.Files",
            "Common.FileUploader.Cancel",
            "Common.FileUploader.Retry",
            "Common.FileUploader.Delete",

            //#7215
            "Admin.Configuration.Settings.ProductEditor.DisplayAttributeCombinationImagesOnly",

            //#374
            "Admin.Configuration.Plugins.Fields.AclCustomerRoles",
            "Admin.Configuration.Plugins.Fields.AclCustomerRoles.Hint",
            "Admin.Catalog.Categories.Fields.AclCustomerRoles",
            "Admin.Catalog.Categories.Fields.AclCustomerRoles.Hint",
            "Admin.Catalog.Manufacturers.Fields.AclCustomerRoles",
            "Admin.Catalog.Manufacturers.Fields.AclCustomerRoles.Hint",
            "Admin.Catalog.Products.Fields.AclCustomerRoles",
            "Admin.Catalog.Products.Fields.AclCustomerRoles.Hint",
            "Admin.ContentManagement.Topics.Fields.AclCustomerRoles",
            "Admin.ContentManagement.Topics.Fields.AclCustomerRoles.Hint",
            "Permission.AccessAdminPanel",
            "Permission.AccessWebService",
            "Permission.AccessClosedStore",
            "Permission.EnableMultiFactorAuthentication",
            "Permission.ManageVendors",
            "Permission.AllowCustomerImpersonation",
            "Permission.Authentication.EnableMultiFactorAuthentication",
            "Permission.Authentication.ManageExternalMethods",
            "Permission.Authentication.ManageMultifactorMethods",
            "Permission.DisplayPrices",
            "Permission.EnableShoppingCart",
            "Permission.EnableWishlist",
            "Permission.HtmlEditor.ManagePictures",
            "Permission.ManageACL",
            "Permission.ManageActivityLog",
            "Permission.ManageAffiliates",
            "Permission.ManageAppSettings",
            "Permission.ManageAttributes",
            "Permission.ManageBlog",
            "Permission.ManageCampaigns",
            "Permission.ManageCategories",
            "Permission.ManageCountries",
            "Permission.ManageCurrencies",
            "Permission.ManageCurrentCarts",
            "Permission.ManageCustomers",
            "Permission.ManageDiscounts",
            "Permission.ManageEmailAccounts",
            "Permission.ManageExternalAuthenticationMethods",
            "Permission.ManageForums",
            "Permission.ManageGiftCards",
            "Permission.ManageLanguages",
            "Permission.ManageMaintenance",
            "Permission.ManageManufacturers",
            "Permission.ManageMessageQueue",
            "Permission.ManageMessageTemplates",
            "Permission.ManageMultifactorAuthenticationMethods",
            "Permission.ManageNews",
            "Permission.ManageNewsletterSubscribers",
            "Permission.ManageOrders",
            "Permission.ManagePaymentMethods",
            "Permission.ManagePlugins",
            "Permission.ManagePolls",
            "Permission.ManageProductReviews",
            "Permission.ManageProducts",
            "Permission.ManageProductTags",
            "Permission.ManageRecurringPayments",
            "Permission.ManageReturnRequests",
            "Permission.ManageScheduleTasks",
            "Permission.ManageSettings",
            "Permission.ManageShippingSettings",
            "Permission.ManageStores",
            "Permission.ManageSystemLog",
            "Permission.ManageTaxSettings",
            "Permission.ManageTopics",
            "Permission.ManageWidgets",
            "Permission.OrderCountryReport",
            "Permission.PublicStoreAllowNavigation",
            "Permission.SalesSummaryReport",
        });

        #endregion

        #region Rename locales

        #endregion

        #region Add or update locales

        localizationService.AddOrUpdateLocaleResource(new Dictionary<string, string>
        {
            //#6977
            ["Common.FileUploader.Browse"] = "Browse",
            ["Common.FileUploader.Processing"] = "Uploading...", 

            //#7089
            ["Admin.ContentManagement.MessageTemplates.List.SearchEmailAccount"] = "Email account",
            ["Admin.ContentManagement.MessageTemplates.List.SearchEmailAccount.All"] = "All",

            //#7108
            ["Admin.ContentManagement.MessageTemplates.Description.OrderCancelled.VendorNotification"] = "This message template is used to notify a vendor that the certain order was cancelled.The order can be cancelled by a customer on the account page or by store owner in Customers - Customers in Orders tab or in Sales - Orders.",

            //#7215
            ["Admin.Catalog.Products.Fields.DisplayAttributeCombinationImagesOnly.Hint"] = "You may choose pictures associated to each product attribute value or attribute combination (these pictures will replace the main product image when this product attribute value or attribute combination is selected). Enable this option if you want to display only images of a chosen product attribute value or a attribute combination (other pictures will be hidden). Otherwise, all uploaded pictures will be displayed on the product details page",

            //#7208
            ["Admin.Customers.Customers.List.SearchIsActive"] = "Is active",
            ["Admin.Customers.Customers.List.SearchIsActive.Hint"] = "Search customers by an account status.",

            //#374
            ["Admin.IAclSupportedModel.Fields.AclCustomerRoles"] = "Limited to customer roles",
            ["Admin.IAclSupportedModel.Fields.AclCustomerRoles.Hint"] = "Choose one or several customer roles i.e. administrators, vendors, guests, who will be able to use or see this item. If you don't need this option just leave this field empty.",
            ["Admin.Configuration.ACL.NoPermissionsDefined"] = "No permissions defined",
            ["Admin.Configuration.ACL.NoCustomerRolesAvailable"] = "No customer roles available",
            ["Admin.Configuration.ACL.Permission.CategoryName"] = "Category of permissions",
            ["Admin.Configuration.ACL.Permission.PermissionName"] = "Permission name",
            ["Admin.Configuration.ACL.Permission.Edit"] = "Edit permission rules",
            ["Security.Permission.Catalog.CategoriesCreateEditDelete"] = "Admin area. Categories. Create, edit, delete",
            ["Security.Permission.Catalog.CategoriesImportExport"] = "Admin area. Categories. Import and export",
            ["Security.Permission.Catalog.CategoriesView"] = "Admin area. Categories. View",
            ["Security.Permission.Catalog.CheckoutAttributesCreateEditDelete"] = "Admin area. Checkout attributes. Create, edit, delete",
            ["Security.Permission.Catalog.CheckoutAttributesView"] = "Admin area. Checkout attributes. View",
            ["Security.Permission.Catalog.ManufacturerCreateEditDelete"] = "Admin area. Manufacturer. Create, edit, delete",
            ["Security.Permission.Catalog.ManufacturerImportExport"] = "Admin area. Manufacturer. Import and export",
            ["Security.Permission.Catalog.ManufacturerView"] = "Admin area. Manufacturer. View",
            ["Security.Permission.Catalog.ProductAttributesCreateEditDelete"] = "Admin area. Product attributes. Create, edit, delete",
            ["Security.Permission.Catalog.ProductAttributesView"] = "Admin area. Product attributes. View",
            ["Security.Permission.Catalog.ProductReviewsCreateEditDelete"] = "Admin area. Product reviews. Create, edit, delete",
            ["Security.Permission.Catalog.ProductReviewsView"] = "Admin area. Product reviews. View",
            ["Security.Permission.Catalog.ProductsCreateEditDelete"] = "Admin area. Products. Create, edit, delete",
            ["Security.Permission.Catalog.ProductsImportExport"] = "Admin area. Products. Import and export",
            ["Security.Permission.Catalog.ProductsView"] = "Admin area. Products. View",
            ["Security.Permission.Catalog.ProductTagsCreateEditDelete"] = "Admin area. Product tags. Create, edit, delete",
            ["Security.Permission.Catalog.ProductTagsView"] = "Admin area. Product tags. View",
            ["Security.Permission.Catalog.SpecificationAttributesCreateEditDelete"] = "Admin area. Specification attributes. Create, edit, delete",
            ["Security.Permission.Catalog.SpecificationAttributesView"] = "Admin area. Specification attributes. View",
            ["Security.Permission.Configuration.ManageACL"] = "Admin area. ACL. Manage",
            ["Security.Permission.Configuration.ManageCountries"] = "Admin area. Countries. Manage",
            ["Security.Permission.Configuration.ManageCurrencies"] = "Admin area. Currencies. Manage",
            ["Security.Permission.Configuration.ManageEmailAccounts"] = "Admin area. Email Accounts. Manage",
            ["Security.Permission.Configuration.ManageExternalAuthenticationMethods"] = "Admin area. External Authentication Methods. Manage",
            ["Security.Permission.Configuration.ManageLanguages"] = "Admin area. Languages. Manage",
            ["Security.Permission.Configuration.ManageMultifactorauthenticationmethods"] = "Admin area. Multi-factor Authentication Methods. Manage",
            ["Security.Permission.Configuration.ManagePaymentMethods"] = "Admin area. Payment Methods. Manage",
            ["Security.Permission.Configuration.ManagePlugins"] = "Admin area. Plugins. Manage",
            ["Security.Permission.Configuration.ManageSettings"] = "Admin area. Settings. Manage",
            ["Security.Permission.Configuration.ManageShippingSettings"] = "Admin area. Shipping Settings. Manage",
            ["Security.Permission.Configuration.ManageStores"] = "Admin area. Stores. Manage",
            ["Security.Permission.Configuration.ManageTaxSettings"] = "Admin area. Tax Settings. Manage",
            ["Security.Permission.Configuration.ManageWidgets"] = "Admin area. Widgets. Manage",
            ["Security.Permission.ContentManagement.BlogCommentsCreateEditDelete"] = "Admin area. Blog comments. Create, edit, delete",
            ["Security.Permission.ContentManagement.BlogCommentsView"] = "Admin area. Blog comments. View",
            ["Security.Permission.ContentManagement.BlogCreateEditDelete"] = "Admin area. Blog. Create, edit, delete",
            ["Security.Permission.ContentManagement.BlogView"] = "Admin area. Blog. View",
            ["Security.Permission.ContentManagement.ForumsCreateEditDelete"] = "Admin area. Forums. Create, edit, delete",
            ["Security.Permission.ContentManagement.ForumsView"] = "Admin area. Forums. View",
            ["Security.Permission.ContentManagement.MessageTemplatesCreateEditDelete"] = "Admin area. Message Templates. Create, edit, delete",
            ["Security.Permission.ContentManagement.MessageTemplatesView"] = "Admin area. Message Templates. View",
            ["Security.Permission.ContentManagement.NewsCommentsCreateEditDelete"] = "Admin area. News comments. Create, edit, delete",
            ["Security.Permission.ContentManagement.NewsCommentsView"] = "Admin area. News comments. View",
            ["Security.Permission.ContentManagement.NewsCreateEditDelete"] = "Admin area. News. Create, edit, delete",
            ["Security.Permission.ContentManagement.NewsView"] = "Admin area. News. View",
            ["Security.Permission.ContentManagement.PollsCreateEditDelete"] = "Admin area. Polls. Create, edit, delete",
            ["Security.Permission.ContentManagement.PollsView"] = "Admin area. Polls. View",
            ["Security.Permission.ContentManagement.TopicsCreateEditDelete"] = "Admin area. Topics. Create, edit, delete",
            ["Security.Permission.ContentManagement.TopicsView"] = "Admin area. Topics. View",
            ["Security.Permission.Customers.ActivityLogDelete"] = "Admin area. Activity Log. Delete",
            ["Security.Permission.Customers.ActivityLogManageTypes"] = "Admin area. Activity Log. Manage types",
            ["Security.Permission.Customers.ActivityLogView"] = "Admin area. Activity Log. View",
            ["Security.Permission.Customers.CustomerRolesCreateEditDelete"] = "Admin area. Customer roles. Create, edit, delete",
            ["Security.Permission.Customers.CustomerRolesView"] = "Admin area. Customer roles. View",
            ["Security.Permission.Customers.CustomersCreateEditDelete"] = "Admin area. Customers. Create, edit, delete",
            ["Security.Permission.Customers.CustomersImpersonation"] = "Admin area. Customers. Allow impersonation",
            ["Security.Permission.Customers.CustomersImportExport"] = "Admin area. Customers. Import and export",
            ["Security.Permission.Customers.CustomersView"] = "Admin area. Customers. View",
            ["Security.Permission.Customers.GdprManage"] = "Admin area. GDPR. Manage",
            ["Security.Permission.Customers.VendorsCreateEditDelete"] = "Admin area. Vendors. Create, edit, delete",
            ["Security.Permission.Customers.VendorsView"] = "Admin area. Vendors. View",
            ["Security.Permission.Orders.CurrentCartsManage"] = "Admin area. Current Carts. Manage",
            ["Security.Permission.Orders.GiftCardsCreateEditDelete"] = "Admin area. Gift cards. Create, edit, delete",
            ["Security.Permission.Orders.GiftCardsView"] = "Admin area. Gift cards. View",
            ["Security.Permission.Orders.OrdersCreateEditDelete"] = "Admin area. Orders. Create, edit, delete",
            ["Security.Permission.Orders.OrdersImportExport"] = "Admin area. Orders. Import and export",
            ["Security.Permission.Orders.OrdersView"] = "Admin area. Orders. View",
            ["Security.Permission.Orders.RecurringPaymentsCreateEditDelete"] = "Admin area. Recurring payments. Create, edit, delete",
            ["Security.Permission.Orders.RecurringPaymentsView"] = "Admin area. Recurring payments. View",
            ["Security.Permission.Orders.ReturnRequestsCreateEditDelete"] = "Admin area. Return requests. Create, edit, delete",
            ["Security.Permission.Orders.ReturnRequestsView"] = "Admin area. Return requests. View",
            ["Security.Permission.Orders.ShipmentsCreateEditDelete"] = "Admin area. Shipments. Create, edit, delete",
            ["Security.Permission.Orders.ShipmentsView"] = "Admin area. Shipments. View",
            ["Security.Permission.Promotions.AffiliatesCreateEditDelete"] = "Admin area. Affiliates. Create, edit, delete",
            ["Security.Permission.Promotions.AffiliatesView"] = "Admin area. Affiliates. View",
            ["Security.Permission.Promotions.CampaignsCreateEdit"] = "Admin area. Campaigns. Create and Edit",
            ["Security.Permission.Promotions.CampaignsDelete"] = "Admin area. Campaigns. Delete",
            ["Security.Permission.Promotions.CampaignsSendEmails"] = "Admin area. Campaigns. Send emails",
            ["Security.Permission.Promotions.CampaignsView"] = "Admin area. Campaigns. View",
            ["Security.Permission.Promotions.DiscountsCreateEditDelete"] = "Admin area. Discounts. Create, edit, delete",
            ["Security.Permission.Promotions.DiscountsView"] = "Admin area. Discounts. View",
            ["Security.Permission.Promotions.SubscribersCreateEditDelete"] = "Admin area. Newsletter Subscribers. Create, edit, delete",
            ["Security.Permission.Promotions.SubscribersImportExport"] = "Admin area. Newsletter Subscribers. Import and export",
            ["Security.Permission.Promotions.SubscribersView"] = "Admin area. Newsletter Subscribers. View",
            ["Security.Permission.PublicStore.AccessClosedStore"] = "Public store. Access a closed store",
            ["Security.Permission.PublicStore.DisplayPrices"] = "Public store. Display Prices",
            ["Security.Permission.PublicStore.EnableShoppingCart"] = "Public store. Enable shopping cart",
            ["Security.Permission.PublicStore.EnableWishlist"] = "Public store. Enable wishlist",
            ["Security.Permission.PublicStore.PublicStoreAllowNavigation"] = "Public store. Allow navigation",
            ["Security.Permission.Reports.Bestsellers"] = "Admin area. Reports. Bestsellers",
            ["Security.Permission.Reports.CountrySales"] = "Admin area. Reports. Country sales",
            ["Security.Permission.Reports.CustomersByNumberOfOrders"] = "Admin area. Reports. Customers by number of orders",
            ["Security.Permission.Reports.CustomersByOrderTotal"] = "Admin area. Reports. Customers by order total",
            ["Security.Permission.Reports.LowStock"] = "Admin area. Reports. Low stock",
            ["Security.Permission.Reports.ProductsNeverPurchased"] = "Admin area. Reports. Products never purchased",
            ["Security.Permission.Reports.RegisteredCustomers"] = "Admin area. Reports. Registered customers",
            ["Security.Permission.Reports.SalesSummary"] = "Admin area. Reports. Sales summary",
            ["Security.Permission.security.AccessAdminPanel"] = "Access admin area",
            ["Security.Permission.security.EnableMultiFactorAuthentication"] = "Security. Enable Multi-factor authentication",
            ["Security.Permission.System.HtmlEditor.ManagePictures"] = "Admin area. HTML Editor. Manage pictures",
            ["Security.Permission.System.ManageAppSettings"] = "Admin area. App Settings. Manage",
            ["Security.Permission.System.ManageMaintenance"] = "Admin area. Maintenance. Manage",
            ["Security.Permission.System.ManageMessageQueue"] = "Admin area. Message Queue. Manage",
            ["Security.Permission.System.ManageScheduleTasks"] = "Admin area. Schedule Tasks. Manage",
            ["Security.Permission.System.ManageSystemLog"] = "Admin area. System Log. Manage",

            //4306
            ["Admin.Configuration.Settings.Catalog.ShowSearchBoxCategories"] = "Show product categories for the search box",
            ["Admin.Configuration.Settings.Catalog.ShowSearchBoxCategories.Hint"] = "Check to display the drop-down list with product categories next to the search box.",
            ["Search.SearchBox.AllCategories"] = "All categories",
        }, languageId);

        #endregion
    }

    /// <summary>Collects the DOWN migration expressions</summary>
    public override void Down()
    {
        //add the downgrade logic if necessary 
    }
}
