using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.AI {
	public abstract class Decision : ScriptableObject {
		public abstract bool Decide(StateMachine stateMachine);
	}
}