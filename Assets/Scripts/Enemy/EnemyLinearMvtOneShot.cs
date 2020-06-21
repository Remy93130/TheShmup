using UnityEngine;

public class EnemyLinearMvtOneShot : Enemy {

	protected override Vector3 MoveVect
	{
		get => m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
	}
}
