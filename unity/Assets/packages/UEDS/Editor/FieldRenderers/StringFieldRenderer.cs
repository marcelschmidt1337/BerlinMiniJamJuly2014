using System;
using UnityEditor;

public class StringFieldRenderer : FieldRendererBase
{
	public StringFieldRenderer(): base(typeof(string))
	{
		
	}
	
	#region implemented abstract members of FieldRendererBase
	
	public override void OnEditorGUI (UEDS.Setting pSetting)
	{
		string val = (string)pSetting.GetValue();
		pSetting.SetValue( EditorGUILayout.TextField(
			pSetting.mSettingName,
			val) );
		
	}
	
	#endregion
	
	
	
	
}

