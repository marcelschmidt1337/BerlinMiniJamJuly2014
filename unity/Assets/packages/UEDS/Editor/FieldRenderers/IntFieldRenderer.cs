using System;
using UnityEditor;

public class IntFieldRenderer : FieldRendererBase
{
	public IntFieldRenderer(): base(typeof(int))
	{
		
	}

	#region implemented abstract members of FieldRendererBase

	public override void OnEditorGUI (UEDS.Setting pSetting)
	{
		int val = (int)pSetting.GetValue();
		pSetting.SetValue( EditorGUILayout.IntField(
			pSetting.mSettingName,
			val) );

	}

	#endregion




}

