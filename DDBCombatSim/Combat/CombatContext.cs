namespace DDBCombatSim.Combat;

using DDBCombatSim.Battlefield;
using DDBCombatSim.Combatant;
using DDBCombatSim.Effect;
using DDBCombatSim.Request;
using DDBCombatSim.Request.RequestModels;
using DDBCombatSim.Request.ResponseModels;
using DDBCombatSim.Signal;

public class CombatContext
{
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public CombatContext(CombatHub combatHub, string encounterId, List<ICombatant> combatants)
    {
        EncounterId = encounterId;
        InputRequestManager = new InputRequestManager(combatHub, encounterId);
        EffectManager = new EffectManager(InputRequestManager);
        Combatants = combatants;
        Round = 1;
        Turn = 0;
        Battlefield = new(20, 20);
    }

    public string EncounterId { get; }

    public int Round { get; set; }

    public int Turn { get; set; }

    public List<ICombatant> Combatants { get; set; }

    public InputRequestManager InputRequestManager { get; }

    public EffectManager EffectManager { get; set; }

    public Battlefield Battlefield { get; set; }

    public CancellationToken CancellationToken => cancellationTokenSource.Token;

    public void CancelCombat()
    {
        cancellationTokenSource.Cancel();
    }

    public async Task HandleTurnActionsAsync(string combatantId)
    {
        EInputResponseType lastResponseType;

        do
        {
            // TODO: Create a new InputRequest for the current combatant available actions

            var inputRequest = new InputRequest<TurnActionRequest>
            {
                RequestType = EInputRequestType.TurnAction,
                Data = new TurnActionRequest(),
                CombatantId = combatantId
            };

            var response = await InputRequestManager.GetUserInputAsync<TurnActionRequest, TurnActionResponse>(EncounterId, inputRequest, CancellationToken);
            
            if (response == null)
            {
                response = new InputResponse<TurnActionResponse>
                {
                    ResponseType = EInputResponseType.EndTurn,
                    Data = new TurnActionResponse()
                };
            }

            switch (response.ResponseType)
            {
                case EInputResponseType.EndTurn:
                    break;
                case EInputResponseType.Movement:
                    // TODO: Handle movement
                    break;
                case EInputResponseType.CombatAction:
                    // TODO: Handle combat action
                    break;
                default:
                    // TODO: Handle invalid response
                    break;
            }

            lastResponseType = response.ResponseType;
        } while (lastResponseType != EInputResponseType.EndTurn);
    }
}
