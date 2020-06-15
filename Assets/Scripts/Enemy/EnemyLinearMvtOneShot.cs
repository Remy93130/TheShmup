using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinearMvtOneShot : Enemy {

	protected override Vector3 MoveVect
	{
		get
		{
			return m_Transform.right * m_TranslationSpeed * Time.fixedDeltaTime;
		}
	}
}
