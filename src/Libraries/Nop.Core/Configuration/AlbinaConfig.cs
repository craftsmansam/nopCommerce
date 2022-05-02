namespace Nop.Core.Configuration
{
    /// <summary>
    /// Represents startup hosting configuration parameters
    /// </summary>
    public class AlbinaConfig
    {
        public string EmailTest { get; set; }
        public string JobInquirySubject { get; set; }
		public string InfoToAddress { get; set; }
		public string JobInquiryToAddress { get; set; }
		public string ComponentPartRequestToAddress { get; set; }
		public string InfoFromAddress { get; set; }
		public string NagiosTestAddress { get; set; }
		public string SpiralMathReportCachelMathReportCacheKey { get; set; }
		public string ErrorToAddress { get; set; }
		public string ErrorFromAddress { get; set; }
		public string ErrorSubject { get; set; }
		public bool OnDevelopmentServer { get; set; }
        public string SpiralMathReportCache {get; set; }
		public int UploadDocumentMaxSize { get; set; }
    }
}