using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

namespace UEDS
{
	public class SettingsProxy
	{
		List<SettingsContainer> mContainerAttributes;
			
		
		public SettingsProxy ()
		{

			//
			// find all attributes in code
			//
			
			FindAllContainerAttributes();
			
			FindAllSettingAttributesInContainers();
		}

		public void DelInstance(SerializedSettingsContainer pContainer)
		{


			if(Settings.data.mContainers.Contains( pContainer ))
				Settings.data.mContainers.Remove( pContainer );

		}

		public SerializedSettingsContainer DupInstance (SerializedSettingsContainer mSelectedContainer)
		{
			var nContainer = AddInstance(mSelectedContainer.mConcerningType, mSelectedContainer.mInstanceName + " dup");
			nContainer.mSettings = new List<Setting>();
			foreach(var s in mSelectedContainer.mSettings)
				nContainer.mSettings.Add( new Setting(s) );
			return nContainer;
		}

		public SerializedSettingsContainer AddInstance (System.Type pType, string pInstance)
		{
			SerializedSettingsContainer ssc = new SerializedSettingsContainer();
			ssc.mConcerningType = pType;
			ssc.mInstanceName = pInstance;

			Settings.data.mContainers.Add(ssc);
			return ssc;
		}
		
		public SettingsContainer GetContainerForType(System.Type pType)
		{
			foreach(var c in mContainerAttributes)
			{
				if(c.mType == pType)
				{
					return c;
				}
			}
			return null;
		}
		
		public IEnumerable<SettingsContainer> settingContainers
		{
			get
			{
				return mContainerAttributes;
			}
		}


		
		void FindAllContainerAttributes()
		{
			mContainerAttributes = new List<SettingsContainer>();
			foreach(var attribute in GetAllAttributes<EditorSettingsContainerAttribute,System.Type>( System.AttributeTargets.Class | System.AttributeTargets.Struct ))
			{
				mContainerAttributes.Add(new SettingsContainer(attribute.Key, attribute.Value));
			}
		
			UnityEngine.Debug.Log("Found " + mContainerAttributes.Count + " Containers");
		}
		
		void FindAllSettingAttributesInContainers()
		{
			foreach( var container in mContainerAttributes)
			{
				foreach( var attribute in GetAllAttributes<EditorSettingAttribute, MemberInfo >( System.AttributeTargets.Property | System.AttributeTargets.Field, container.mType))
				{
					if(attribute.Value is FieldInfo)
						container.mSettings.Add(SettingFactory.CreateFromField(attribute.Key, attribute.Value as FieldInfo));
					else if (attribute.Value is PropertyInfo)
						container.mSettings.Add(SettingFactory.CreateFromProperty(attribute.Key, attribute.Value as PropertyInfo));
				}
			
			}

		}




		
		IEnumerable<KeyValuePair<T,TX>> GetAllAttributes<T,TX>( System.AttributeTargets pTarget, System.Type pBaseType = null ) where T : System.Attribute where TX : class
		{
			bool searchClass 		= (pTarget & System.AttributeTargets.Class) == System.AttributeTargets.Class;
			bool searchStruct		= (pTarget & System.AttributeTargets.Struct) == System.AttributeTargets.Struct;
			bool searchProperty 	= (pTarget & System.AttributeTargets.Property) == System.AttributeTargets.Property;
			bool searchField 		= (pTarget & System.AttributeTargets.Field) == System.AttributeTargets.Field;
			
			foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(var t in assembly.GetTypes())
				{
					if( t.IsClass && searchClass || searchStruct )
					{
						foreach( var attribute in t.GetCustomAttributes(typeof(T),true))
						{
							yield return new KeyValuePair<T, TX>(attribute as T, t as TX);
						}
					}
					
					if(t.IsClass && t == pBaseType)
					{
						if(searchField)
						{
							foreach( var field in t.GetFields())
							{
								foreach( var attribute in field.GetCustomAttributes(typeof(T),true))
								{
									yield return new KeyValuePair<T, TX>( attribute as T, field as TX);
								}
							}
						}
						if(searchProperty)
						{
							foreach( var property in t.GetProperties())
							{
								foreach( var attribute in property.GetCustomAttributes(typeof(T),true))
								{
									yield return new KeyValuePair<T, TX>( attribute as T, property as TX);
								}
							}
						}
					}
					
				}
			}
		}

		public string[] GetInstanceNames (System.Type pContainerName)
		{

			List<string> mString = new List<string>();


			bool defaultFound = false;
			foreach ( var c in Settings.GetContainersOfType( pContainerName ) )
			{
				if(c.mInstanceName == Settings.kDefaultInstanceName)
					defaultFound = true;
				mString.Add(c.mInstanceName);
			}

			if(!defaultFound)
				mString.Add( Settings.kDefaultInstanceName );

			return mString.ToArray();
		}

		public IEnumerator Save (string fileName)
		{
			Settings.FileName = fileName;
			return Settings.Save();
		}

	}
	
	
	
	internal static class SettingFactory
	{
		static Setting CreateSetting(EditorSettingAttribute pAttribute)
		{
			Setting s = new Setting(pAttribute);
			return s;
		}
			
		public static Setting CreateFromProperty(EditorSettingAttribute pAttribute, PropertyInfo pInfo)
		{
			Setting s = CreateSetting(pAttribute);
			s.mType = Setting.ESettingType.property;
			s.mValueType = pInfo.PropertyType;
			s.mSettingName = pInfo.Name;
			return s;
		}
		
		public static Setting CreateFromField(EditorSettingAttribute pAttribute, FieldInfo pInfo)
		{
			Setting s = CreateSetting(pAttribute);
			s.mType = Setting.ESettingType.field;
			s.mValueType = pInfo.FieldType;
			s.mSettingName = pInfo.Name;
			
			return s;
		}
	}
	
	
	

	/// <summary>
	/// Represents a class with settings
	/// </summary>
	public sealed class SettingsContainer
	{
		UnityEngine.GUIContent mDisplayGizmo;
		public EditorSettingsContainerAttribute mAttribute;
		public Type mType;
		
		public List<Setting>mSettings = new List<Setting>();
		
		public SettingsContainer(EditorSettingsContainerAttribute pAttribute, Type pType)
		{
			mType = pType;
			mAttribute = pAttribute;
		}
		
		
		public string DisplayName
		{
			get
			{
				if(mAttribute != null && mAttribute.title != null)
					return mAttribute.title;
					
				return mType.ToString();
				
			}
		}
		
		public string DisplayDescription
		{
			get
			{
				if(mAttribute != null & mAttribute.description != null)
					return mAttribute.description;
				return "No Description available";
			}
		}
		
		public UnityEngine.GUIContent DisplayGizmo
		{
			get
			{
				if(mDisplayGizmo == null)
				{
					mDisplayGizmo = new UnityEngine.GUIContent();
					
					if(mAttribute != null && mAttribute.gizmo != null)
					{
						UnityEngine.Texture2D t = UnityEditor.EditorGUIUtility.FindTexture( mAttribute.gizmo);
						if(t == null)
							UnityEngine.Debug.LogError("Could not find Gizmo at: " + mAttribute.gizmo);
						mDisplayGizmo.image = t;
					}
				}
				
				return mDisplayGizmo;
			}
			
		}
	}
}