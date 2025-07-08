using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace WebApi.Helpers.Authentication;

public class IdentityOperationIdTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        foreach (var path in document.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                if (operation.Value.OperationId == null)
                {
                    // Generate operationId based on path and HTTP method
                    var operationId = GenerateOperationId(path.Key, operation.Key);
                    if (!string.IsNullOrEmpty(operationId))
                    {
                        operation.Value.OperationId = operationId;
                    }
                }
            }
        }

        return Task.CompletedTask;
    }

    private string? GenerateOperationId(string path, OperationType method)
    {
        // Map Identity endpoints to meaningful operation IDs
        return (path.ToLower(), method) switch
        {
            ("/register", OperationType.Post) => "Register",
            ("/login", OperationType.Post) => "Login",
            ("/refresh", OperationType.Post) => "Refresh",
            ("/confirmemail", OperationType.Get) => "ConfirmEmail",
            ("/resendconfirmationemail", OperationType.Post) => "ResendConfirmationEmail",
            ("/forgotpassword", OperationType.Post) => "ForgotPassword",
            ("/resetpassword", OperationType.Post) => "ResetPassword",
            ("/manage/2fa", OperationType.Post) => "ManageTwoFactor",
            ("/manage/info", OperationType.Get) => "GetManageInfo",
            ("/manage/info", OperationType.Post) => "PostManageInfo",
            ("/api/auth/login", OperationType.Post) => "ExtendedLogin",
            ("/api/auth/refresh", OperationType.Post) => "ExtendedRefresh",
            _ => null
        };
    }
}