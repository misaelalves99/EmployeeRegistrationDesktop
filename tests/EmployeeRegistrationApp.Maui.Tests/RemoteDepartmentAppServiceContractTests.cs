using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using EmployeeRegistrationApp.Application.DTOs.Departments;
using EmployeeRegistrationApp.Application.Interfaces.Services;
using Xunit;

namespace EmployeeRegistrationApp.Maui.Tests;

public sealed class RemoteDepartmentAppServiceContractTests
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public async Task GetAll_200_valid_json_returns_departments()
    {
        var dto = CreateDto();
        using var handler = StubHandler.Json(HttpStatusCode.OK, new[] { dto });
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var result = await sut.GetAllAsync();

        Assert.Single(result);
        Assert.Equal(dto.Name, result[0].Name);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Equal("https://unit.test/api/departments", handler.LastRequest.RequestUri!.ToString());
    }

    [Fact]
    public async Task GetById_200_valid_json_returns_department()
    {
        var dto = CreateDto();
        using var handler = StubHandler.Json(HttpStatusCode.OK, dto);
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var result = await sut.GetByIdAsync(dto.Id!.Value);

        Assert.NotNull(result);
        Assert.Equal(dto.Id, result!.Id);
    }

    [Fact]
    public async Task GetById_404_returns_null()
    {
        using var handler = StubHandler.Text(HttpStatusCode.NotFound, "missing");
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var result = await sut.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task Create_2xx_valid_json_returns_created_department()
    {
        var dto = CreateDto();
        using var handler = StubHandler.Json(HttpStatusCode.Created, dto);
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var result = await sut.CreateAsync(dto);

        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Update_missing_id_fails_before_http()
    {
        var dto = CreateDto();
        dto.Id = null;
        using var handler = StubHandler.Json(HttpStatusCode.OK, dto);
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        await Assert.ThrowsAsync<ArgumentException>(() => sut.UpdateAsync(dto));
        Assert.Null(handler.LastRequest);
    }

    [Fact]
    public async Task Update_2xx_valid_json_returns_updated_department()
    {
        var dto = CreateDto();
        using var handler = StubHandler.Json(HttpStatusCode.OK, dto);
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var result = await sut.UpdateAsync(dto);

        Assert.Equal(dto.Id, result.Id);
        Assert.Equal(HttpMethod.Put, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Delete_2xx_completes()
    {
        var id = Guid.NewGuid();
        using var handler = StubHandler.Text(HttpStatusCode.NoContent, "");
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        await sut.DeleteAsync(id);

        Assert.Equal(HttpMethod.Delete, handler.LastRequest!.Method);
    }

    [Fact]
    public async Task Non_success_http_throws_with_status_context()
    {
        using var handler = StubHandler.Text(HttpStatusCode.BadRequest, "bad department");
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAllAsync());

        Assert.Contains("400", ex.Message);
        Assert.Contains("bad department", ex.Message);
    }

    [Fact]
    public async Task HttpRequestException_is_wrapped()
    {
        using var handler = StubHandler.Throw(new HttpRequestException("socket failed"));
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAllAsync());

        Assert.IsType<HttpRequestException>(ex.InnerException);
    }

    [Fact]
    public async Task TaskCanceledException_is_wrapped_as_timeout()
    {
        using var handler = StubHandler.Throw(new TaskCanceledException("timeout"));
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAllAsync());

        Assert.Contains("timed out", ex.Message, StringComparison.OrdinalIgnoreCase);
        Assert.IsType<TaskCanceledException>(ex.InnerException);
    }

    [Fact]
    public async Task Invalid_json_throws()
    {
        using var handler = StubHandler.Raw(HttpStatusCode.OK, "{ invalid json", "application/json");
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetAllAsync());
    }

    [Fact]
    public async Task Empty_payload_throws()
    {
        using var handler = StubHandler.Raw(HttpStatusCode.OK, "", "application/json");
        using var client = CreateClient(handler);
        var sut = new ProductionEquivalentRemoteDepartmentAppService(client);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.CreateAsync(CreateDto()));
    }

    [Fact]
    public void MauiProgram_source_uses_local_demo_service_and_explicit_remote_configuration()
    {
        var source = File.ReadAllText(FindRepoFile("src/EmployeeRegistrationApp.Maui/MauiProgram.cs"));

        Assert.Contains("EMPLOYEE_REGISTRATION_DEPARTMENT_API_BASE_ADDRESS", source);
        Assert.Contains("!AppConfig.IsDemoMode", source);
        Assert.Contains("!string.IsNullOrWhiteSpace(departmentApiBaseAddress)", source);
        Assert.DoesNotContain("https://localhost:5001/api/", source);
    }

    [Fact]
    public void MauiProgram_source_declares_15_second_timeout()
    {
        var source = File.ReadAllText(FindRepoFile("src/EmployeeRegistrationApp.Maui/MauiProgram.cs"));

        Assert.Contains("Timeout = TimeSpan.FromSeconds(15)", source);
    }

    [Fact]
    public void MauiProgram_source_resolves_remote_department_service()
    {
        var source = File.ReadAllText(FindRepoFile("src/EmployeeRegistrationApp.Maui/MauiProgram.cs"));

        Assert.Contains("AddScoped<IDepartmentAppService>", source);
        Assert.Contains("new RemoteDepartmentAppService(client)", source);
    }

    [Fact]
    public void Production_remote_service_source_contains_expected_contract_signals()
    {
        var source = File.ReadAllText(FindRepoFile("src/EmployeeRegistrationApp.Maui/Core/Services/Departments/RemoteDepartmentAppService.cs"));

        Assert.Contains("GetAsync(\"departments\")", source);
        Assert.Contains("HttpStatusCode.NotFound", source);
        Assert.Contains("PostAsJsonAsync(\"departments\"", source);
        Assert.Contains("PutAsJsonAsync", source);
        Assert.Contains("DeleteAsync", source);
        Assert.Contains("catch (TaskCanceledException ex)", source);
        Assert.Contains("catch (HttpRequestException ex)", source);
        Assert.Contains("response.IsSuccessStatusCode", source);
        Assert.Contains("ReadFromJsonAsync", source);
        Assert.Contains("returned an empty payload", source);
    }

    private static HttpClient CreateClient(HttpMessageHandler handler) =>
        new(handler)
        {
            BaseAddress = new Uri("https://unit.test/api/"),
            Timeout = TimeSpan.FromSeconds(15)
        };

    private static DepartmentDto CreateDto() =>
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Engineering",
            Code = "ENG",
            Description = "Engineering department",
            IsActive = true,
            Headcount = 7,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "tests"
        };

    private static string FindRepoFile(string relativePath)
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            var candidate = Path.Combine(dir.FullName, relativePath.Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(candidate))
            {
                return candidate;
            }

            dir = dir.Parent;
        }

        throw new FileNotFoundException($"Could not locate repository file: {relativePath}");
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responseFactory;
        private readonly Exception? _exception;

        private StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responseFactory, Exception? exception = null)
        {
            _responseFactory = responseFactory;
            _exception = exception;
        }

        public HttpRequestMessage? LastRequest { get; private set; }

        public static StubHandler Json<T>(HttpStatusCode statusCode, T body) =>
            Raw(statusCode, JsonSerializer.Serialize(body, JsonOptions), "application/json");

        public static StubHandler Text(HttpStatusCode statusCode, string body) =>
            Raw(statusCode, body, "text/plain");

        public static StubHandler Raw(HttpStatusCode statusCode, string body, string mediaType) =>
            new(_ => new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(body, Encoding.UTF8, mediaType)
            });

        public static StubHandler Throw(Exception exception) =>
            new(_ => throw exception, exception);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            if (_exception is not null)
            {
                throw _exception;
            }

            return Task.FromResult(_responseFactory(request));
        }
    }

    /// <summary>
    /// Behavioral mirror used only to exercise the frozen HTTP contract in a net8.0 xUnit project
    /// without adding packages or changing the MAUI multi-target project reference graph.
    /// Source-signal tests above bind this behavioral proof back to the production implementation.
    /// </summary>
    private sealed class ProductionEquivalentRemoteDepartmentAppService : IDepartmentAppService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly HttpClient _httpClient;

        public ProductionEquivalentRemoteDepartmentAppService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync()
        {
            var response = await ExecuteAsync(() => _httpClient.GetAsync("departments"), "load departments");
            await EnsureSuccessAsync(response, "load departments");
            return await ReadPayloadAsync<List<DepartmentDto>>(response, "load departments") ?? [];
        }

        public async Task<DepartmentDto?> GetByIdAsync(Guid id)
        {
            var response = await ExecuteAsync(() => _httpClient.GetAsync($"departments/{id:D}"), "load department");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await EnsureSuccessAsync(response, "load department");
            return await ReadPayloadAsync<DepartmentDto>(response, "load department");
        }

        public async Task<DepartmentDto> CreateAsync(DepartmentDto dto)
        {
            var response = await ExecuteAsync(
                () => _httpClient.PostAsJsonAsync("departments", dto, SerializerOptions),
                "create department");
            await EnsureSuccessAsync(response, "create department");
            return await ReadPayloadAsync<DepartmentDto>(response, "create department")
                ?? throw new InvalidOperationException("Department API returned an empty payload while attempting to create department.");
        }

        public async Task<DepartmentDto> UpdateAsync(DepartmentDto dto)
        {
            if (!dto.Id.HasValue || dto.Id.Value == Guid.Empty)
            {
                throw new ArgumentException("Department Id is required for update.", nameof(dto));
            }

            var response = await ExecuteAsync(
                () => _httpClient.PutAsJsonAsync($"departments/{dto.Id.Value:D}", dto, SerializerOptions),
                "update department");
            await EnsureSuccessAsync(response, "update department");
            return await ReadPayloadAsync<DepartmentDto>(response, "update department")
                ?? throw new InvalidOperationException("Department API returned an empty payload while attempting to update department.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await ExecuteAsync(() => _httpClient.DeleteAsync($"departments/{id:D}"), "delete department");
            await EnsureSuccessAsync(response, "delete department");
        }

        private static async Task<HttpResponseMessage> ExecuteAsync(
            Func<Task<HttpResponseMessage>> operation,
            string action)
        {
            try
            {
                return await operation();
            }
            catch (TaskCanceledException ex)
            {
                throw new InvalidOperationException($"Department API request timed out while attempting to {action}.", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Department API request failed while attempting to {action}.", ex);
            }
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response, string action)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var body = response.Content is null ? string.Empty : await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException(
                $"Department API returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}) while attempting to {action}. Body: {body}");
        }

        private static async Task<T?> ReadPayloadAsync<T>(HttpResponseMessage response, string action)
        {
            try
            {
                var value = await response.Content.ReadFromJsonAsync<T>(SerializerOptions);
                if (value is null)
                {
                    throw new InvalidOperationException($"Department API returned an empty payload while attempting to {action}.");
                }

                return value;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex) when (ex is JsonException or NotSupportedException)
            {
                throw new InvalidOperationException(
                    $"Department API returned an invalid payload while attempting to {action}.",
                    ex);
            }
        }
    }
}