using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathPatternOneShot : Pattern
{
	[Header("Path Pattern One shot")]
	[SerializeField] private iTweenPath[] m_Paths;
	private iTweenPath m_CurrentPath;

	[SerializeField] private float m_MaxEnemySpawnPeriod;
	[SerializeField] private float m_MinEnemySpawnPeriod;
	[SerializeField] private AnimationCurve m_SpawnPeriodProbaCurve;
	
	public override float NextWaitDurationBeforeSpawn {
		get { return Mathf.Lerp(m_MinEnemySpawnPeriod, m_MaxEnemySpawnPeriod, m_SpawnPeriodProbaCurve.Evaluate(Random.value)); }
	}

	public override Enemy SpawnEnemy()
	{
		GameObject enemyGO = Instantiate(m_EnemyPrefab);
		int randomIndex = Random.Range(0, m_Paths.Length);
		m_CurrentPath = m_Paths[randomIndex];
		enemyGO.transform.position = m_CurrentPath.nodes[0];

		EnemyPathMvtOneShot enemy = enemyGO.GetComponent<EnemyPathMvtOneShot>();
		enemy.InitPath(m_CurrentPath);

		return enemy;
	}
}
