using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic implementation of the Singleton pattern for Unity.
/// This class ensures that only one instance of a specific type exists in the scene.
/// </summary>
/// <typeparam name="T">The type of component to instantiate as a singleton.</typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
	/// <summary>
    /// The static instance of the singleton.
    /// </summary>
    private static T _instance;

	/// <summary>
    /// Gets the instance of the singleton.
    /// If no instance exists, it creates one.
    /// </summary>
    public static T Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>(); 
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

	/// <summary>
    /// Called when the script is instantiated.
    /// Ensures that the instance is properly set.
    /// </summary>
    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
