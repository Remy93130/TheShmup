using SDD.Events;
using UnityEngine;

public class BossPattern : Pattern
{
	public override float NextWaitDurationBeforeSpawn
	{
		get => 0;
	}

	private Vector3 SpawnWorldPos { get => Camera.main.ViewportToWorldPoint(new Vector3(1.025f, 0.5f, -Camera.main.transform.position.z)); }

	public override Enemy SpawnEnemy()
	{
		EventManager.Instance.Raise(new ChangeBackgroundEvent());
		GameObject enemyGO = Instantiate(m_EnemyPrefab);
		enemyGO.transform.position = SpawnWorldPos;
		Enemy enemy = enemyGO.GetComponent<Enemy>();
		return enemy;
	}
}
