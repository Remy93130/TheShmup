using UnityEngine;

public class BossIntermediateDifficult: Boss {

	[SerializeField] private GameObject m_EnemyPrefab;

	public override void ShootBullet()
	{
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 1.2f, 0), new Quaternion(0, 0, -0.08f, 1));
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 1.2f, 0), new Quaternion(0, 0, 0.08f, 1));
		if (Random.Range(0f, 2f) <= 1)
		{
			float y = UnityEngine.Random.Range(-4f, 4f);
			float x = m_BulletSpawnPoint.position.x;
			Instantiate(m_EnemyPrefab, new Vector3(x, y, 0), new Quaternion(0, 90, 0, 1));
		}

	}
}
