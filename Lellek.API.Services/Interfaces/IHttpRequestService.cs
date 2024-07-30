using System.Net.Http;
using System.Threading.Tasks;

namespace PlanB.PDF.Manager.Service.Interfaces;

public interface IHttpRequestService
{
    Task<dynamic> GetContent(HttpMethod method, string url);

    void SetHeader(string name, string value);

    Task<dynamic> GetContent(HttpMethod method, string url, HttpContent httpContent);

    Task<dynamic> GetAudio(HttpMethod method, string url, HttpContent httpContent);
}

