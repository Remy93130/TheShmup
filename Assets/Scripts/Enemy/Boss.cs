using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss: Enemy {


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
		float currXPosition = m_Rigidbody.transform.position.x;
        Debug.Log(currXPosition);
		if (currXPosition > 10)
		{
			base.FixedUpdate();
		}
		m_Rigidbody.transform.position = new Vector3(currXPosition, 0, 0);
	}

	public void OnCollisionEnter(Collision collision)
	{
		float currXPosition = m_Rigidbody.transform.position.x;
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
		m_Rigidbody.transform.position = new Vector3(currXPosition, 0, 0);
	}

}
