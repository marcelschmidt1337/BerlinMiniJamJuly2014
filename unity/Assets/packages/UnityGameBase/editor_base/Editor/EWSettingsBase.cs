using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;

[Obsolete("Use UEDS instead")]
public class EWSettingsBase {
	
	public const string kSettingsRootPath = "/settings";
	
	[XmlAttribute]
	public string mPath
	{
		get;
		protected set;
	}
	
	public static T Load<T>(string pPath) where T : EWSettingsBase, new()
	{
		if(CheckFileExists(pPath))
		{
			try
			{
				Debug.Log("Reading XML Settings from: " + pPath + " for: " + typeof(T).ToString());
				
				XmlSerializer serializer = new XmlSerializer(typeof(T));
				TextReader reader = new StreamReader(Application.dataPath + pPath);
				T data = (T)serializer.Deserialize(reader);
				
				data.mPath = pPath;
				
				return data;
				
			}catch(Exception e)
			{
				EditorUtility.DisplayDialog("Error reading settings file",e.Message,"Dismiss");
				Debug.LogError("Error reading settings file: " + e.Message);
			}
		}else
		{
			Debug.Log("XML file not found. Creating new file: " + pPath);
			
			T data = new T();
			data.mPath = pPath;
			data.Save();
			return data;
		}
		return null;
	}
	
	public void Save()
	{
		Save(mPath);
	}
	
	public void Save(string pPath)
	{
		MakeSureFolderExists( Application.dataPath + kSettingsRootPath );
		
		try{
			XmlSerializer serializer = new XmlSerializer(this.GetType());
			TextWriter writer = new StreamWriter(Application.dataPath + pPath);
				
			serializer.Serialize(writer,this);
				
			writer.Flush();
			writer.Close();
				
		}catch(Exception e)
		{
			Debug.Log("Could not write to: " + mPath + " error: " + e.Message);
		}
	
	}
	
	static bool CheckFileExists(string pPath)
	{
		FileInfo fi = new FileInfo(Application.dataPath + pPath);
		return fi.Exists;
	}
	
	static bool MakeSureFolderExists(string pPath)
	{
		DirectoryInfo di = new DirectoryInfo(pPath);
		if(!di.Exists)
			di.Create();
		return true;
	}
}
