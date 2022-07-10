using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.AI {
	public abstract class Action : ScriptableObject {
		public abstract void Act(StateMachine stateMachine);
	}
}