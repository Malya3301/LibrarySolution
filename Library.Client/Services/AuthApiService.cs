using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Library.Data.DTOs;
using Library.Data.Responses;

namespace Library.Client.Services;

public class AuthApiService : BaseApiService
{
    public async Task<ApiResponse<string>> LoginAsync(LoginDto model)
    {
        try
        {
            var response = await Http.PostAsJsonAsync("auth/login", model);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            return result ?? new ApiResponse<string>
            {
                Status = false,
                Data = "Пустой ответ от сервера"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                Status = false,
                Data = $"Ошибка подключения: {ex.Message}"
            };
        }
    }

    public async Task<ApiResponse<string>> RegisterAsync(RegisterDto model)
    {
        try
        {
            var response = await Http.PostAsJsonAsync("auth/register", model);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            return result ?? new ApiResponse<string>
            {
                Status = false,
                Data = "Пустой ответ от сервера"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                Status = false,
                Data = $"Ошибка подключения: {ex.Message}"
            };
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await Http.PostAsync("auth/logout", null);
        }
        finally
        {
            // Очищаем локальные куки в любом случае, даже если сервер временно недоступен
            ClearCookies();
        }
    }
}