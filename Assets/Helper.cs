using Firebase.Database;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Helpers
{
    public class Helper
    {

    }
    public static class StaticHelper
    {
        public static T GetDataOrDefault<T>(this DocumentSnapshot shot, string key)
        {
            if (shot.TryGetValue<T>(key, out T value))
            {
                return value;
            }
            return default(T);

        }
     

    }
}
