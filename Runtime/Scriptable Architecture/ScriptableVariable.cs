using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shizounu.Library.ScriptableArchitecture
{
    public abstract class ScriptableVariable<T> : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] T initialValue;
        private T _runtimeValue;
        public T runtimeValue{
            get => _runtimeValue;
            set {
                _runtimeValue = value;
                if(onRuntimeValueChange != null)
                    onRuntimeValueChange.Invoke();
            }
        }


        public ScriptableEvent onRuntimeValueChange;
        public void OnBeforeSerialize(){
            runtimeValue = initialValue;
            if(onRuntimeValueChange != null)
                onRuntimeValueChange.Invoke();
        }
        public void OnAfterDeserialize(){

        }
    }

    [System.Serializable] public abstract class VariableReference<T>
    {
        public bool useConstant;
        public T ConstantValue;
        public ScriptableVariable<T> Variable;

        public VariableReference(){}
        public VariableReference(T value){
            useConstant = true;
            ConstantValue = value;
        }

        public T Value{
            get { return useConstant ? ConstantValue : Variable.runtimeValue;}
        }
    }
    
}

