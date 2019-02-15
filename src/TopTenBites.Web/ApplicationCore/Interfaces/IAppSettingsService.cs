namespace TopTenBites.Web.ApplicationCore.Interfaces
{
    public interface IAppSettingsService
    {
        string YelpApiKey { get; }
        string GoogleMapsApiKey { get; }
        string UploadsPath { get; }
        string UploadsVirtualDirectory { get; }
        string Error404ViewPath { get; set; }
        string Error500ViewPath { get; set; }
    }
}
