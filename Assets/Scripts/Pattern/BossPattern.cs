using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossPattern : Pattern
{
	public override float NextWaitDurationBeforeSpawn
	{
		get { return 0; }
	}

	private Vector3 SpawnWorldPos { get { return Camera.main.ViewportToWorldPoint(new Vector3(1.025f, 0.5f, -Camera.main.transform.position.z)); } }

	public override Enemy SpawnEnemy()
	{
		EventManager.Instance.Raise(new ChangeBackgroundEvent());
		GameObject enemyGO = Instantiate(m_EnemyPrefab);

		enemyGO.transform.position = SpawnWorldPos;

		Enemy enemy = enemyGO.GetComponent<Enemy>();
		return enemy;
	}
}
