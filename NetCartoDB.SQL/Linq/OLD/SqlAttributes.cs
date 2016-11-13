using System;
using System.Reflection;
using NetCartoDB.Core.Utils;

namespace NetCartoDB.SQL.Linq
{
    public class SqlAttributes
    {
        [AttributeUsage(System.AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
        public class TableAttribute : Attribute
        {
            public string Name { get; set; }
            public string PrimaryKey { get; set; }
            public bool AutoIncremet { get; set; }

            public TableAttribute(string table)
            {
                this.Name = table;
            }
        }

        [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class ColumnAttribute : Attribute
        {
            public ColumnAttribute() { }
            public ColumnAttribute(string name) { this.Name = name; }
            public string Format { get; set; }
            public string Name { get; set; }
            public string Default { get; set; }
        }

        [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
        public class IgnoreAttribute : Attribute
        {
            public bool Skip { get; set; }
            public IgnoreAttribute() : this(true) { }
            public IgnoreAttribute(bool ignore)
            {
                this.Skip = ignore;
            }
        }

        internal class Manager
        {
            internal static bool IsAutoincrement<TEntity>()
            {
                return SqlAttributes.Manager.IsAutoincrement(typeof(TEntity));
            }
            internal static bool IsAutoincrement(Type type)
            {
                SqlAttributes.TableAttribute attr = Reflection.GetAttribute<SqlAttributes.TableAttribute>(type);
                return (attr != null) ? attr.AutoIncremet : false;
            }
            internal static string GetTableName<TEntity>()
            {
                return SqlAttributes.Manager.GetTableName(typeof(TEntity));
            }
            internal static string GetTableName(Type type)
            {
                SqlAttributes.TableAttribute attr = Reflection.GetAttribute<SqlAttributes.TableAttribute>(type);
                return (attr != null) ? attr.Name : type.Name;
            }

            internal static bool IsIgnored(Type objType, string pName)
            {
                SqlAttributes.IgnoreAttribute attr = Reflection.GetAttribute<SqlAttributes.IgnoreAttribute>(objType, pName);
                return (attr != null) ? attr.Skip : false;
            }
            internal static bool IsIgnored<TEntity>(string pName)
            {
                return SqlAttributes.Manager.IsIgnored(typeof(TEntity), pName);
            }
            internal static bool IsIgnored(PropertyInfo property)
            {
                SqlAttributes.IgnoreAttribute attr = Reflection.GetAttribute<SqlAttributes.IgnoreAttribute>(property);
                return (attr != null) ? attr.Skip : false;
            }
            internal static string GetPrimaryKey<TEntity>()
            {
                return SqlAttributes.Manager.GetPrimaryKey(typeof(TEntity));
            }
            internal static string GetPrimaryKey(Type type)
            {
                SqlAttributes.TableAttribute attr = Reflection.GetAttribute<SqlAttributes.TableAttribute>(type);
                return (attr != null) ? attr.PrimaryKey : null;
            }
            internal static string GetColumnName(PropertyInfo property)
            {
                SqlAttributes.ColumnAttribute attr = Reflection.GetAttribute<SqlAttributes.ColumnAttribute>(property);
                return (attr != null) ? attr.Name : property.Name;
            }
            internal static string GetColumnName(Type type, string pName)
            {
                SqlAttributes.ColumnAttribute attr = Reflection.GetAttribute<SqlAttributes.ColumnAttribute>(type, pName);
                return (attr != null) ? attr.Name : pName;
            }
            internal static string GetColumnName<TEntity>(string pName)
            {
                return GetColumnName(typeof(TEntity), pName);
            }
            internal static string GetColumnFormat(Type type, string pName)
            {
                SqlAttributes.ColumnAttribute attr = Reflection.GetAttribute<SqlAttributes.ColumnAttribute>(type, pName);
                return (attr != null) ? attr.Format : null;
            }
            internal static string GetColumnFormat(PropertyInfo property)
            {
                SqlAttributes.ColumnAttribute attr = Reflection.GetAttribute<SqlAttributes.ColumnAttribute>(property);
                return (attr != null) ? attr.Format : null;
            }
            internal static string GetColumnFormat<TEntity>(string pName)
            {
                return GetColumnFormat(typeof(TEntity), pName);
            }
            internal static object GetColumnDefaultValue(PropertyInfo pInfo)
            {
                SqlAttributes.ColumnAttribute attr = Reflection.GetAttribute<SqlAttributes.ColumnAttribute>(pInfo);
                return (attr != null) ? attr.Default : Activator.CreateInstance(pInfo.PropertyType);
            }
        }
    }
}