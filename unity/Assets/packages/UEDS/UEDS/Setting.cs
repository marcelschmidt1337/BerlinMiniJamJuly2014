using System;
using UnityEngine;
using System.Xml.Serialization;
using System.Reflection;

namespace UEDS
{
	
	/// <summary>
	/// Represents a single setting within a class. 
	/// </summary>
	public sealed class Setting
	{
		public enum ESettingType
		{
			property,
			field
		}
		[XmlIgnore]
		public bool isDirty{ get; private set;}
		
		
		#pragma warning disable 649
		public ESettingType mType;
		
		public string ValueTypeString
		{
			get
			{
				if(mValueType == null)
					return null;
				return mValueType.AssemblyQualifiedName;
			}
			set
			{
				if(value == null)
				{
					mValueType = null;
				}
				else
				{
					mValueType = Type.GetType(value);
				}
			}
		}
		
		[XmlIgnore]
		public Type mValueType;
		
		public string mSettingName;
		#pragma warning restore
		
		
		[XmlIgnore]
		public object mValue;
		
		
		public SerializedValue ValueProperty
		{
			get
			{
				if(GetValue() != null)
				{
					return new SerializedValue(GetValue(), mValueType);
				}
				return null;
			}
			set
			{
				if(value != null)
					mValue = value.GetValue();
			}
		}
		
		public EditorSettingAttribute mAttribute;
		
		public string DisplayName
		{
			get
			{
				if(mAttribute.title != null)
					return mAttribute.title;
				return mSettingName;
			}
		}
		
		public string Description
		{
			get 
			{
				return mAttribute.description;
			}
			
		}
		
		public Setting()
		{
			
		}
		
		public Setting(EditorSettingAttribute pAttribute)
		{
			mAttribute = pAttribute;
		}
		
		public Setting(Setting pOriginal)
		{
			mType = pOriginal.mType;
			mValueType = pOriginal.mValueType;
			mValue = pOriginal.mValue;
			mSettingName = pOriginal.mSettingName;
			
			mAttribute = pOriginal.mAttribute;
		}
		
		public void SetValue(object pValue)
		{
			if(pValue != mValue)
			{
				isDirty = true;
			}
			mValue = pValue;
		}
		
		public object GetValue()
		{
			
			if(mValue != null)
				return mValue;
			else
				return GetDefault(mValueType);
		}
		
		
		public object GetDefault(Type t)
		{
			#if UNITY_EDITOR || !UNITY_METRO
			return this.GetType().GetMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
			#else
			return this.GetType().GetTypeInfo().GetDeclaredMethod("GetDefaultGeneric").MakeGenericMethod(t).Invoke(this, null);
			#endif
		}
		
		public T GetDefaultGeneric<T>()
		{
			return default(T);
		}
		
		public override bool Equals (object obj)
		{
			if(!(obj is Setting))
				return false;
			
			Setting other = (obj as Setting);
			if(other.mValueType == mValueType)
				if(other.mSettingName == mSettingName)
					return true;
			
			return base.Equals (obj);
		}
		
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
		
	}
	
}

