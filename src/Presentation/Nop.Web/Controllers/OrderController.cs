﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Controllers
{
    public partial class OrderController : BaseWebAttachmentPublicController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IShopOrderModelFactory _shopOrderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IShopOrderService _shopOrderService;
        private readonly IPaymentService _paymentService;
        private readonly IPdfService _pdfService;
        private readonly IShipmentService _shipmentService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly ISettingService _settingService;

        #endregion

		#region Ctor

        public OrderController(ICustomerService customerService,
            IOrderModelFactory orderModelFactory,
            IShopOrderModelFactory shopOrderModelFactory,
            IOrderProcessingService orderProcessingService, 
            IOrderService orderService, 
            IShopOrderService shopOrderService, 
            IPaymentService paymentService, 
            IPdfService pdfService,
            IShipmentService shipmentService, 
            IWebHelper webHelper,
            IWorkContext workContext,
            RewardPointsSettings rewardPointsSettings,
            ISettingService settingService)
        {
            _customerService = customerService;
            _orderModelFactory = orderModelFactory;
            _shopOrderModelFactory = shopOrderModelFactory;
            _orderProcessingService = orderProcessingService;
            _orderService = orderService;
            _shopOrderService = shopOrderService;
            _paymentService = paymentService;
            _pdfService = pdfService;
            _shipmentService = shipmentService;
            _webHelper = webHelper;
            _workContext = workContext;
            _rewardPointsSettings = rewardPointsSettings;
            _settingService = settingService;
        }

        #endregion

        #region Methods

        //My account / Orders
        public virtual async Task<IActionResult> CustomerOrders()
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            var model = await _orderModelFactory.PrepareCustomerOrderListModelAsync();
            return View(model);
        }

        //My account / Orders / Cancel recurring order
        [HttpPost, ActionName("CustomerOrders")]
        [FormValueRequired(FormValueRequirement.StartsWith, "cancelRecurringPayment")]
        public virtual async Task<IActionResult> CancelRecurringPayment(IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            foreach (var formValue in form.Keys)
                if (formValue.StartsWith("cancelRecurringPayment", StringComparison.InvariantCultureIgnoreCase))
                    recurringPaymentId = Convert.ToInt32(formValue["cancelRecurringPayment".Length..]);

            var recurringPayment = await _orderService.GetRecurringPaymentByIdAsync(recurringPaymentId);
            if (recurringPayment == null)
            {
                return RedirectToRoute("CustomerOrders");
            }

            if (await _orderProcessingService.CanCancelRecurringPaymentAsync(customer, recurringPayment))
            {
                var errors = await _orderProcessingService.CancelRecurringPaymentAsync(recurringPayment);

                var model = await _orderModelFactory.PrepareCustomerOrderListModelAsync();
                model.RecurringPaymentErrors = errors;

                return View(model);
            }

            return RedirectToRoute("CustomerOrders");
        }

        //My account / Orders / Retry last recurring order
        [HttpPost, ActionName("CustomerOrders")]
        [FormValueRequired(FormValueRequirement.StartsWith, "retryLastPayment")]
        public virtual async Task<IActionResult> RetryLastRecurringPayment(IFormCollection form)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (!await _customerService.IsRegisteredAsync(customer))
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            if (!form.Keys.Any(formValue => formValue.StartsWith("retryLastPayment", StringComparison.InvariantCultureIgnoreCase) &&
                int.TryParse(formValue[(formValue.IndexOf('_') + 1)..], out recurringPaymentId)))
            {
                return RedirectToRoute("CustomerOrders");
            }

            var recurringPayment = await _orderService.GetRecurringPaymentByIdAsync(recurringPaymentId);
            if (recurringPayment == null)
                return RedirectToRoute("CustomerOrders");

            if (!await _orderProcessingService.CanRetryLastRecurringPaymentAsync(customer, recurringPayment))
                return RedirectToRoute("CustomerOrders");

            var errors = await _orderProcessingService.ProcessNextRecurringPaymentAsync(recurringPayment);
            var model = await _orderModelFactory.PrepareCustomerOrderListModelAsync();
            model.RecurringPaymentErrors = errors.ToList();

            return View(model);
        }

        //My account / Reward points
        public virtual async Task<IActionResult> CustomerRewardPoints(int? pageNumber)
        {
            if (!await _customerService.IsRegisteredAsync(await _workContext.GetCurrentCustomerAsync()))
                return Challenge();

            if (!_rewardPointsSettings.Enabled)
                return RedirectToRoute("CustomerInfo");

            var model = await _orderModelFactory.PrepareCustomerRewardPointsAsync(pageNumber);
            return View(model);
        }

        //My account / Order details page
        public virtual async Task<IActionResult> Details(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            ShopOrder shopOrder = null;
            if (int.TryParse(order.CustomOrderNumber, out var shopOrderNumber) && shopOrderNumber > 10000)
            {
                shopOrder = await _shopOrderService.GetShopOrderByShopOrderNumberAsync(shopOrderNumber);
            }

            var model = await _orderModelFactory.PrepareOrderDetailsModelAsync(order, shopOrder);
            return View(model);
        }

        //My account / Shop Order details page
        [HttpsRequirement]
        public virtual async Task<IActionResult> ShopOrderDetails(int shopOrderNumber)
        {
            var shopOrder = await _shopOrderService.GetShopOrderByShopOrderNumberAsync(shopOrderNumber);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (shopOrder == null || customer.JCCustomerID != shopOrder.CustomerID)
                return Challenge();

            var model = _shopOrderModelFactory.PrepareShopOrderDetailsModel(shopOrder);
            return View(model);
        }

        [HttpsRequirement]
        public virtual async Task<IActionResult> ShopOrderConfirmation(int shopOrderId)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (!_shopOrderService.TestShopOrderPermissions(shopOrderId, customer.JCCustomerID, out var shopOrder))
                return AccessDeniedView();

            return await ServeWebAttachmentAsync(_settingService, shopOrder.ShopOrderConfirmationFilename);
        }

        [HttpsRequirement]
        public virtual async Task<IActionResult> ShopOrderMtrDocument(int shopOrderMtrId)
        {
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (!_shopOrderService.TestMTRPermissions(shopOrderMtrId, customer.JCCustomerID, out var mtr, out var shopOrder))
                return AccessDeniedView();

            return await ServeWebAttachmentAsync(_settingService, mtr.FilenameOnDisk);
        }

        //My account / Order details page / Print
        public virtual async Task<IActionResult> PrintOrderDetails(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            ShopOrder shopOrder = null;
            if (int.TryParse(order.CustomOrderNumber, out var shopOrderNumber) && shopOrderNumber > 10000)
            {
                shopOrder = await _shopOrderService.GetShopOrderByShopOrderNumberAsync(shopOrderNumber);
            }

            var model = await _orderModelFactory.PrepareOrderDetailsModelAsync(order, shopOrder);
            model.PrintMode = true;

            return View("Details", model);
        }

        //My account / Order details page / PDF invoice
        [CheckLanguageSeoCode(ignore: true)]
        public virtual async Task<IActionResult> GetPdfInvoice(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            byte[] bytes;
            await using (var stream = new MemoryStream())
            {
                await _pdfService.PrintOrderToPdfAsync(stream, order, await _workContext.GetWorkingLanguageAsync());
                bytes = stream.ToArray();
            }
            return File(bytes, MimeTypes.ApplicationPdf, $"order_{order.CustomOrderNumber}.pdf");
        }

        //My account / Order details page / re-order
        public virtual async Task<IActionResult> ReOrder(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            await _orderProcessingService.ReOrderAsync(order);
            return RedirectToRoute("ShoppingCart");
        }

        //My account / Order details page / Complete payment
        [HttpPost, ActionName("Details")]
        
        [FormValueRequired("repost-payment")]
        public virtual async Task<IActionResult> RePostPayment(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            var customer = await _workContext.GetCurrentCustomerAsync();
            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            if (!await _paymentService.CanRePostProcessPaymentAsync(order))
                return RedirectToRoute("OrderDetails", new { orderId = orderId });

            var postProcessPaymentRequest = new PostProcessPaymentRequest
            {
                Order = order
            };
            await _paymentService.PostProcessPaymentAsync(postProcessPaymentRequest);

            if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
            {
                //redirection or POST has been done in PostProcessPayment
                return Content("Redirected");
            }

            //if no redirection has been done (to a third-party payment page)
            //theoretically it's not possible
            return RedirectToRoute("OrderDetails", new { orderId = orderId });
        }

        //My account / Order details page / Shipment details page
        public virtual async Task<IActionResult> ShipmentDetails(int shipmentId)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(shipmentId);
            if (shipment == null)
                return Challenge();

            var order = await _orderService.GetOrderByIdAsync(shipment.OrderId);
            var customer = await _workContext.GetCurrentCustomerAsync();

            if (order == null || order.Deleted || customer.Id != order.CustomerId)
                return Challenge();

            var model = await _orderModelFactory.PrepareShipmentDetailsModelAsync(shipment);
            return View(model);
        }
        
        #endregion
    }
}