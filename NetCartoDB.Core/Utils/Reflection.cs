using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetCarto.Core.Utils
{
    public class Reflection
    {
        public static T Create<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public static PropertyInfo GetProperty<Type>(string name)
        {
            return (typeof(Type)).GetRuntimeProperty(name);
        }

        public static PropertyInfo GetProperty(Type objType, string name)
        {
            return objType.GetRuntimeProperty(name);
        }

        public static FieldInfo GetField<Type>(string name)
        {
            return (typeof(Type)).GetRuntimeField(name);
        }

        public static FieldInfo GetField(Type objType, string name)
        {
            return objType.GetRuntimeField(name);
        }

        public static EventInfo GetEvent<Type>(string name)
        {
            return (typeof(Type)).GetRuntimeEvent(name);
        }

        public static EventInfo GetEvent(Type objType, string name)
        {
            return objType.GetRuntimeEvent(name);
        }

        public static MethodInfo GetMethod<T>(string name, params Type[] args)
        {
            return (typeof(T)).GetRuntimeMethod(name, args);
        }

        public static MethodInfo GetEvent(Type objType, string name, params Type[] args)
        {
            return objType.GetRuntimeMethod(name, args);
        }

        public static object Invoke(MethodInfo method, object instance, params object[] args)
        {
            return method.Invoke(instance, args);
        }

        public static object Invoke(MethodInfo method, params object[] args)
        {
            return Reflection.Invoke(method, null, args);
        }

        public static object GetValue(PropertyInfo property, object instance)
        {
            return property.GetValue(instance);
        }

        public static object GetValue(FieldInfo field, object instance)
        {
            return field.GetValue(instance);
        }

        public static T GetValue<T>(PropertyInfo property, object instance) where T : class
        {
            return GetValue(property, instance) as T;
        }

        public static T GetValue<T>(FieldInfo field, object instance) where T : class
        {
            return GetValue(field, instance) as T;
        }

        public static void SetValue(PropertyInfo property, object instance, object value)
        {
            property.SetValue(instance, value);
        }

        public static void SetValue(FieldInfo field, object instance, object value)
        {
            field.SetValue(instance, value);
        }

        public static Dictionary<PropertyInfo, object> GetPropertiesValue(object instance)
        {
            Dictionary<PropertyInfo, object> data = new Dictionary<PropertyInfo, object>();
            var properties = Reflection.GetProperties(instance.GetType());
            for (int i = 0; i < properties.Count(); i++)
            { 
                string value = Reflection.GetValue(properties.ElementAt(i), instance)?.ToString() ?? "null";
                data.Add(properties.ElementAt(i), value);
            }

            return data;
        }

        public static Dictionary<FieldInfo, object> GetFieldsValue(object instance)
        {
            Dictionary<FieldInfo, object> data = new Dictionary<FieldInfo, object>();
            var fields = Reflection.GetFields(instance.GetType());
            for (int i = 0; i < fields.Count(); i++)
            {
                string value = Reflection.GetValue(fields.ElementAt(i), instance)?.ToString() ?? "null";
                data.Add(fields.ElementAt(i), value);
            }

            return data;
        }

        public static IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type.GetRuntimeProperties();
        }

        public static IEnumerable<FieldInfo> GetFields(Type type)
        {
            return type.GetRuntimeFields();
        }

        public static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetRuntimeProperties();
        }

        public static IEnumerable<FieldInfo> GetFields<T>()
        {
            return typeof(T).GetRuntimeFields();
        }

        public static IEnumerable<MemberInfo> GetMembers(Type type)
        {
            List<MemberInfo> data = new List<MemberInfo>();
            data.AddRange(type.GetRuntimeEvents());
            data.AddRange(type.GetRuntimeFields());
            data.AddRange(type.GetRuntimeMethods());
            data.AddRange(type.GetRuntimeProperties());

            return data;
        }

        public static IEnumerable<MemberInfo> GetMembers<T>()
        {
            return Reflection.GetMembers(typeof(T));
        }

        public static MemberInfo GetMemberInfo(Type objType, string memberName)
        {
            return (MemberInfo)objType.GetRuntimeProperty(memberName)
                ?? (MemberInfo)objType.GetRuntimeField(memberName)
                ?? (MemberInfo)objType.GetRuntimeEvent(memberName)
                ?? (MemberInfo)objType.GetRuntimeMethod(memberName, null);
        }

        public static TAttribute GetAttribute<TAttribute>(Type objType, string propertyName, bool inherent = true) where TAttribute : Attribute
        {
            MemberInfo member = Reflection.GetMemberInfo(objType, propertyName);
            return Reflection.GetAttribute<TAttribute>(member, inherent);
        }

        public static TAttribute GetAttribute<TAttribute>(MemberInfo member, bool inherent = true) where TAttribute : Attribute
        {
            return member.GetCustomAttribute<TAttribute>(inherent) as TAttribute;
        }

        public static TAttribute GetAttribute<TAttribute>(Type objType, bool inherent = true) where TAttribute : Attribute
        {
            return Reflection.GetAttribute<TAttribute>(objType.GetTypeInfo(), inherent) as TAttribute;
        }

        public static bool HasAttribute<TAttibute>(Type type, bool inherent = true) where TAttibute : Attribute
        {
            return Reflection.GetAttribute<TAttibute>(type, inherent) != null;
        }

        public static bool HasAttribute<TAttibute>(Type type, string pname, bool inherent = true) where TAttibute : Attribute
        {
            return Reflection.GetAttribute<TAttibute>(type, pname, inherent) != null;
        }

        public static bool HasAttribute<TAttibute>(MemberInfo member, bool inherent = true) where TAttibute : Attribute
        {
            return Reflection.GetAttribute<TAttibute>(member, inherent) != null;
        }

        public static bool HasAttribute<TAttribute, T>(bool inherent = true) where TAttribute : Attribute
        {
            return Reflection.GetAttribute<TAttribute>(typeof(T), inherent) != null;
        }

        public static bool HasAttribute<TAttribute, T>(string pName, bool inherent = true) where TAttribute : Attribute
        {
            return Reflection.GetAttribute<TAttribute>(typeof(T), pName, inherent) != null;
        }
    }
}
