﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SDD.Events;

public class PlayerController : SimpleGameStateObserver {

	Rigidbody m_Rigidbody;

	[Header("Axes")]
	[SerializeField] private string m_VerticalAxisName;
	[SerializeField] private string m_HorizontalAxisName;
	[SerializeField] private string m_FireAxisName;

	[Header("Spawn")]
	[SerializeField] private Transform m_SpawnPoint;

	[Header("Movement")]
	[SerializeField] private float m_MaxTranslationSpeed;

	[Header("Shoot")]
	[SerializeField] private GameObject m_BulletPrefab;
	[SerializeField] private float m_ShootPeriod;
	private float _nextShootTime;
	[SerializeField] private Transform m_BulletSpawnPoint;

	[Header("Gfx")]
	[SerializeField] private Transform m_Gfx;
	[SerializeField] private float m_GfxSwayAmplitude;
	[SerializeField] private float m_GfxSwayPulsation;
	Quaternion m_InitLocalOrientation;

	public Vector3 GetPositionPlayer {  get { return m_Rigidbody.position; } }

    #region Lifecycle

    protected override void Awake()
	{
		base.Awake();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_InitLocalOrientation = m_Gfx.localRotation;
	}

	private void Update()
	{
		if (!GameManager.Instance.IsPlaying) return;

		//Fire
		if (Input.GetButton(m_FireAxisName) && _nextShootTime<Time.time)
		{
			ShootBullet();
			_nextShootTime = Time.time + m_ShootPeriod;
		}

		//Gfx rotation
		m_Gfx.localRotation = Quaternion.AngleAxis(Mathf.Sin(Time.time*m_GfxSwayPulsation)*m_GfxSwayAmplitude,Vector3.right)*m_InitLocalOrientation;
	}

	private void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying)
		{
			m_Rigidbody.velocity = Vector3.zero;
			return;
		}

		float hAxis = Input.GetAxis(m_HorizontalAxisName);
		float vAxis = Input.GetAxis(m_VerticalAxisName);

		Vector3 inputVector = new Vector3(hAxis, vAxis, 0);

		Vector3 velocity = Vector3.ClampMagnitude( inputVector,1) * m_MaxTranslationSpeed;
		m_Rigidbody.velocity = velocity;
	}

    #endregion

    private void Reset()
	{
		m_Rigidbody.position = m_SpawnPoint.position;
		_nextShootTime = Time.time;
	}

	void ShootBullet() => Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);

	private void OnCollisionEnter(Collision collision)
	{
		// Boss collision = oneshot
		if (collision.gameObject.GetComponent<IBoss>() != null)
		{
			EventManager.Instance.Raise(new PlayerHasBeenHitEvent() { ePlayerController = this, eOneShot = true });
		}
		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
		{
			EventManager.Instance.Raise(new PlayerHasBeenHitEvent() { ePlayerController = this });
		}
	}

    #region Game state event

    protected override void GameBeginnerLevelPlay(GameBeginnerLevelEvent e) => Reset();

	protected override void GameIntermediateLevelPlay(GameIntermediateLevelEvent e) => Reset();

	protected override void GameDifficultLevelPlay(GameDifficultLevelEvent e) => Reset();
	protected override void GameArcadePlay(GameArcadeEvent e) => Reset();

    #endregion


}
