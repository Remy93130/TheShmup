using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathMvtOneShot : EnemyITweenPathMvt
{
	public override void ShootBullet() => Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);
}
