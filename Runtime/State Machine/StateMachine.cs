using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Shizounu.Library.AI {
	public class StateMachine : MonoBehaviour {

		[SerializeField] private State startState = null;
		[SerializeField] private State activeState;
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

		private void Awake() {
			if(startState != null)
				ActiveState = startState;
		}

		public void doTick(){
			ActiveState?.OnUpdate(this);
		}
	}
}