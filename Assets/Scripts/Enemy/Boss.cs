using SDD.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss: Enemy, IBoss {

	[Header("Boss animation")]
	[SerializeField] private GameObject[] m_ExplosionsSpawns;
	[SerializeField] private GameObject m_DamageAnimation;
	[SerializeField] private GameObject m_DamageAnimationSpawn;
	[SerializeField] private int m_LivesForDamageAnimation;
	private GameObject _animation = null;

	protected override Vector3 MoveVect
	{
		get => m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
	}

    #region Lifecycle

    protected override void Awake()
    {
        base.Awake();
        EventManager.Instance.Raise(new NewBossEvent() { eNLives = NbLives });
        EventManager.Instance.Raise(new GameBossShotedEvent() { eNLives = NbLives});
        HudManager.Instance.SetBorderBoss(true);
    }

	public override void FixedUpdate()
	{
		if (m_Destroyed) return;
		float currXPosition = m_Rigidbody.transform.position.x;
        
		if (currXPosition > 8)
		{
			base.FixedUpdate();
		}
		if (NbLives <= m_LivesForDamageAnimation && !_animation)
		{
			_animation = Instantiate(m_DamageAnimation, m_DamageAnimationSpawn.transform.position, Quaternion.identity);
		}
		m_Rigidbody.transform.position = new Vector3(currXPosition, 0, 0);
	}

    #endregion

    protected override void OnCollisionEnter(Collision collision)
	{
        
        float currXPosition = m_Rigidbody.transform.position.x;
		if (collision.gameObject.CompareTag("PlayerBullet"))
		{
			if (--NbLives == 0)
			{
				EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
				EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = true });
                
                m_Destroyed = true;
                HudManager.Instance.SetBorderBoss(false);
				for (int i = 0; i < m_ExplosionsSpawns.Length; i++)
				{
					Instantiate(m_explosionPrefab, m_ExplosionsSpawns[i].transform.position, Quaternion.identity);
				}
				Destroy(_animation);
                Destroy(gameObject);
                

            }
		}
        EventManager.Instance.Raise(new GameBossShotedEvent() { eNLives = NbLives });
        m_Rigidbody.velocity = Vector3.zero;
		m_Rigidbody.transform.position = new Vector3(currXPosition, 0, 0);
	}

}
