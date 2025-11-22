using UnityEngine;
using System;

namespace IngameDebugConsole
{
    [DefaultExecutionOrder(-10000)]
    [DisallowMultipleComponent]
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        [NonSerialized] bool _initialized;
        public static T Instance { get; private set; }
        protected virtual void Awake()
        {
            _initialized = false;
            if ( Instance != null && Instance != this )
            {
                Debug.LogWarning( $"Duplicate { typeof(T).Name } found. Destroying { gameObject.name }." );
                Destroy( gameObject );

                return;
            }
            InitializeIfNeeded();
        }

        protected virtual void OnEnable()
        {
            InitializeIfNeeded();
        }

        private void InitializeIfNeeded()
        {
            if ( !Application.isPlaying || _initialized )
                return;
            Instance = ( T )this;
            _initialized = true;
            if ( !ShouldDestroyOnLoad ) DontDestroyOnLoad( gameObject );
            Initialize();
        }

        protected virtual void Initialize() { }

        protected virtual void OnDestroy()
        {
            if ( Instance == this ) Instance = null;
        }

        abstract protected bool ShouldDestroyOnLoad { get; }
    }
}