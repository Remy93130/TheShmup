﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowMvtOneShot : Enemy {

	private PlayerController player;
	protected override Vector3 MoveVect
	{
		get
		{
			Vector3 playerPosition = GameManager.Instance.GetPlayer.GetPositionPlayer;
			if (playerPosition.x + 2 < m_Rigidbody.position.x)
			{
				float deltaY = playerPosition.y - m_Rigidbody.position.y;
				return (m_Transform.right + new Vector3(0,deltaY / m_TranslationSpeed, 0)) * m_TranslationSpeed * Time.deltaTime;
			}
			else
			{
				return m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
			}
		}
	}

	protected override void Awake() => base.Awake();

	public override void ShootBullet() => Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
}
