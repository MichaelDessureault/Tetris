using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shape))]
public class ShapeEditor : Editor {
    SerializedProperty typeProp;
    SerializedProperty currentRotationIndexProp;
    SerializedProperty rotationStateProp;
    SerializedProperty rotationStateArrayProp;
    SerializedProperty rotationArrayProp;
    SerializedProperty offSetListProp;
    SerializedProperty defaultRotationProp;

    private Shape targetShape;

    private void OnEnable() {
        targetShape = (Shape)target;

        typeProp = serializedObject.FindProperty("type");
        currentRotationIndexProp = serializedObject.FindProperty("currentRotationIndex");
        rotationStateProp = serializedObject.FindProperty("rotationState");
        rotationStateArrayProp = serializedObject.FindProperty("rotationStateList");
        rotationArrayProp = serializedObject.FindProperty("rotationList");
        offSetListProp = serializedObject.FindProperty("offSetList");
        defaultRotationProp = serializedObject.FindProperty("defaultRotation");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        // Display the script inside the inspector
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), typeof(Shape), false);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(typeProp);
        EditorGUILayout.PropertyField(defaultRotationProp);
        EditorGUILayout.PropertyField(currentRotationIndexProp);
        EditorGUILayout.PropertyField(rotationStateProp);

        //DisplayListTogether(rotationStateArrayProp, rotationArrayProp, offSetListProp);
       
        if (GUILayout.Button("Add to List")) {
            rotationStateArrayProp.arraySize++;
            rotationArrayProp.arraySize++;
            
            // offSetListProp is a 2d list
            offSetListProp.arraySize++;
        }

        InsertSpaces(2);

        if (GUILayout.Button("Clear All")) {
            targetShape.ClearAll();
        }

        InsertSpaces(2);

        for (int i = 0; i < rotationArrayProp.arraySize; i++) {
            EditorGUILayout.LabelField("Element (" + i.ToString() + ")");
            StartVertical();

            SerializedProperty sp = rotationStateArrayProp.GetArrayElementAtIndex(i);
            SerializedProperty sp2 = rotationArrayProp.GetArrayElementAtIndex(i);

            EditorGUILayout.PropertyField(sp, new GUIContent("Rotation State"));
            EditorGUILayout.PropertyField(sp2, new GUIContent("Rotation"));

            SerializedProperty sp3 = offSetListProp.GetArrayElementAtIndex(i);
            SerializedProperty sp3List = sp3.FindPropertyRelative("offsets");
            
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Rotation Offsets");
            if (GUILayout.Button("+", GUILayout.Width(30))) {
                sp3List.arraySize++;
            }
            if (GUILayout.Button("-", GUILayout.Width(30))) {
                sp3List.DeleteArrayElementAtIndex(sp3List.arraySize - 1);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            for (int j = 0; j < sp3List.arraySize; j++) {
                EditorGUILayout.PropertyField(sp3List.GetArrayElementAtIndex(j));
            }
            EditorGUI.indentLevel--;


            if (GUILayout.Button("Remove This Index (" + i.ToString() + ")")) {
                rotationStateArrayProp.DeleteArrayElementAtIndex(i);
                rotationArrayProp.DeleteArrayElementAtIndex(i);
                offSetListProp.DeleteArrayElementAtIndex(i);
            }

            EndVertical();
            InsertSpaces(1);
        }

        if (GUILayout.Button("Preview Rotation")) {
            targetShape.PreviewRotation();
        }

        if (GUILayout.Button("Preview Rotation Offsets")) {
            targetShape.PreviewOffSets();
        }

        //if (GUILayout.Button("Print")) {
        //    targetShape.Print();
        //}

        serializedObject.ApplyModifiedProperties();
    }
    
    //private int SmallestLength(SerializedProperty list1, SerializedProperty list2, SerializedProperty list3) {
    //    return Mathf.Min(list1.arraySize, list2.arraySize, list3.arraySize);
    //}

    #region GUI Helpers
    // Creates a vertical box in the inspector for graphic appearance
    protected void StartVertical() {
        GUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
    }

    protected void EndVertical() {
        EditorGUI.indentLevel--;
        GUILayout.EndVertical();
    }

    // Inserts space into the inspector based on the amount passed
    protected void InsertSpaces(int amount) {
        for (int i = 0; i < amount; i++) {
            EditorGUILayout.Space();
        }
    }
    #endregion

}
