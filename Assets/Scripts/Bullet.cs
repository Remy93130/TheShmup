using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SimpleGameStateObserver
{
	protected Rigidbody m_Rigidbody;
	protected Transform m_Transform;

	[Header("Life duration")]
	[SerializeField]
	private float m_LifeDuration;

	[Header("Movement")]
	[SerializeField]
	protected float m_TranslationSpeed;

	protected override void Awake()
	{
		base.Awake();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Transform = GetComponent<Transform>();
		Destroy(gameObject, m_LifeDuration);
	}


	protected virtual void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying) return;
		Vector3 moveVect = m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
		m_Rigidbody.MovePosition(m_Rigidbody.position + moveVect);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}

	protected override void GameMenu(GameMenuEvent e)
	{
		Destroy(gameObject);
	}

	protected override void GameOver(GameOverEvent e)
	{
		Destroy(gameObject);
	}

	protected override void GameVictory(GameVictoryEvent e)
	{
		Destroy(gameObject);
	}
}
