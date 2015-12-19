//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TweenRotation))]
public class TweenRotationEditor : UITweenerEditor
{
	public override void OnInspectorGUI ()
	{
		GUILayout.Space(6f);
		NGUIEditorTools.SetLabelWidth(120f);

		TweenRotation tw = target as TweenRotation;
		GUI.changed = false;

		Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
		Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("IsRotationByPoint");
		tw.isRotationByPoint = EditorGUILayout.Toggle(tw.isRotationByPoint);
		EditorGUILayout.EndHorizontal();
		Vector3 origin = Vector3.zero;
		float angle = 0;
		if (tw.isRotationByPoint)
		{
			origin = EditorGUILayout.Vector3Field("Origin", tw.origin);
			angle = EditorGUILayout.FloatField("Angle" , tw.angel);
		}
		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.from = from;
			tw.to = to;
			tw.origin = origin;
			tw.angel = angle;
			NGUITools.SetDirty(tw);
		}

		DrawCommonProperties();
	}
}
