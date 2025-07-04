using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Shiny.Mediator;
using SharedModels.Dtos.Contacts;
using UnoApp.Mediator.Requests.Contacts;

namespace UnoApp.Mediator.Handlers.Contacts;

[SingletonHandler]
public class GetAllContactsRequestHandler : IRequestHandler<GetAllContactsRequest, IEnumerable<ContactDto>>
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public GetAllContactsRequestHandler(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        try
        {
            var apiUrl = configuration["ApiClient:Url"];
            if (string.IsNullOrEmpty(apiUrl))
            {
                // Return sample data when API is not configured
                return GetSampleContacts();
            }

            var response = await httpClient.GetAsync($"{apiUrl}/contacts/", cancellationToken);
            response.EnsureSuccessStatusCode();

            var contacts = await response.Content.ReadFromJsonAsync<IEnumerable<ContactDto>>(cancellationToken);
            return contacts ?? new List<ContactDto>();
        }
        catch (Exception ex)
        {
            // In a real application, you'd want to log this error
            Console.WriteLine($"Error fetching contacts: {ex.Message}");
            // Return sample data on error for demo purposes
            return GetSampleContacts();
        }
    }

    private static IEnumerable<ContactDto> GetSampleContacts()
    {
        var contacts = new List<ContactDto>
        {
            new("wix-001", "John Smith", "john.smith@example.com", "+1 555-0123", "Technology"),
            new("wix-002", "Sarah Johnson", "sarah.johnson@example.com", "+1 555-0124", "Marketing"),
            new("wix-003", "Michael Brown", "michael.brown@example.com", "+1 555-0125", "Sales"),
            new("wix-004", "Emily Davis", "emily.davis@example.com", "+1 555-0126", "Finance"),
            new("wix-005", "David Wilson", "david.wilson@example.com", "+1 555-0127", "Operations")
        };

        // Set IDs for each contact since records are immutable
        return contacts.Select(contact => contact with { Id = Guid.NewGuid() });
    }
}