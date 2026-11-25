using System;
using System.Net;
using System.Net.Http;

namespace Library.Client.Services;

public abstract class BaseApiService
{
    protected const string BaseUrl = "http://localhost:5063";
    private static readonly CookieContainer CookieContainer = new();

    protected static readonly HttpClient Http = new(new HttpClientHandler
    {
        CookieContainer = CookieContainer,
        UseCookies = true
    })
    {
        BaseAddress = new Uri(BaseUrl)
    };

    // Вспомогательный метод для очистки кук при выходе
    protected static void ClearCookies()
    {
        foreach (Cookie cookie in CookieContainer.GetCookies(new Uri(BaseUrl)))
        {
            cookie.Expired = true;
        }
    }
}