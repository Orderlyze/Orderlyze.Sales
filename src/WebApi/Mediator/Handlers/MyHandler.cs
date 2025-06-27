using Shiny.Mediator;
using WebApi.Mediator.Requests;

namespace WebApi.Mediator.Handlers
{
    [MediatorHttpGroup("/routes", RequiresAuthorization = true)]
    public class MyHandler : IRequestHandler<MyRequest, MyResult>
    {
        [MediatorHttpPost("MyOperation", "/my")] // creates a route with the operationId of "MyOperation" and a route of "/routes/my"
        public async Task<MyResult> Handle(
            MyRequest request,
            IMediatorContext context,
            CancellationToken ct
        )
        {
            return new MyResult();
        }
    }
}
