using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Application.Interfaces.Services;

namespace EmployeeRegistrationApp.Maui.Core.Services.Departments;

public sealed class RemoteDepartmentAppService : IDepartmentAppService
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient _httpClient;

    public RemoteDepartmentAppService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync()
    {
        using var response = await SendAsync(
            () => _httpClient.GetAsync("departments"),
            "load departments");

        return await DeserializeAsync<List<DepartmentDto>>(response, "load departments");
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id)
    {
        using var response = await SendAsync(
            () => _httpClient.GetAsync($"departments/{id:D}"),
            "load department");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        return await DeserializeAsync<DepartmentDto>(response, "load department");
    }

    public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        using var response = await SendAsync(
            () => _httpClient.PostAsJsonAsync("departments", dto, SerializerOptions),
            "create department");

        return await DeserializeAsync<DepartmentDto>(response, "create department");
    }

    public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (dto.Id is null || dto.Id == Guid.Empty)
        {
            throw new ArgumentException("Department id is required for update.", nameof(dto));
        }

        using var response = await SendAsync(
            () => _httpClient.PutAsJsonAsync($"departments/{dto.Id.Value:D}", dto, SerializerOptions),
            "update department");

        return await DeserializeAsync<DepartmentDto>(response, "update department");
    }

    public async Task DeleteAsync(Guid id)
    {
        using var response = await SendAsync(
            () => _httpClient.DeleteAsync($"departments/{id:D}"),
            "delete department");

        await EnsureSuccessAsync(response, "delete department");
    }

    private static async Task<HttpResponseMessage> SendAsync(
        Func<Task<HttpResponseMessage>> send,
        string operation)
    {
        try
        {
            return await send();
        }
        catch (TaskCanceledException ex)
        {
            throw new InvalidOperationException(
                $"Department API timed out while attempting to {operation}.",
                ex);
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException(
                $"Department API could not {operation}.",
                ex);
        }
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response, string operation)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var detail = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(
            $"Department API failed to {operation}. HTTP {(int)response.StatusCode} ({response.ReasonPhrase}). Response: {detail}");
    }

    private static async Task<T> DeserializeAsync<T>(
        HttpResponseMessage response,
        string operation)
    {
        await EnsureSuccessAsync(response, operation);

        try
        {
            var payload = await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
            return payload ?? throw new InvalidOperationException(
                $"Department API returned an empty payload while attempting to {operation}.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException(
                $"Department API returned invalid JSON while attempting to {operation}.",
                ex);
        }
        catch (NotSupportedException ex)
        {
            throw new InvalidOperationException(
                $"Department API returned an unsupported payload while attempting to {operation}.",
                ex);
        }
    }
}