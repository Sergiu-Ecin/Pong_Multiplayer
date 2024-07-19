using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PongMultiplayerManager : NetworkManager
{
    public static PongMultiplayerManager Instance { get; private set; }

    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;
    public Transform ballSpawnPoint;
    private GameObject ball;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient connection)
    {
        Transform spawnPoint = numPlayers == 0 ? leftSpawnPoint : rightSpawnPoint;
        GameObject newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.AddPlayerForConnection(connection, newPlayer);

        if (numPlayers == 2)
        {
            SpawnBall();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient connection)
    {
        if (ball != null)
            NetworkServer.Destroy(ball);

        base.OnServerDisconnect(connection);
    }

    public void RespawnBallAndPlayers()
    {
        SpawnBall();

        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.identity != null)
            {
                Transform spawnPoint = conn.identity.GetComponent<PongPlayer>().isLocalPlayer ? leftSpawnPoint : rightSpawnPoint;
                conn.identity.transform.position = spawnPoint.position;
                conn.identity.transform.rotation = spawnPoint.rotation;
            }
        }
    }

    private void SpawnBall()
    {
        ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"), ballSpawnPoint.position, ballSpawnPoint.rotation);
        NetworkServer.Spawn(ball);
    }
}