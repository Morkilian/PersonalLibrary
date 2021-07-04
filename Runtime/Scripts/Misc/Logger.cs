namespace Morkilian.Helper
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class Logger
    {
        public const string CommonPrefix = "[Morkilian]";
        public const string UsageErrorPrefix = "Something went wrong while using this package.";

        public static void DebugError(string log)
        {
            Debug.LogError($"{CommonPrefix} {UsageErrorPrefix}: {log}");
        }
    }

}