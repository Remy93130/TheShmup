using UnityEngine;

public class ITweenPathPattern : Pattern
{
	[Header("ITween Path Pattern")]
	[SerializeField] private iTweenPath[] m_Paths;
	private iTweenPath m_CurrentPath;

	[SerializeField] private float m_MaxEnemySpawnPeriod;
	[SerializeField] private float m_MinEnemySpawnPeriod;
	[SerializeField] private AnimationCurve m_SpawnPeriodProbaCurve;
	
	public override float NextWaitDurationBeforeSpawn
	{
		get => Mathf.Lerp(m_MinEnemySpawnPeriod, m_MaxEnemySpawnPeriod, m_SpawnPeriodProbaCurve.Evaluate(Random.value));
	}

	public override Enemy SpawnEnemy()
	{
		GameObject enemyGO = Instantiate(m_EnemyPrefab);
		m_CurrentPath = m_Paths[Random.Range(0, m_Paths.Length)];
		enemyGO.transform.position = m_CurrentPath.nodes[0];
		EnemyITweenPathMvt enemy = enemyGO.GetComponent<EnemyITweenPathMvt>();
		enemy.InitPath(m_CurrentPath);
		return enemy;
	}
}
