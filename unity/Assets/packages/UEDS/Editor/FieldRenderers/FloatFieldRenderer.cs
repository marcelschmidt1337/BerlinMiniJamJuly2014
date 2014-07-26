using System;
using UnityEditor;

public class FloatFieldRenderer : FieldRendererBase
{
	public FloatFieldRenderer(): base(typeof(float))
	{

	}

	#region implemented abstract members of FieldRendererBase

	public override void OnEditorGUI (UEDS.Setting pSetting)
	{

		pSetting.SetValue( EditorGUILayout.FloatField(
			pSetting.mSettingName,
			(float)pSetting.GetValue()) );

	}

	#endregion




}

