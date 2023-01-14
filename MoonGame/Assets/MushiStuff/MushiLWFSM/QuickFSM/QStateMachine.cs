using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MushiLWFSM
{
    /// <summary>
    /// Quick and VERY dirty state machine implementation
    /// Intended for single file state machines with enums to identify states
    /// </summary>
    public class QStateMachine
    {
        private int currentState;
        public int CurrentState => currentState;

        private Dictionary<int, QState> stateMap = new();

        public QStateMachine(int entryState)
        {
            currentState = entryState;
        }
        
        public void AddNewState(
            int stateID,
            Action updateAction = null,
            Action fixedUpdateAction = null,
            Action enterStateAction = null,
            Action exitStateAction = null,
            Func<int> switchStateAction = null,
            bool immediateUpdate = false)
        {
            var newState = new QState(
                updateAction,
                fixedUpdateAction,
                enterStateAction,
                exitStateAction,
                switchStateAction,
                immediateUpdate);
            
            stateMap.Add(stateID, newState);
        }

        public void RemoveState(int stateID)
        {
            stateMap.Remove(stateID);
        }

        public void UpdateStateMachine()
        {
            int newState = stateMap[currentState].SwitchState();
            if (newState != -1)
            {
                SwitchState(newState);
                if (!stateMap[currentState].ImmediateUpdate)
                {
                    return;
                }
            }
            stateMap[currentState].Update();
        }

        public void SwitchState(int newState)
        {
            stateMap[currentState].ExitState();
            currentState = newState;
            stateMap[currentState].EnterState();
        }
        
        public void FixedUpdateStateMachine()
        {
            stateMap[currentState].FixedUpdate();
        }
    }
}

