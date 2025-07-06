/// <summary>
/// This file shows what the MediatorHttp source generator should create
/// based on the WebApi.json OpenAPI specification
/// </summary>
namespace UnoApp.ApiClient;

// This should be generated for the GET /contacts endpoint with operationId: "GetAll"
[global::Shiny.Mediator.Http.HttpAttribute(global::Shiny.Mediator.Http.HttpVerb.Get, "/contacts")]
public partial class GetAllHttpRequest : global::Shiny.Mediator.Http.IHttpRequest<ContactDto[]>
{
    // No parameters for this endpoint
}

// This should be generated for the POST /contacts endpoint with operationId: "Add"
[global::Shiny.Mediator.Http.HttpAttribute(global::Shiny.Mediator.Http.HttpVerb.Post, "/contacts")]
public partial class AddHttpRequest : global::Shiny.Mediator.Http.IHttpRequest<ContactDto>
{
    // Request body would be handled differently
    public AddContactRequest Body { get; set; }
}

// This should be generated for the GET /contacts/{Id} endpoint with operationId: "Get"
[global::Shiny.Mediator.Http.HttpAttribute(global::Shiny.Mediator.Http.HttpVerb.Get, "/contacts/{Id}")]
public partial class GetHttpRequest : global::Shiny.Mediator.Http.IHttpRequest<ContactDto>
{
    [global::Shiny.Mediator.Http.HttpParameterAttribute(global::Shiny.Mediator.Http.HttpParameterType.Path, "Id")]
    public Guid Id { get; set; }
}

// This should be generated for the DELETE /contacts/{Id} endpoint with operationId: "Delete"
[global::Shiny.Mediator.Http.HttpAttribute(global::Shiny.Mediator.Http.HttpVerb.Delete, "/contacts/{Id}")]
public partial class DeleteHttpRequest : global::Shiny.Mediator.Http.IHttpRequest<bool>
{
    [global::Shiny.Mediator.Http.HttpParameterAttribute(global::Shiny.Mediator.Http.HttpParameterType.Path, "Id")]
    public Guid Id { get; set; }
}

// These models should also be generated from the components/schemas section
public class ContactDto
{
    public string WixId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Branche { get; set; }
    public Guid? Id { get; set; }
}

public class AddContactRequest
{
    public string WixId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Branche { get; set; }
}