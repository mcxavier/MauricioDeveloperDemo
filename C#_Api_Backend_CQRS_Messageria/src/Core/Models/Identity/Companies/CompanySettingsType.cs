using Core.SeedWork;

namespace Core.Models.Identity.Companies
{
    public class CompanySettingsType : Enumeration
    {
        public CompanySettingsType(int id, string name) : base(id, name) { }
        public static CompanySettingsType LinxIOConfig = new CompanySettingsType(1, "LinxIOConfig");
        public static CompanySettingsType ReshopConfig = new CompanySettingsType(2, "ReshopConfig");
        public static CompanySettingsType SalesConfig = new CompanySettingsType(3, "SalesConfig");
        public static CompanySettingsType LinxCepConfig = new CompanySettingsType(4, "LinxCepConfig");
        public static CompanySettingsType VTexClientConfig = new CompanySettingsType(5, "VTexClientConfig");
        public static CompanySettingsType LinxUxEnvironmentConfig = new CompanySettingsType(6, "LinxUxEnvironmentConfig");
        public static CompanySettingsType PayHubClientConfig = new CompanySettingsType(7, "PayHubClientConfig");
        public static CompanySettingsType PagarMeClientConfig = new CompanySettingsType(8, "PagarMeClientConfig");
        public static CompanySettingsType TwilioConfig = new CompanySettingsType(9, "TwilioConfig");
        public static CompanySettingsType SmtpConfig = new CompanySettingsType(10, "SmtpConfig");
        public static CompanySettingsType InstrumentationKeyConfig = new CompanySettingsType(11, "InstrumentationKeyConfig");
    }

    public enum CompanySettingsTypeEnum
    {
        LinxIOConfig = 1,
        ReshopConfig = 2,
        SalesConfig = 3,
        LinxCepConfig = 4,
        VTexClientConfig = 5,
        LinxUxEnvironmentConfig = 6,
        PayHubClientConfig = 7,
        PagarMeClientConfig = 8,
        TwilioConfig = 9,
        SmtpConfig = 10,
        InstrumentationKeyConfig = 11
    }
}