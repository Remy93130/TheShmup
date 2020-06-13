using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeginner: Enemy {

	[SerializeField] private int NbLives;

	protected override Vector3 MoveVect
	{
		get
		{
			return m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
		}
	}

	public override void FixedUpdate()
	{
		if (m_Destroyed) return;

		if(m_Rigidbody.position.x > 10)
		{
			base.FixedUpdate();
		}

	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("PlayerBullet"))
		{
			NbLives -= 1;
			if (NbLives == 0)
			{
				EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
				EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = true });
				m_Destroyed = true;
				Destroy(gameObject);
			}
		}
		m_Rigidbody.velocity = Vector3.zero;
	}


	public override void ShootBullet()
	{
		GameObject bulletGO1 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
		GameObject bulletGO2 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position + new Vector3(0, 0.6f, 0), new Quaternion(0, 0, -0.05f, 1));
		GameObject bulletGO3 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position - new Vector3(0, 0.6f, 0), new Quaternion(0, 0, 0.05f, 1));
	}
}
