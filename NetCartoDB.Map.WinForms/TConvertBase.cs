using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCartoDB.Map.WinForms
{
    public class TConvertBase<T> : TypeConverter where T : new()
    {
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(typeof(T));
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return (destinationType == typeof(InstanceDescriptor)) ? base.CanConvertTo(context, destinationType) : false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo info, object value, Type destType)
        {
            if (destType == typeof(InstanceDescriptor))
                return new InstanceDescriptor(typeof(T).GetConstructor(new Type[0]), null, false);
            else
                return base.ConvertTo(context, info, value, destType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type t)
        {
            return (t == typeof(string)) ? true : base.CanConvertFrom(context, t);
        }
    }

    public class TConvertComplexBase<T> : TConvertBase<T> where T : ITConvertSerializer, new()
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return (typeof(T) == typeof(InstanceDescriptor) || typeof(T) == typeof(string)) ? true : base.CanConvertTo(context, destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value)
        {
            if (value is string)
            {
                string str = value as string;

                try
                {
                    return new T().Deserialize(str);
                }
                catch
                {
                    return new T();
                }

            }
            return base.ConvertFrom(context, info, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo info, object value, Type destType)
        {
            if (destType == typeof(string))
                return new T()?.Deserialize(value as string);
            else if (destType == typeof(InstanceDescriptor))
                return new InstanceDescriptor(typeof(T).GetMethod("Initialize"), new object[] { value }, true);

            return base.ConvertTo(context, info, value, destType);
        }
    }
}
