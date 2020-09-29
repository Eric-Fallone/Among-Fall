using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerSpawnSystem : NetworkBehaviour
{
	[SerializeField] private GameObject playerPrefab = null;

	private static List<Transform> spawnPoints = new List<Transform>();
	private int nextSpawnIndex = 0;

	public static void AddSpawnPoint( Transform transformIn )
	{
		spawnPoints.Add(transformIn);

		spawnPoints = spawnPoints.OrderBy( x => x.GetSiblingIndex()).ToList();
	}

	public static void RemoveSpawnPoint(Transform transformIn)
	{
		spawnPoints.Remove(transformIn);
	}

	public override void OnStartServer()
	{
		NetworkManagerAmongFall.OnServerReadied += SpawnPlayer;
	}

	[Server]
	public void SpawnPlayer(NetworkConnection conn)
	{
		Transform spawnPont = spawnPoints.ElementAtOrDefault(nextSpawnIndex);

		if(spawnPont == null)
		{
			Debug.LogError($"Missing spawn point for player {nextSpawnIndex}");
			return;
		}

		GameObject playerInstance = Instantiate(playerPrefab, spawnPoints[nextSpawnIndex].position, spawnPoints[nextSpawnIndex].rotation);
		
		NetworkServer.Spawn(playerInstance, conn);

		nextSpawnIndex++;
	}

}
