using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    private bool isReady;

    public static FirebaseManager instance;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                MLogs("Firebase is ready");
                isReady = true;
            }
            else
            {
                MLogs(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
    // Start is called before the first frame update
    void Start()
    {
    }


    public void LogEvent(string eventName)
    {
        MLogs("Firebase Event: " + eventName);

        UIManagerScript.instance.debugText.text = isReady.ToString();
        if (isReady)
        {
            FirebaseAnalytics.LogEvent(eventName);

            UIManagerScript.instance.debugText.text = eventName;
        }
        else
        {
            MLogs("Firebase is not ready");
        }
    }

    public void LogEvent(string eventName, string paramName, int paramValue)
    {
        MLogs("Firebase Event: " + eventName + " " + paramName + " " + paramValue);
        UIManagerScript.instance.debugText.text = isReady.ToString();

        if (isReady)
        {
            Parameter myparams = new Parameter(paramName, paramValue);
            FirebaseAnalytics.LogEvent(eventName, myparams);

            UIManagerScript.instance.debugText.text = eventName + " " + paramName + " " + paramValue;
        }
        else
        {
            MLogs("Firebase is not ready");
        }
    }

    public void LogLevelStartedEvent(int level)
    {
        if (isReady)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart,
                new Parameter(FirebaseAnalytics.ParameterLevel, level));
        }
        else
        {
            Debug.Log("Firebase Not Initialized");
        }
    }


    public void LogLevelCompleteEvent(int level)
    {
        if (isReady)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp,
                new Parameter("success", 1),
                new Parameter(FirebaseAnalytics.ParameterLevel, level));
        }
        else
        {
            Debug.Log("Firebase Not Initialized");
        }
    }

    public void LogLevelFailedEvent(int level)
    {
        if (isReady)
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd,
                new Parameter("success", 0),
                new Parameter(FirebaseAnalytics.ParameterLevel, level));
        }
        else
        {
            Debug.Log("Firebase Not Initialized");
        }
    }

    void MLogs(string log)
    {
        Debug.Log("$$ " + log);
    }
}
