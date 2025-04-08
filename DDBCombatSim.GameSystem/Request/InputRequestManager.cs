namespace DDBCombatSim.GameSystem.Request;

using DDBCombatSim.GameSystem.Action.Context;
using DDBCombatSim.GameSystem.Battlefield.Area;
using DDBCombatSim.GameSystem.Request.RequestModels;
using DDBCombatSim.GameSystem.Request.ResponseModels;
using DDBCombatSim.GameSystem.Signal;
using System.Collections.Concurrent;

public class InputRequestManager
{
    private readonly string encounterId;
    private readonly CombatHub combatHub;
    private readonly ConcurrentDictionary<string, object> pendingRequests;

    public InputRequestManager(CombatHub combatHub, string encounterId)
    {
        this.combatHub = combatHub;
        pendingRequests = new();
        this.encounterId = encounterId;
    }

    public async Task<InputResponse<TResponse>?> GetUserInputAsync<TRequest, TResponse>(string encounterId, InputRequest<TRequest> inputRequest, CancellationToken cancellationToken)
    {
        await combatHub.SendInputRequest(encounterId, inputRequest, cancellationToken);

        var tcs = new TaskCompletionSource<InputResponse<TResponse>?>();
        pendingRequests[inputRequest.RequestId] = tcs;

        try
        {
            using (cancellationToken.Register(() => tcs.TrySetCanceled()))
            {
                return await tcs.Task;
            }
        }
        finally
        {
            pendingRequests.TryRemove(inputRequest.RequestId, out _);
        }
    }

    public void CancelRequest<TResponse>(string requestId)
    {
        if (pendingRequests.TryRemove(requestId, out var tcs))
        {
            if (tcs is TaskCompletionSource<InputResponse<TResponse>?> taskCompletionSource)
            {
                taskCompletionSource.SetResult(null);
            }
            else
            {
                throw new InvalidOperationException("Request type does not match response type");
            }
        }
    }

    public void HandleResponse<TResponse>(InputResponse<TResponse> response)
    {
        if (pendingRequests.TryRemove(response.RequestId, out var tcs))
        {
            if (tcs is TaskCompletionSource<InputResponse<TResponse>> taskCompletionSource)
            {
                taskCompletionSource.SetResult(response);
            }
            else
            {
                throw new InvalidOperationException("Request type does not match response type");
            }
        }
    }

    public async Task<TResponse?> GetUserInputAsync<TRequest, TResponse>(InputRequest<TRequest> inputRequest, CancellationToken cancellationToken) where TResponse : class
    {
        var response = await GetUserInputAsync<TRequest, TResponse>(encounterId, inputRequest, cancellationToken);

        return response == null ? null : response.Data;
    }

    public async Task<RollResponse?> GetAttackRollResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.AttackRoll,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetDamageRollResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.DamageRoll,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetHealRollResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.HealRoll,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetDeathSavingThrowResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.DeathSavingThrow,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetSavingThrowResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.SavingThrow,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetSkillCheckRollResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.SkillCheckRoll,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetAbilityCheckRollResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.AbilityCheckRoll,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<RollResponse?> GetInitiativeRollResult(string combatantId, RollRequest rollRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<RollRequest>
        {
            RequestType = EInputRequestType.InitiativeRoll,
            Data = rollRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<RollRequest, RollResponse>(inputRequest, cancellationToken);
    }

    public async Task<TargetResponse?> GetTargets(string combatantId, TargetRequest targetRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<TargetRequest>
        {
            RequestType = EInputRequestType.Targetting,
            Data = targetRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<TargetRequest, TargetResponse>(inputRequest, cancellationToken);
    }

    public async Task<TargetResponse?> GetTargetSelection(string combatantId, TargetSelectionRequest targetSelectionRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<TargetSelectionRequest>
        {
            RequestType = EInputRequestType.TargetSelection,
            Data = targetSelectionRequest,
            CombatantId = combatantId
        };
        return await GetUserInputAsync<TargetSelectionRequest, TargetResponse>(inputRequest, cancellationToken);
    }

    public async Task<ApprovalResponse?> SendApprovalRequestAsync(string combatantId, ApprovalRequest approvalRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<ApprovalRequest>
        {
            RequestType = EInputRequestType.Approval,
            Data = approvalRequest,
            CombatantId = combatantId
        };
        var response = await GetUserInputAsync<ApprovalRequest, ApprovalResponse>(inputRequest, cancellationToken);
        return response;
    }

    public async Task<string?> SendOptionRequest(string combatantId, OptionRequest optionRequest, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<OptionRequest>
        {
            RequestType = EInputRequestType.Option,
            Data = optionRequest,
            CombatantId = combatantId
        };
        var response = await GetUserInputAsync<OptionRequest, string>(inputRequest, cancellationToken);
        return response;
    }

    public async Task<IArea?> SendAreaSelectionRequest(string combatantId, AreaSelectionContext ctx, CancellationToken cancellationToken)
    {
        var inputRequest = new InputRequest<AreaSelectionContext>
        {
            RequestType = EInputRequestType.Option,
            Data = ctx,
            CombatantId = combatantId
        };
        var response = await GetUserInputAsync<AreaSelectionContext, IArea>(inputRequest, cancellationToken);
        return response;
    }
}
