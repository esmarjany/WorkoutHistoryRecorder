using System;

namespace WorkoutHistoryRecorder.WatchApp.Infra
{
    class MyLoger
    {
        public static void Log(string content)
        {
            Tizen.Log.Info("MyApp", content);
        }

        internal static void Log(Exception ex)
        {
            Log(ex.Message);
            Log(ex.StackTrace);
        }
    }
}
