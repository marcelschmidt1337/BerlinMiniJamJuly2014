using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Linq;
using System.Reflection;


namespace UEDS
{
	public interface IPrimitveSerializer
	{
		
		List<SerializedPrimitive> Serialize(object pValue);
		object Deserialize(List<SerializedPrimitive> pValue);
		System.Type HandledType();
	}
	
	public class SerializedPrimitive
	{
		//public string mType;
		public string mName;
		public string mValue;
		
		public SerializedPrimitive(){}
		
		public SerializedPrimitive(string pName, object pValue)
		{
			mName = pName;
			mValue = pValue.ToString ();
			//mType = pValue.GetType().AssemblyQualifiedName;
		}
		
	}
	
	public class SerializedValue
	{
		private static Dictionary<System.Type,IPrimitveSerializer> serializers;
		
		
		public string mType;
		
		[XmlIgnore]
		public System.Type ValueType
		{
			get
			{
				return Type.GetType(mType);
			}
			set
			{
				mType = value.AssemblyQualifiedName;
			}
		}
		
		public List<SerializedPrimitive> mValues = new List<SerializedPrimitive>();
		
		public SerializedValue(){}
		
		
		
		
		public SerializedValue (object pValue, System.Type pType)
		{
			LoadSerializers();
			ValueType = pType;
			
			
			Serialize(pType, pValue);
			
		}
		
		void Serialize(System.Type pType, object pValue )
		{
			if(IsNumericType(pType))
				mValues = SinglePrimitive(pType, pValue);
			else if(pType == typeof(string))
				mValues = SinglePrimitive(pType, pValue);
			else if(serializers.ContainsKey(pType))
				mValues = serializers[pType].Serialize(pValue);
			
		}
		
		
		
		List<SerializedPrimitive> SinglePrimitive(System.Type pType, object pValue)
		{
			var outList = new List<SerializedPrimitive>();
			
			var e = new SerializedPrimitive("",pValue);
			outList.Add(e);
			
			return outList;
		}
		
		object Deserialize()
		{
			System.Type t = ValueType;
			if(IsNumericType (t))
				return ToNumericType(t, mValues[0].mValue);
			else if(t == typeof(string))
				return mValues[0].mValue;
			else if(serializers.ContainsKey(t))
				return serializers[t].Deserialize(mValues);
			
			return null;
		}
		
		void LoadSerializers()
		{
			
			if(serializers == null)
			{
				serializers = new Dictionary<Type, IPrimitveSerializer>();
				
				foreach ( var assembly in GetAssembly())
				{
					foreach(var t in GetTypes(assembly))
					{
						if(!IsClass(t))
							continue;
						int idx = Array.IndexOf( GetInterfaces(t) , typeof(IPrimitveSerializer));
						if(idx != -1)
						{
							if(t == typeof(IPrimitveSerializer))
								continue;
							var instance = Activator.CreateInstance(t) as IPrimitveSerializer;
							
							
							
							serializers.Add(instance.HandledType() ,instance);
							
							Debug.Log("Serializer for " + instance.HandledType() + " found");
							
						}
					}
				}
				
				
				
			}
		}
		
		public object GetValue()
		{
			LoadSerializers();
			return Deserialize();
		}
		
		static bool IsNumericType(System.Type o)
		{   
			
			#if UNITY_EDITOR || !UNITY_METRO
			
			switch (Type.GetTypeCode(o))
			{
			case TypeCode.Byte:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Single:
				return true;
			default:
				return false;
			}
			#else
			var ti = o.GetTypeInfo();
			if(ti.IsPrimitive)
			{
				if(o != typeof(IntPtr) && 
				   o != typeof(UIntPtr) &&
				   o != typeof(char) && 
				   o != typeof(bool))
					
					return true;
			}
			return false;
			#endif
		}
		static object ToNumericType(System.Type pType, string pInput)
		{   
			#if UNITY_EDITOR || !UNITY_METRO
			
			
			switch (Type.GetTypeCode(pType))
			{
			case TypeCode.Byte:
				return Byte.Parse(pInput);
			case TypeCode.SByte:
				return SByte.Parse(pInput);
			case TypeCode.UInt16:
				return UInt16.Parse(pInput);
			case TypeCode.UInt32:
				return UInt32.Parse(pInput);
			case TypeCode.UInt64:
				return UInt64.Parse(pInput);
			case TypeCode.Int16:
				return Int16.Parse(pInput);
			case TypeCode.Int32:
				return Int32.Parse(pInput);
			case TypeCode.Int64:
				return Int64.Parse(pInput);
			case TypeCode.Decimal:
				return Decimal.Parse(pInput);
			case TypeCode.Double:
				return Double.Parse(pInput);
			case TypeCode.Single:
				return Single.Parse(pInput);
			default:
				return pInput;
			}
			#else
			if(pType == typeof(byte))
			{
				return Byte.Parse(pInput);
			}else if(pType == typeof(SByte))
			{
				return SByte.Parse(pInput);
			}else if(pType == typeof(UInt16))
			{
				return UInt16.Parse(pInput);
			}else if(pType == typeof(UInt32))
			{
				return UInt32.Parse(pInput);
			}else if(pType == typeof(UInt64))
			{
				return UInt64.Parse(pInput);
			}else if(pType == typeof(decimal))
			{
				return Decimal.Parse(pInput);
			}else if(pType == typeof(double))
			{
				return Double.Parse(pInput);
			}else if(pType == typeof(Single))
			{
				return Single.Parse(pInput);
			}else 
			{
				return pInput;
			}
			
			#endif
		}
		
		public static System.Type[] GetInterfaces(System.Type pType)
		{
			#if UNITY_EDITOR || !UNITY_METRO			
			return pType.GetInterfaces();
			#else
			return pType.GetTypeInfo().ImplementedInterfaces.ToArray();
			#endif
		}
		
		public static PropertyInfo[] GetProperties(System.Type pType)
		{
			#if UNITY_EDITOR || !UNITY_METRO			
			return pType.GetProperties();
			#else
			return pType.GetTypeInfo().DeclaredProperties.ToArray();
			#endif
		}
		
		public static FieldInfo[] GetFields(System.Type pType)
		{
			#if UNITY_EDITOR || !UNITY_METRO			
			return pType.GetFields();
			#else
			return pType.GetTypeInfo().DeclaredFields.ToArray();
			#endif
		}
		
		public static bool IsClass(System.Type pType)
		{
			#if UNITY_EDITOR || !UNITY_METRO
			return pType.IsClass;
			#else
			return pType.GetTypeInfo().IsClass;
			#endif
		}
		
		
		public static System.Type[] GetTypes (Assembly assembly)
		{
			#if UNITY_EDITOR || !UNITY_METRO			
			return assembly.GetTypes();
			#else
			
			var a = assembly.DefinedTypes.ToArray();
			var o = new System.Type[a.Length];
			for(int i = 0;i< a.Length;i++)
			{
				o[i] = a[i].AsType();
			}
			return o;
			#endif
		}
		
		public static Assembly[] GetAssembly()
		{
			#if UNITY_EDITOR || !UNITY_METRO
			return AppDomain.CurrentDomain.GetAssemblies();
			#else
			Assembly[] a = new Assembly[1];
			System.Type t = typeof(int);
			a[0] = t.GetTypeInfo().Assembly;
			return a;
			#endif
		}
		
		public static object[] GetCustomAttributes (Type pType, Type pAttributeType, bool pInherited)
		{
			#if UNITY_EDITOR || !UNITY_METRO
			return pType.GetCustomAttributes(pAttributeType, pInherited);
			#else
			var tInfo = pType.GetTypeInfo();
			return tInfo.GetCustomAttributes(pAttributeType, pInherited).ToArray();
			#endif
			
		}
	}
}

