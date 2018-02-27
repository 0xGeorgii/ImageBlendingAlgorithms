using log4net;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Processing
{
    public class Log
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Log));
        private static HashSet<Action<string>> _callbacks = new HashSet<Action<string>>();

        static Log()
        {
            _log.Debug($"=== Log subsytem has been initialized ====");
        }

        public static void RegisterCallback(Action<string> callback)
        {
            _callbacks.Add(callback);
        }

        static public void UnregisterCallback(Action<string> callback)
        {
            _callbacks.Remove(callback);
        }

        public static void Debug(string message)
        {
            _log.Debug(message);
            foreach (var callback in _callbacks)
            {
                callback(message);
            }
        }

        public static void Debug(Exception ex)
        {
            _log.Debug(ex);
            foreach (var callback in _callbacks)
            {
                callback(ex.Message);
            }
        }
    }    
}
