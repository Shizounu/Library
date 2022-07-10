using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shizounu.Library.Editor;

namespace Shizounu.AI {
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
		private void Awake()
		{
			
		}

		private void Start() {
			if(startState != null)
				ActiveState = startState;
		}

		public void doTick(){
			ActiveState?.OnUpdate(this);
		}
	}
}