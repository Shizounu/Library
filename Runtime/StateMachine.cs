using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shizounu.Library.Editor;
using UnityEngine.AI;

namespace Shizounu.Library.AI {
	public class StateMachine : MonoBehaviour {

		[SerializeField] private State startState = null;
		[SerializeField, ReadOnly] private State activeState;
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
		public NavMeshAgent NavMeshAgent;

		private void Start() {
			if(startState != null)
				ActiveState = startState;
		}

		private void Update() {
			doTick();
		}
		public void doTick(){
			ActiveState?.OnUpdate(this);
		}
	}
}