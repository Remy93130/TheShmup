using System.Collections;
using System;
using UnityEngine;


using Random = UnityEngine.Random;
using SDD.Events;

[Serializable]
public class Level
{
	[SerializeField] GameObject[] m_PatternsPrefabs;

	public GameObject getLevelPattern(int indexPattern) => m_PatternsPrefabs[Mathf.Max(indexPattern, 0)];

	public int randomLevel() => Random.Range(0, m_PatternsPrefabs.Length - 1);

	public int getBossIndex() => m_PatternsPrefabs.Length - 1;

	public bool hasLevelFinished(int indexPattern) => (indexPattern == m_PatternsPrefabs.Length) ? true : false;
}

public class EnemiesManager : Manager<EnemiesManager> {

	#region patterns & current pattern management
	[Header("EnemiesManager")]
	[SerializeField] Level[] m_levels;
	private int m_CurrentPatternIndex;
	private int m_CurrentLevelIndex;
	private int m_ArcadeCounter;
	private Level m_CurrentLevel;
	private GameObject m_CurrentPatternGO;
	private IPattern m_CurrentPattern;
	private float m_RationShootLevel;

	public IPattern CurrentPattern { get => m_CurrentPattern; }

	public float RationShootLevel { get => m_RationShootLevel; }

	#endregion

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<AllEnemiesOfPatternHaveBeenDestroyedEvent>(AllEnemiesOfPatternHaveBeenDestroyed);
		EventManager.Instance.AddListener<GoToNextPatternEvent>(GoToNextPattern);
	}

	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<AllEnemiesOfPatternHaveBeenDestroyedEvent>(AllEnemiesOfPatternHaveBeenDestroyed);
		EventManager.Instance.RemoveListener<GoToNextPatternEvent>(GoToNextPattern);
	}
	#endregion

	#region Manager Implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Pattern flow
	void Reset()
	{
		EventManager.Instance.Raise(new GameResetEvent());
		DeleteOldGameObject();
		Destroy(m_CurrentPatternGO);
		m_CurrentPatternGO = null;
		m_CurrentPatternIndex = -1;
		HudManager.Instance.SetBorderBoss(false, true);
    }

	private void DeleteOldGameObject()
	{
		GameObject[] oldEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < oldEnemies.Length; i++)
		{
			Enemy enemyComponent = oldEnemies[i].GetComponent<Enemy>();
			if (enemyComponent)
				enemyComponent.Explosion();
		}
		GameObject[] oldBullet = GameObject.FindGameObjectsWithTag("EnemyBullet");
		for (int i = 0; i < oldBullet.Length; i++)
			Destroy(oldBullet[i]);
		ResetAnimation();
	}

	private void ResetAnimation()
	{
		GameObject[] animations = GameObject.FindGameObjectsWithTag("Animation");
		for (int i = 0; i < animations.Length; i++)
			Destroy(animations[i]);
	}

	IPattern InstantiatePattern(int patternIndex)
	{
		m_CurrentPatternGO = Instantiate(m_CurrentLevel.getLevelPattern(patternIndex));
		return m_CurrentPatternGO.GetComponent<IPattern>();
	}

	private IEnumerator InstantiatePatternCoroutine()
	{
		Destroy(m_CurrentPatternGO);
		while (m_CurrentPatternGO) yield return null;
		if (m_CurrentLevel.hasLevelFinished(m_CurrentPatternIndex))
		{
			EventManager.Instance.Raise(new LevelHasEnded());
		}
		else
		{
			m_CurrentPattern = InstantiatePattern(m_CurrentPatternIndex);
			m_CurrentPattern.StartPattern();
			EventManager.Instance.Raise(new PatternHasBeenInstantiatedEvent() { ePattern = m_CurrentPattern });
		}
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameMenu(GameMenuEvent e) => Reset();

	protected override void GameBeginnerLevelPlay(GameBeginnerLevelEvent e)
	{
		m_CurrentLevelIndex = 0;
		m_CurrentLevel = m_levels[m_CurrentLevelIndex];
		m_RationShootLevel = 1;
		PlayLevel();
	}

	protected override void GameIntermediateLevelPlay(GameIntermediateLevelEvent e)
	{
		m_CurrentLevelIndex = 1;
		m_CurrentLevel = m_levels[m_CurrentLevelIndex];
		m_RationShootLevel = 1.5f;
		PlayLevel();
	}

	protected override void GameDifficultLevelPlay(GameDifficultLevelEvent e)
	{
		m_CurrentLevelIndex = 2;
		m_CurrentLevel = m_levels[m_CurrentLevelIndex];
		m_RationShootLevel = 2;
		PlayLevel();
	}

	protected override void GameArcadePlay(GameArcadeEvent e)
	{
		m_CurrentLevelIndex = 3;
		m_CurrentLevel = m_levels[m_CurrentLevelIndex];
		m_RationShootLevel = 1;
		m_ArcadeCounter = 0;
		PlayLevel(true);
	}

	protected void PlayLevel(bool isArcade = false)
	{
		Reset();
        HudManager.Instance.m_PrefScore.SetActive(isArcade);
		EventManager.Instance.Raise(new GoToNextPatternEvent() { eArcadeMode = isArcade });
	}

	#endregion

	#region Callbacks to EnemiesManager events
	public void GoToNextPattern(GoToNextPatternEvent e)
	{
		if (e.eArcadeMode)
			ArcadePattern();
		else
			m_CurrentPatternIndex++;
		StartCoroutine(InstantiatePatternCoroutine());
	}

	private void ArcadePattern()
	{
		if (++m_ArcadeCounter % 10 == 0)
		{
			m_CurrentPatternIndex = m_CurrentLevel.getBossIndex();
			m_RationShootLevel += 1;
		}
		else
		{
			m_CurrentPatternIndex = m_CurrentLevel.randomLevel();
		}
	}
	#endregion

	#region Callbacks to Pattern events
	void AllEnemiesOfPatternHaveBeenDestroyed(AllEnemiesOfPatternHaveBeenDestroyedEvent e) => EventManager.Instance.Raise(new GoToNextPatternEvent());
	#endregion
}
