using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathMvtFiveShot : Enemy {

	iTweenPath m_Path;
	Vector3[] m_PathNodes;
	float m_PathLength;
	float m_Distance;

	public void InitPath(iTweenPath path)
	{
		m_Path = path;
		m_PathNodes = m_Path.nodes.ToArray();
		m_PathLength = iTween.PathLength(m_PathNodes);

		m_Distance = 0;
	}

	protected override Vector3 MoveVect
	{
		get
		{
			Vector3 nextPosition = iTween.PointOnPath(m_PathNodes, m_Distance / m_PathLength);
			return nextPosition - m_Transform.position;
		}
	}

	public override void FixedUpdate()
	{
		if (m_Destroyed) return;

		m_Distance += m_TranslationSpeed * Time.fixedDeltaTime;
		if(m_Distance>m_PathLength)
		{
			EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = false });
			m_Destroyed = true;
			Destroy(gameObject);
		}
		else
			base.FixedUpdate();
	}


	public override void ShootBullet()
	{
		GameObject bulletGO1 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		GameObject bulletGO2 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		GameObject bulletGO3 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
		GameObject bulletGO4 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 1.2f, 0), new Quaternion(0, 0, -0.08f, 1));
		GameObject bulletGO5 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 1.2f, 0), new Quaternion(0, 0, 0.08f, 1));
	}
}
