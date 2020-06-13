using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBeginnerPattern : Pattern
{
	public override float NextWaitDurationBeforeSpawn
	{
		get { return 0; }
	}

	private Vector3 SpawnWorldPos { get { return Camera.main.ViewportToWorldPoint(new Vector3(1.025f, 0.5f, -Camera.main.transform.position.z)); } }

	public override Enemy SpawnEnemy()
	{
		GameObject enemyGO = Instantiate(m_EnemyPrefab);

		enemyGO.transform.position = SpawnWorldPos;

		BossBeginner enemy = enemyGO.GetComponent<BossBeginner>();
		return enemy;
	}
}
