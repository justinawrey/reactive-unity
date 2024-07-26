using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ReactiveUnity.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Reactive<>))]
    public class ReactiveVariablePropertyDrawer : PropertyDrawer
    {
        private SerializedProperty GetValueProperty(SerializedProperty property) =>
            property.FindPropertyRelative("_val");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(
                position,
                GetValueProperty(property),
                new GUIContent($"{label.text} [R]")
            );
            bool changed = EditorGUI.EndChangeCheck();

            if (changed)
            {
                property.serializedObject.ApplyModifiedProperties();
                ForceFlushCallbacks(property);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(GetValueProperty(property));
        }

        private void ForceFlushCallbacks(SerializedProperty property)
        {
            object targetObject = property.serializedObject.targetObject;
            FieldInfo fieldInfo = targetObject
                .GetType()
                .GetField(
                    property.propertyPath,
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public
                );
            object fieldValue = fieldInfo.GetValue(targetObject);

            Type reactiveVarType = fieldInfo.FieldType;
            MethodInfo methodInfo = reactiveVarType.BaseType.GetMethod(
                "ForceFlushCallbacks",
                BindingFlags.NonPublic | BindingFlags.Instance
            );
            methodInfo.Invoke(fieldValue, null);
        }
    }
}
