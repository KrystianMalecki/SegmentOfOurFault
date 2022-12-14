using Firebase.Database;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
namespace Helpers
{
    public class Helper
    {

    }
    public static class StaticHelper
    {
        public static readonly bool displayMoreInfo = true;

        public static T GetDataOrDefault<T>(this DocumentSnapshot shot, string key)
        {
            if (shot.TryGetValue<T>(key, out T value))
            {
                return value;
            }
            return default(T);

        }
        public static void DebugPoint(object obj = null,
              [CallerLineNumber] int lineNumber = 0,
              [CallerMemberName] string caller = null)
        {
            UnityEngine.Debug.Log(
                $"{caller} at {lineNumber + 1}. Logs: '{obj}'");
        }

        public static void DebugMoreInfo(object obj = null,
             [CallerLineNumber] int lineNumber = 0,
             [CallerMemberName] string caller = null)
        {
            if (!displayMoreInfo)
            {
                return;
            }
            UnityEngine.Debug.Log(
                $"{caller} at {lineNumber + 1}. Logs: '{obj}'");
        }
        
        public static string ToJsonString(this object obj)
        {
            return JsonUtility.ToJson(obj, true);
        }


    }
}
