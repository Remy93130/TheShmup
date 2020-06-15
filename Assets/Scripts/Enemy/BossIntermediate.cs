using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntermediate: Boss {

	public override void ShootBullet()
	{
		GameObject bulletGO1 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		GameObject bulletGO2 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		GameObject bulletGO3 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
		GameObject bulletGO4 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 1.2f, 0), new Quaternion(0, 0, -0.08f, 1));
		GameObject bulletGO5 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 1.2f, 0), new Quaternion(0, 0, 0.08f, 1));
		/*int probaJunior = UnityEngine.Random.Range(0, 6);
		if (probaJunior == 0)
		{
			TODO
			Code pour faire apparaitre un nouveau vaisseau Exemple : un vaisseau qui tire un seul missile
		}*/

	}
}
