namespace DDBCombatSim.GameSystem.Signal;

using DDBCombatSim.GameSystem.Action;
using DDBCombatSim.GameSystem.Request;
using Microsoft.AspNetCore.SignalR;

public class CombatHub : Hub
{
    private readonly ConnectionContext connectionContext = new();

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext == null)
        {
            await Clients.Caller.SendAsync("ConnectionError", "Unable to get HttpContext");
            Context.Abort();
            return;
        }

        string? encounterId = httpContext.Request.Query["encounterId"];

        if (encounterId == null)
        {
            await Clients.Caller.SendAsync("ConnectionError", "Encounter ID is required");
            Context.Abort();
            return;
        }

        string? characterId = httpContext.Request.Query["characterId"];
        bool isDm = string.IsNullOrEmpty(characterId);

        var connection = new ConnectionRecord
        {
            ConnectionId = Context.ConnectionId,
            EncounterId = encounterId,
            CharacterId = characterId,
            IsDm = isDm
        };

        try
        {
            connectionContext.AddConnection(connection);
        }
        catch (InvalidOperationException ex)
        {
            await Clients.Caller.SendAsync("ConnectionError", ex.Message);
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, encounterId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connection = connectionContext.RemoveConnection(Context.ConnectionId);

        if (connection != null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.EncounterId);
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendInputRequest<TRequest>(string encounterId, InputRequest<TRequest> inputRequest, CancellationToken cancellationToken)
    {
        var connectionId = (connectionContext.GetConnectionIdByCharacterId(encounterId, inputRequest.CombatantId) 
            ?? connectionContext.GetDmConnectionId(encounterId)) 
            ?? throw new InvalidOperationException("Actor not connected to encounter");

        await Clients.Client(connectionId).SendAsync("InputRequest", inputRequest, cancellationToken);
    }
}
