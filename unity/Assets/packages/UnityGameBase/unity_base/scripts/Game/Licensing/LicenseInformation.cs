using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.IO;


public class LicenseInformation
{
	private const string licensePath = "license/";
	
	[XmlAttribute("licenseName")]
	public string mLicenseName;
	
	[XmlAttribute("levelList")]
	public string mLevelList;
	
	[XmlAttribute("packageName")]
	public string mPackageName;
	
	[XmlAttribute("appDisplayName")]
	public string mAppDisplayName;
	
	[XmlAttribute("lExp")]
	public string mLExp;
	
	[XmlAttribute("lMod")]
	public string mLMod;
	
	[XmlAttribute("VersionCode")]
	public string mVCode;
	
	[XmlAttribute("VersionName")]
	public string mVName;
	
	[XmlAttribute("TargetStore")]
	public ETargetStore mTargetStore;
	

	
	public static LicenseInformation Load()
	{
		TextAsset ta = Resources.Load(licensePath + "current") as TextAsset;
		MemoryStream ms = new MemoryStream(ta.bytes);

		XmlSerializer serializer = new XmlSerializer(typeof(LicenseInformation));
		TextReader reader = new StreamReader(ms);
		LicenseInformation li = (LicenseInformation)serializer.Deserialize(reader);
		return li;
	}
#if UNITY_EDITOR
	public void Save()
	{
		string fileName = "Assets/Resources/" + licensePath +  mLicenseName + ".xml";
		
		if(Helpers.MakeSureFolderExists("Assets/Resources/" + licensePath))
		{
			try
			{
				Debug.Log("Writing to: " + fileName);
				XmlSerializer serializer = new XmlSerializer(typeof(LicenseInformation));
				TextWriter writer = new StreamWriter(fileName);
				
				serializer.Serialize(writer,this);
				
				writer.Flush();
				writer.Close();
				
			}catch(Exception e)
			{
				Debug.Log("Could not write License : " + e.Message);
			}
		}else{
			throw new DirectoryNotFoundException("Could not open License Directory: " + "Assets/Resources/" + licensePath);
		}
	}
#endif
}

