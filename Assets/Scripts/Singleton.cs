﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        var s1ngleton = new GameObject("[SINGLETON] " + typeof(T));
                        _instance = s1ngleton.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }
        protected virtual void Awake() 
        {
            if (Instance != this) Destroy(gameObject);
        }
}
