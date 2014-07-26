using System;
using UnityEditor;
using UnityEngine;
using UEDS;

public class ColorFieldRenderer : FieldRendererBase
{

	public ColorFieldRenderer(): base(typeof(UnityEngine.Color))
	{
		
	}

	#region implemented abstract members of FieldRendererBase

	public override void OnEditorGUI (UEDS.Setting pSetting)
	{
		pSetting.SetValue( EditorGUILayout.ColorField(
			pSetting.DisplayName ,
			(UnityEngine.Color)pSetting.GetValue()) );
	}

	#endregion




}

