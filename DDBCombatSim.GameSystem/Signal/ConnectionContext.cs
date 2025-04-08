namespace DDBCombatSim.GameSystem.Signal;

using System.Collections.Concurrent;

public class ConnectionContext
{
    private const string DmKey = "dm";

    private readonly ConcurrentDictionary<string, ConnectionRecord> connections = new();
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, ConnectionRecord>> encounterConnections = new();

    public void AddConnection(ConnectionRecord connection)
    {
        if (connection.IsDm && connection.CharacterId != null)
        {
            throw new InvalidOperationException("DM cannot have a character ID");
        }

        if (!connection.IsDm && connection.CharacterId == null)
        {
            throw new InvalidOperationException("Character must have a character ID");
        }

        var connectionsForEncounter = encounterConnections.GetOrAdd(connection.EncounterId, _ => new ConcurrentDictionary<string, ConnectionRecord>());

        lock (connectionsForEncounter)
        {
            if (connections.ContainsKey(connection.ConnectionId))
            {
                throw new InvalidOperationException("Connection already exists");
            }

            if (connection.IsDm && connectionsForEncounter.ContainsKey(DmKey))
            {
                throw new InvalidOperationException("Encounter already has a DM");
            }

            if (!connection.IsDm && connectionsForEncounter.ContainsKey(connection.CharacterId ?? string.Empty))
            {
                throw new InvalidOperationException("Character already connected to encounter");
            }

            connections[connection.ConnectionId] = connection;
            connectionsForEncounter[connection.CharacterId ?? DmKey] = connection;
        }
    }

    public ConnectionRecord? RemoveConnection(string connectionId)
    {
        if (connections.TryRemove(connectionId, out var connection))
        {
            if (encounterConnections.TryGetValue(connection.EncounterId, out var connectionsForEncounter))
            {
                lock (connectionsForEncounter)
                {
                    connectionsForEncounter.TryRemove(connection.CharacterId ?? DmKey, out _);

                    if (connectionsForEncounter.IsEmpty)
                    {
                        encounterConnections.TryRemove(connection.EncounterId, out _);
                    }
                }
            }
        }

        return connection;
    }

    public string? GetConnectionIdByCharacterId(string encounterId, string characterId)
    {
        if (encounterConnections.TryGetValue(encounterId, out var connections))
        {
            return connections.TryGetValue(characterId, out var connection) ? connection.ConnectionId : null;
        }

        return null;
    }

    public string? GetDmConnectionId(string encounterId)
    {
        if (encounterConnections.TryGetValue(encounterId, out var connections))
        {
            return connections.TryGetValue(DmKey, out var connection) ? connection.ConnectionId : null;
        }

        return null;
    }
}