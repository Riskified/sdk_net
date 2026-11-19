namespace Riskified.SDK.Model.OrderElements
{
    /// <summary>
    /// The documented values for <see cref="OrderBase.Source"/>.
    /// </summary>
    public static class OrderSource
    {
        public const string DesktopWeb = "desktop_web";
        public const string MobileWeb = "mobile_web";
        public const string MobileApp = "mobile_app";
        public const string MobileAppAndroid = "mobile_app_android";
        public const string MobileAppIos = "mobile_app_ios";
        public const string Web = "web";
        public const string Chat = "chat";
        public const string ThirdParty = "third_party";
        public const string Phone = "phone";
        public const string InStore = "in_store";
        public const string ShopifyDraftOrder = "shopify_draft_order";
        public const string Unknown = "unknown";
        public const string Subscription = "subscription";
        public const string AiAgent = "ai_agent";
    }
}
