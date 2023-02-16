using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shizounu.Library.AI {
	public class StateMachine : MonoBehaviour {

		[Header("States")]
		[SerializeField] protected State startState = null;
		[SerializeField] protected State activeState;
		public State ActiveState {
			get => activeState;
			set {
				if (value == null) 
					throw new ArgumentNullException();
				if (value == activeState) 
					return;
				activeState?.OnExit(this);
				activeState = value;
				activeState?.OnEnter(this);
			}
		}

		protected Dictionary<Action, System.Object> actionData;
		protected Dictionary<Decision, System.Object> decisionData;

		private void Awake() {
			ActiveState = startState;
		}

		protected virtual void Update() {
			doTick();
		}

		public void doTick(){
			ActiveState?.OnUpdate(this);
		}


	}
}