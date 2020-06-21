using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeginner: Boss {

	public override void ShootBullet()
	{
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 1.2f, 0), new Quaternion(0, 0, -0.08f, 1));
		Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 1.2f, 0), new Quaternion(0, 0, 0.08f, 1));
	}
}
