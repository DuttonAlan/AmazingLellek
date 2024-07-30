using Newtonsoft.Json;
using PlanB.PDF.Manager.Service.Interfaces;

namespace PlanB.PDF.Manager.Service.Services;

public class HttpRequestService : IHttpRequestService
{
    private HttpClient client;

    public HttpRequestService(HttpClient client) 
    {
        this.client = client;
    }
    public void SetHeader(string name, string value)
    {
        client.DefaultRequestHeaders.Remove(name);
        client.DefaultRequestHeaders.Add(name, value);
    }

    public async Task<dynamic> GetContent(HttpMethod method, string url) 
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            HttpResponseMessage response = await this.client.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(content)!;
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Fehler beim Senden der Anforderung: {ex}");
            return ex;
        }
    }
    public async Task<dynamic> GetContent(HttpMethod method, string url, HttpContent httpContent)
    {
        try
        {
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            request.Content= httpContent;
            HttpResponseMessage response = await this.client.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject(content)!;
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Fehler beim Senden der Anforderung: {ex}");
            return ex;
        }
    }

    public async Task<dynamic> GetAudio(HttpMethod method, string url, HttpContent httpContent)
    {
        HttpRequestMessage request = new HttpRequestMessage(method, url);
        request.Content = httpContent;
        HttpResponseMessage response = await this.client.SendAsync(request);

        //var content = await response.Content.ReadAsStreamAsync();
        return response;
        
    }
}
