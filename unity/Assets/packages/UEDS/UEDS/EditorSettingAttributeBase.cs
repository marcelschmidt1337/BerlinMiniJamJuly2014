using System;

namespace UEDS
{
	
	public class EditorSettingAttributeBase : System.Attribute
	{
		public string title{ get; set;}
		public string description { get; set;}


	}
	
	[System.AttributeUsage(System.AttributeTargets.Class |
		System.AttributeTargets.Struct)]
	public class EditorSettingsContainerAttribute : EditorSettingAttributeBase
	{
		public string gizmo{ get; set;}

		public EditorSettingsContainerAttribute(){}

	}
	
	[System.AttributeUsage(System.AttributeTargets.Field | 
		System.AttributeTargets.Property)]
	public class EditorSettingAttribute : EditorSettingAttributeBase
	{
		public object defaultValue { get; set;}
		public EditorSettingAttribute(){}
	}

	[System.AttributeUsage(System.AttributeTargets.Class)]
	public class EditorSettingIOProviderAttribute : System.Attribute
	{

	}
}

