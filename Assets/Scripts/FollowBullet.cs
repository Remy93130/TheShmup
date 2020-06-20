using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : Bullet
{
	protected override void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying) return;
		Vector3 moveVect;

		Vector3 playerPosition = GameManager.Instance.GetPlayer.GetPositionPlayer;
		if (playerPosition.x+2 < m_Rigidbody.position.x)
		{
			float Differencey = playerPosition.y - m_Rigidbody.position.y;
			moveVect = ((m_Transform.right + new Vector3(0, Differencey / m_TranslationSpeed, 0)) * m_TranslationSpeed * Time.deltaTime);
			m_Rigidbody.MovePosition(m_Rigidbody.position + moveVect);
		}
		else
		{
			base.FixedUpdate();
		}
	}
}
