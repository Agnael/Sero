
namespace Sero.Core
{
    public interface IUserAgentParsingService
    {
        UserAgentOverview Parse(string userAgentHeaderValue);
    }
}
