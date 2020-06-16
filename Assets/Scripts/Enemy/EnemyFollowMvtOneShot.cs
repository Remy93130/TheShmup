using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowMvtOneShot : Enemy {

	private PlayerController player;

	protected override Vector3 MoveVect
	{
		get
		{
			return m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		player = GameManager.Instance.GetPlayer;
	}

	public override void ShootBullet()
	{
		GameObject bulletGO1 = Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
	}
}
