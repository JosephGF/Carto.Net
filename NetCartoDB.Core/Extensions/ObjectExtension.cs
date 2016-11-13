using NetCarto.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetCarto.Core.Extensions
{
    public static class ObjectExtension
    {
        public static string ToJson(this object @object)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(@object);
        }

        public static T FromJson<T>(this string @string)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(@string);
        }

        public static object FromJson(this string @string)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(@string);
        }
        
        public static object ConvertTo(this object @object, Type type)
        {
            bool isCasted = false;
            Type tParent = @object.GetType();

            var fields = Reflection.GetFields(tParent);
            var properties = Reflection.GetFields(tParent);

            object instance = Activator.CreateInstance(type);

            foreach (var f in Reflection.GetFields(type).Where(f => f.IsPublic))
            {
                var it = fields.FirstOrDefault(d => d.Name.Equals(f.Name));
                if (it != null)
                {
                    it.SetValue(instance, f.Name);
                    isCasted = true;
                }
            }

            foreach (var f in Reflection.GetProperties(type).Where(f => f.CanWrite))
            {
                var it = properties.FirstOrDefault(d => d.Name.Equals(f.Name));
                if (it != null)
                {
                    it.SetValue(instance, f.Name);
                    isCasted = true;
                }
            }

            return isCasted ? instance : null;
        }

        public static T ConvertTo<T>(this object @object) where T : class, new()
        {
            return @object.ConvertTo(typeof(T)) as T;
        }
    }
}
