using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net;

namespace Maze
{
    public static class Helper
    {
        public static String ToJSONString(this Object obj)
        {
            using (var stream = new MemoryStream())
            {
                var ser = new DataContractJsonSerializer(obj.GetType());

                ser.WriteObject(stream, obj);

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static T FromJSONString<T>(this string obj)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(obj)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                T ret = (T)ser.ReadObject(stream);
                return ret;
            }
        }
    }
}
