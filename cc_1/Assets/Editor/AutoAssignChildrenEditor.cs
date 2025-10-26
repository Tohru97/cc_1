using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class AutoAssignChildrenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Auto Assign Children"))
        {
            foreach (MonoBehaviour mono in targets)
            {
                Undo.RecordObject(mono, "Auto Assign Children");
                AssignChildrenToFields(mono);
            }
        }
    }

    private void AssignChildrenToFields(MonoBehaviour mono)
    {
        SerializedObject serializedObject = new SerializedObject(mono);
        SerializedProperty iterator = serializedObject.GetIterator();

        while (iterator.NextVisible(true))
        {
            if (iterator.propertyType == SerializedPropertyType.ObjectReference && 
                iterator.objectReferenceValue == null)
            {
                string targetName = iterator.name;
                System.Type componentType = GetFieldType(mono.GetType(), iterator.name);

                Transform child = mono.GetComponentsInChildren<Transform>(true)
                    .FirstOrDefault(t => t.name == targetName);

                if (child != null)
                {
                    if (componentType == typeof(GameObject))
                    {
                        iterator.objectReferenceValue = child.gameObject;
                    }
                    else
                    {
                        Component component = componentType != null ? 
                            child.GetComponent(componentType) : 
                            child;

                        if (component != null)
                        {
                            iterator.objectReferenceValue = component;
                        }
                    }

                    if (iterator.objectReferenceValue != null)
                    {
                        Debug.Log($"할당 성공: {mono.name} → {targetName} ({componentType?.Name})", mono);
                    }
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private System.Type GetFieldType(System.Type classType, string fieldName)
    {
        var fieldInfo = classType.GetField(fieldName, 
            BindingFlags.NonPublic | 
            BindingFlags.Public | 
            BindingFlags.Instance);

        return fieldInfo?.FieldType;
    }
}
