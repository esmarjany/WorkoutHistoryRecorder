
using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using WearableCompanion.Droid;

namespace WorkoutHistoryRecorder.Contract.Tools
{
    public class MySerializer
    {
        private static bool _isJson = true;
        public static void Init()
        {

        }

        public static T DeserializeObject<T>(byte[] argument)
        {
            if (_isJson)
                return JsonDeserialize<T>(argument);

            return ProtoBufDeserialize<T>(argument);
        }

        private static T ProtoBufDeserialize<T>(byte[] argument)
        {
            T res = default;
            using (var fileStream = new MemoryStream(argument))
            {
                Serializer.Serialize(fileStream, res);
            }
            return res;
        }

        private static T JsonDeserialize<T>(byte[] argument)
        {
            var strArg = Encoding.Unicode.GetString(argument);
            return JsonConvert.DeserializeObject<T>(strArg);
        }

        public static byte[] SerializeObject(object watchCommand) //where T : class
        {
            if (_isJson)
                return JsonSerilize(watchCommand);
            return ProtoBufSerilize(watchCommand);
        }

        private static byte[] ProtoBufSerilize(object watchCommand)
        {
            using (var memoryStream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(memoryStream, watchCommand);
                return memoryStream.ToArray();
            }
        }

        private static byte[] JsonSerilize(object watchCommand)
        {
            var json = JsonConvert.SerializeObject(watchCommand);
            return Encoding.Unicode.GetBytes(json);
        }
    }
}
