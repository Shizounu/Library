using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Shizounu.Library.AI {
	[CreateAssetMenu(fileName = "New State", menuName = "Shizounu/AI/State")]
	public class State : ScriptableObject {
		[SerializeField] private List<Action> enterActions = null;
		[SerializeField] private List<Action> updateActions = null;
		[SerializeField] private List<Action> exitActions = null;
		[SerializeField] private List<Transition> transitions = null;


		public void OnEnter(StateMachine stateMachine) {
			foreach (Action action in enterActions) {
				action.Act(stateMachine);
			}
		}

		public void OnExit(StateMachine stateMachine) {
			foreach (Action action in exitActions) {
				action.Act(stateMachine);
			}
		}

		public void OnUpdate(StateMachine stateMachine) {
			foreach (Action action in updateActions) {
				action.Act(stateMachine);
			}
			foreach (Transition transiton in transitions) {
				bool decisionSucceded = transiton.decision.Decide(stateMachine);
				if (decisionSucceded != transiton.invertDecision) {
					stateMachine.ActiveState = transiton.transitionState;
				}
			}
		}
	}

	[Serializable]
	public class Transition {
		public Decision decision;
		public bool invertDecision;
		public State transitionState;
	}
}
