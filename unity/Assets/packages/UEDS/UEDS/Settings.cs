using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Reflection;
using System;

namespace UEDS
{
	
	public class SerializedRoot
	{
		public SerializedRoot()
		{}
		
		public List<UEDS.SerializedSettingsContainer> mContainers = new List<UEDS.SerializedSettingsContainer>();
		
		public SerializedSettingsContainer GetContainer (System.Type pType, string pInstanceName)
		{
			var container = mContainers.Find( (SerializedSettingsContainer obj) => {
				if(obj.mConcerningType == pType && pInstanceName == obj.mInstanceName)
					return true;
				return false;
			} );
			
			//
			// if none found, search for default instance
			//
			
			if(container == null)
			{
				
				container = mContainers.Find( (SerializedSettingsContainer obj) => {
					if(obj.mConcerningType == pType && pInstanceName == Settings.kDefaultInstanceName)
						return true;
					return false;
				} );
				
			}
			
			
			return container;
		}
		
	}
	
	public class SerializedSettingsContainer
	{
		public List<Setting> mSettings = new List<Setting>();
		
		public string ConcerningTypeString
		{
			get 
			{
				if(mConcerningType == null)
					return null;
				return mConcerningType.AssemblyQualifiedName;
			}
			set
			{
				if(value == null)
					mConcerningType = null;
				mConcerningType = System.Type.GetType(value);
			}
		}
		
		[XmlIgnore]
		public System.Type mConcerningType;
		
		public string mInstanceName;
		
		public void UpdateSettings (List<Setting> pSettings)
		{
			var savedSettings = new List<Setting>( mSettings );
			
			mSettings = new List<Setting>( ); 
			foreach(var c in pSettings)
			{
				mSettings.Add( new Setting(c) );
			}
			
			if(savedSettings.Count == 0)
				return;
			
			foreach(var s in mSettings)
			{
				var orgSetting = savedSettings.Find( (Setting obj) => { return s.Equals(obj); }) ;
				if(orgSetting != null)
					s.mValue = orgSetting.mValue;
				else
					Debug.LogError("Could not resolve setting " + s.DisplayName);
			}
			
		}		
		
		public SerializedSettingsContainer()
		{
			
		}
	}
	
	
	public static class Settings
	{
		public const string kDefaultFileName = "globalEditorSettings.xml";
		public const string kDefaultInstanceName = "__DEFAULT__";
		public const string kLogPrefix = "GS: ";
		public static SerializedRoot data
		{
			get 
			{
				return IO.GetData<SerializedRoot>();
			}
		}
		
		
		private static IGlobalEditorSettingsIOProvider mIO;
		private static IGlobalEditorSettingsIOProvider IO
		{
			get 
			{
				if(mIO == null)
				{
					Debug.Log(kLogPrefix + "No IO Provider set, using default implementation");
					
					foreach(var assembly in SerializedValue.GetAssembly())
					{
						foreach(var t in SerializedValue.GetTypes(assembly))
						{
							foreach(var attr in SerializedValue.GetCustomAttributes(t, typeof(EditorSettingIOProviderAttribute), true) )
							{
								if(attr != null)
								{
									int idx = Array.IndexOf( SerializedValue.GetInterfaces(t), typeof(IGlobalEditorSettingsIOProvider) );
									if(idx != -1)
									{
										Debug.Log(t);
										mIO = Activator.CreateInstance(t) as IGlobalEditorSettingsIOProvider;
									}
								}
							}
							
						}
					}
					if(mIO == null)
						Debug.LogError("Could not find default implementation of IO Provider!");
				}
				return mIO;
			}
		}
		
		public static SerializedSettingsContainer GetContainer (System.Type pType, string pInstanceName)
		{
			foreach( var c in GetContainersOfType(pType))
				if(c.mInstanceName == pInstanceName)
					return c;
			return null;
		}
		
		public static IEnumerable<SerializedSettingsContainer> GetContainersOfType(System.Type pType)
		{
			if(data == null)
			{
				Debug.LogError(kLogPrefix + "No data loaded. Did you call Init first?");
				yield break;
			}
			
			foreach(var c in data.mContainers)
			{
				if(c.mConcerningType == pType)
					yield return c;
			}
			
		}
		
		public static List<Setting> LoadSettings ( System.Type pType, string pInstanceName)
		{
			if(data == null)
			{
				Debug.LogError(kLogPrefix + "No data loaded. Did you call Init first?");
				return null;
			}
			
			
			var container = data.GetContainer(pType, pInstanceName);
			
			if(container == null)
			{
				//Debug.LogError(kLogPrefix + "No settings where found for this type. Did you save any settings at all? ");
				return null;
			}
			
			var list = new List<Setting>();
			foreach( var c in container.mSettings)
			{
				list.Add(new Setting(c));
			}
			
			return list;
			
		}
		
		
		
		
		
		#region public interface
		
		static string mFileName = Application.persistentDataPath + "/" + kDefaultFileName;
		public static string FileName
		{
			get
			{
				return mFileName;
			}
			set
			{
				mFileName = value;
			}
		}
		
		public static IEnumerator Init()
		{
			return Init<SerializedRoot>();
		}
		public static IEnumerator Init<T>() where T : SerializedRoot, new()
		{
			return Init<T>( FileName );
		}
		static IEnumerator Init<T>( string pPath ) where T : SerializedRoot, new()
		{
			var parms = new ExistCheckParams(pPath);
			
			var existCheck = IO.Exists(parms);
			
			while(existCheck.MoveNext())
				yield return 0;
			
			if(parms.mResult)
			{
				IO.Load<T>(pPath);
				
				
				
				while(!IO.LoaderFinished)
					yield return 0;
				
				if(IO.LoaderHasError)
				{
					IO.CreateEmpty<T>();
					Debug.LogError(kLogPrefix + IO.LoaderError);
				}
				
				Debug.Log(kLogPrefix + "Loader Done");
				yield break;
			}else
			{
				IO.CreateEmpty<T>();
				Debug.Log(kLogPrefix + " Creating empty instance");
				yield break;
				
			}
			
			
			
		}
		
		public static void InitInstanceWithGlobalSettings<T>( T instance, string pInstanceName ) where T : class
		{
			var settings = LoadSettings(typeof(T), pInstanceName);
			if(settings == null)
				return;
			
			System.Type t = instance.GetType();
			var properties = new List<PropertyInfo> ( SerializedValue.GetProperties(t) );
			var fields = new List<FieldInfo> ( SerializedValue.GetFields(t) );
			
			foreach(var s in settings)
			{
				if(s.mType == Setting.ESettingType.field)
				{
					FieldInfo fi = fields.Find((FieldInfo obj) => { return obj.Name == s.mSettingName; });
					fi.SetValue(instance, s.GetValue());
				}else if(s.mType == Setting.ESettingType.property)
				{
					PropertyInfo pi = properties.Find((PropertyInfo obj) => { return obj.Name == s.mSettingName; });
					pi.SetValue(instance, s.GetValue(),null);
				}
			}
			
		}
		
		
		public static void InitInstanceWithGlobalSettings<T>( T instance ) where T : class
		{
			InitInstanceWithGlobalSettings<T>(instance, kDefaultInstanceName );
		}
		
		public static void SetIOProvider(IGlobalEditorSettingsIOProvider pIOProvider)
		{
			mIO = pIOProvider;
		}
		
		
		public static IEnumerator Save()
		{
			
			IO.Save<SerializedRoot>(data, FileName);
			
			while( !IO.WriterFinished)
				yield return 0;
			
			if(IO.WriterHasError)
				Debug.LogError(IO.WriterError);
			
			yield break;
			
		}
		
		#endregion
		
		
		
	}
	
	
	
	
	
	
}