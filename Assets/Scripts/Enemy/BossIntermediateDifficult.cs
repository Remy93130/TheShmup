using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntermediateDifficult: Boss {

	[SerializeField] private GameObject m_EnemyPrefab;

	public override void ShootBullet()
	{
		GameObject bulletGO1 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		GameObject bulletGO2 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		GameObject bulletGO3 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
		GameObject bulletGO4 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 1.2f, 0), new Quaternion(0, 0, -0.08f, 1));
		GameObject bulletGO5 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 1.2f, 0), new Quaternion(0, 0, 0.08f, 1));
		float probaJunior = UnityEngine.Random.Range(0f, 2f);
		Debug.Log("proba : " + probaJunior);
		if (probaJunior <= 1)
		{
			float y = UnityEngine.Random.Range(-4f,4f);
			float x = m_BulletSpawnPoint.position.x;
			GameObject enemyGO = Instantiate(m_EnemyPrefab, new Vector3(x,y,0), new Quaternion(0,90,0,1));
		}

	}
}
