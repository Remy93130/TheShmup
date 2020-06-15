using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathMvtThreeShot: EnemyITweenPathMvt{

	public override void ShootBullet()
	{
		GameObject bulletGO1 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		GameObject bulletGO2 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		GameObject bulletGO3 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
	}
}
