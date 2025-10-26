using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateMonoBehaviourExtensionScript
{
    private const string NewScriptTemplate = @"using UnityEngine;

public class #SCRIPTNAME# : MonoBehaviourExtension
{
    public override void Awake()
    {
        
    }

    public override void Init()
    {

    }
    
    public override void Subscribe()
    {

    }

    public override void Unsubscribe()
    {

    }

    public override void Show()
    {

    }

    public override void Hide()
    {

    }

    public override void Reset()
    {

    }
}
";

    [MenuItem("Assets/Create/C# MonoBehaviourExtension Script", false, 80)]
    public static void CreateScript()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        var endNameEditAction = ScriptableObject.CreateInstance<MyEndNameEditAction>();
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, endNameEditAction, "NewMonoBehaviourExtension.cs", null, null);
    }

    class MyEndNameEditAction : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {            
            string className = Path.GetFileNameWithoutExtension(pathName);
            string scriptContent = NewScriptTemplate.Replace("#SCRIPTNAME#", className);

            File.WriteAllText(pathName, scriptContent);
            AssetDatabase.ImportAsset(pathName);
            
            Object o = AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
            ProjectWindowUtil.ShowCreatedAsset(o);
        }
    }
}