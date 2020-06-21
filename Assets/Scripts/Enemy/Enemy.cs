using SDD.Events;
using UnityEngine;


public abstract class Enemy : SimpleGameStateObserver,IScore {

	protected Rigidbody m_Rigidbody;
	protected Transform m_Transform;

	[SerializeField] private int m_NbLives;

	[Header("Movement")]
	[SerializeField] private float m_MaxTranslationSpeed;
	[SerializeField] private float m_MinTranslationSpeed;
	[SerializeField] private AnimationCurve m_TranslationSpeedProbaCurve;
	protected float m_TranslationSpeed;

	[Header("Shoot")]
	[SerializeField] protected GameObject m_BulletPrefab;
	[SerializeField] private float m_ShootPeriod;
	[SerializeField] private float m_ProbaShoot;
	[SerializeField] protected Transform m_BulletSpawnPoint;
	private float m_NextShootTime;
	private float m_RatioShootLevel;

	[Header("Animation")]
	[SerializeField] protected GameObject m_explosionPrefab = null;

	public float TranslationSpeed { get { return m_TranslationSpeed; } }

	public int NbLives { 
		get { return m_NbLives; }
		set { m_NbLives = value; } 
	}

	protected abstract Vector3 MoveVect { get; }

	[Header("Score")]
	[SerializeField] int m_Score;
	public int Score { get { return m_Score; } }

	protected bool m_Destroyed = false;

    #region Lifecycle

    protected override void Awake()
	{
		base.Awake();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Transform = GetComponent<Transform>();
		m_NextShootTime = Time.time;
		m_RatioShootLevel = EnemiesManager.Instance.RationShootLevel;
		m_TranslationSpeed = Mathf.Lerp(m_MinTranslationSpeed, m_MaxTranslationSpeed, m_TranslationSpeedProbaCurve.Evaluate(Random.value));
	}

	public virtual void Update()
	{
		if (!GameManager.Instance.IsPlaying) return;
		if (Camera.main.WorldToViewportPoint(m_Transform.position).x < -.1f)
		{
			EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = false });
			m_Destroyed = true;
			Destroy(gameObject);
		}
		else
		{
			//Fire
			float probaShoot = Random.Range(1, 1001);
			if (m_NextShootTime < Time.time && probaShoot <= (m_ProbaShoot*m_RatioShootLevel))
			{
				ShootBullet();
                SfxManager.Instance.PlaySfx(Constants.SHOT_SFX);
				m_NextShootTime = Time.time + m_ShootPeriod;
			}
		}
	}

	public virtual void FixedUpdate()
	{
		if (!GameManager.Instance.IsPlaying) return;
		m_Rigidbody.MovePosition(m_Rigidbody.position + MoveVect);
		m_Rigidbody.velocity = Vector3.zero;
	}

    #endregion

    public virtual void ShootBullet() => Instantiate(m_BulletPrefab, m_BulletSpawnPoint.position, Quaternion.identity);

	protected virtual void OnCollisionEnter(Collision collision)
	{
		ShieldCollision shield = GetComponentInChildren<ShieldCollision>();
		if (collision.gameObject.CompareTag("PlayerBullet") && shield != null)
		{
			shield.ManageCollision(collision);
		}
		else if (collision.gameObject.CompareTag("PlayerBullet") || collision.gameObject.CompareTag("Player"))
		{
			if (--m_NbLives == 0)
			{
				EventManager.Instance.Raise(new ScoreItemEvent() { eScore = this as IScore });
				EventManager.Instance.Raise(new EnemyHasBeenDestroyedEvent() { eEnemy = this, eDestroyedByPlayer = true });
				m_Destroyed = true;
				Explosion();
			}
		}
		m_Rigidbody.velocity = Vector3.zero;
	}

	public void Explosion()
	{
		if (m_explosionPrefab)
			Instantiate(m_explosionPrefab, m_Rigidbody.transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
