using System.Collections.Generic;

namespace MyAlloySite.Constant
{
    public static class GlobalValues
    {
        public const string Default = "Default";
        public const string NameDes = "name_desc";
        public const string NameAsc = "name_asc";
        public const string PercentDesc = "percent_desc";
        public const string PercentAsc = "percent_asc";
        public const string PriceDesc = "price_desc";
        public const string PriceAsc = "price_asc";

        public static Dictionary<string, string> SortingDictionary = new Dictionary<string, string>
        {
            {"Default", GlobalValues.Default },
            {"Name Descending", NameDes },
            {"Name Ascending", NameAsc },
            {"Percent Descending", PercentDesc },
            {"Percent Ascending", PercentAsc },
            {"Price Descending", PriceDesc },
            {"Price Ascending", PriceAsc },
        };
    }

    public static class Metadata
    {
        public static class ContentType
        {
            public const string ProductEntryType = "Product";
            public const string VariantEntryType = "Variation";
            public const string EntryRelationType = "ProductVariation";
        }

        public static class LineItem
        {
            public const string FreeGiftItems = "FreeGiftItems";
            public const string IsPaymentExtraFeeItem = "IsPaymentExtraFeeItem";
        }

        public static class Cart
        {
            public const string ExtendServices = "ExtendServices";
            public const string CreatedAccountAfterCheckout = "CreatedAccountAfterCheckout";
            public const string AcceptMarketingEmail = "AcceptMarketingEmail";
            public const string CurrentLanguage = "CurrentLanguage";
            public const string IssueCompanyInvoice = "IssueCompanyInvoice";
            public const string UseDeliveryAddressForBillingAddress = "UseDeliveryAddressForBillingAddress";
            public const string PreferredCulture = "PreferredCulture";
            public const string IsSentAbandonedCart = "SentAbandonedCart";
            public const string CancelledDate = "CancelledDate";
            public const string IsSentConfirmationEmail = nameof(IsSentConfirmationEmail);
            public const string IsSentPickedUpEmail = nameof(IsSentPickedUpEmail);
            public const string IsSentDeliveredEmail = nameof(IsSentDeliveredEmail);
            public const string IsSentCancelledEmail = nameof(IsSentCancelledEmail);
            public const string VoucherSaved = nameof(VoucherSaved);
            public const string Checksum = nameof(Checksum);
            public const string GeneratedOrderNumber = nameof(GeneratedOrderNumber);

            public const string IsCreatedByWebhook = nameof(IsCreatedByWebhook);
            public const string BaseUnitPriceAfterTax = nameof(BaseUnitPriceAfterTax);
            public const string FinalUnitPriceAfterTax = nameof(FinalUnitPriceAfterTax);
            public const string IsSentEnquiryEmail = nameof(IsSentEnquiryEmail);
            public const string OrderStatusHistory = nameof(OrderStatusHistory);
            public const string TemporaryBillingAddress = nameof(TemporaryBillingAddress);
            public const string Timezone = nameof(Timezone);
            public const string SessionId = "Session_Id";
            public const string AllowReceiveMarketingInformation = nameof(AllowReceiveMarketingInformation);
        }

        public static class PurchaseOrder
        {
            public const string InvoiceId = "InvoiceId";
            public const string PdfInvoiceReference = "PdfInvoiceReference";
            public const string XmlInvoiceReference = "XmlInvoiceReference";
            public const string NextEngineOrderId = nameof(NextEngineOrderId);
            public const string PurchaseOrderSearchText = nameof(PurchaseOrderSearchText);
            public const string CompanyInvoice = nameof(CompanyInvoice);
            public const string IsSentToExternalSystem = nameof(IsSentToExternalSystem);
        }

        public static class Shipment
        {
            public const string CodFee = "CodFee";
            public const string WarehouseShippingFee = "WarehouseShippingFee";
            public const string TotalWeight = "TotalWeight";
            public const string ExpectedDeliveryDate = "ExpectedDeliveryDate";
            public const string EstimatedDeliveryDate = "EstimatedDeliveryDate";
            public const string ExpectedDeliveryDateRange = "ExpectedDeliveryDateRange";
            public const string IsSuburban = "IsSuburban";
            public const string ExpectedDeliveryTime = nameof(ExpectedDeliveryTime);
            public const string CollectOldAppliance = nameof(CollectOldAppliance);
            public const string ERPStatus = "ERPStatus";
            public const string ReturnShippingFee = "ReturnShippingFee";
        }

        public static class Payment
        {
            public const string PaymentHistories = nameof(PaymentHistories);
        }
    }
}