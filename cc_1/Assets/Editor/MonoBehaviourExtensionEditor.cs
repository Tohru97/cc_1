using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class MonoBehaviourExtensionCreatorWindow : EditorWindow
{
    private string className = "NewMonoBehaviourExtension";
    private string folderPath = "Assets";
    private string statusMessage = "";

    [MenuItem("Assets/Create/MonoBehaviourExtension Script", false, 80)]
    public static void ShowWindow()
    {
        var window = GetWindow<MonoBehaviourExtensionCreatorWindow>("MonoBehaviourExtension 생성기");
        window.minSize = new Vector2(400, 140);
        window.UpdateFolderPath();
    }

    private void OnGUI()
    {
        GUILayout.Label("MonoBehaviourExtension 상속 스크립트 생성", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        // 클래스명 입력
        className = EditorGUILayout.TextField("클래스명", className);

        // 생성 위치 표시
        EditorGUILayout.LabelField("생성 위치", folderPath);

        // 상태 메시지
        if (!string.IsNullOrEmpty(statusMessage))
        {
            EditorGUILayout.HelpBox(statusMessage, MessageType.Info);
        }

        EditorGUILayout.Space();

        // 생성 버튼
        if (GUILayout.Button("생성"))
        {
            if (!IsValidClassName(className))
            {
                statusMessage = "유효한 클래스명을 입력하세요. (영문, 숫자, _ 만 사용, 첫 글자는 영문/_)";
            }
            else
            {
                CreateScript();
            }
        }
    }

    private void UpdateFolderPath()
    {
        folderPath = "Assets";
        if (Selection.activeObject != null)
        {
            folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!Directory.Exists(folderPath))
                folderPath = Path.GetDirectoryName(folderPath);
        }
    }

    private void CreateScript()
    {
        // 파일명 중복 처리
        string fileName = className + ".cs";
        int count = 1;
        while (File.Exists(Path.Combine(folderPath, fileName)))
        {
            fileName = $"{className}{count}.cs";
            count++;
        }

        string filePath = Path.Combine(folderPath, fileName);

        // 템플릿 생성 및 파일 저장
        string scriptContent = GenerateScriptTemplate(className);
        File.WriteAllText(filePath, scriptContent, Encoding.UTF8);
        AssetDatabase.Refresh();

        // 생성된 파일 선택
        var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(filePath);
        Selection.activeObject = asset;
        EditorGUIUtility.PingObject(asset);

        statusMessage = $"생성 완료: {fileName}";
    }

    // 클래스명 유효성 검사 (C# 규칙 간단 적용)
    private static bool IsValidClassName(string name)
    {
        if (string.IsNullOrEmpty(name)) return false;
        if (!char.IsLetter(name[0]) && name[0] != '_') return false;
        foreach (char c in name)
        {
            if (!char.IsLetterOrDigit(c) && c != '_') return false;
        }
        return true;
    }

    private static string GenerateScriptTemplate(string className)
    {
        return
$@"using UnityEngine;

public class {className} : MonoBehaviourExtension
{{
    public override void Init()
    {{
        // 초기화 코드 작성
    }}

    public override void Subscribe()
    {{
        // 이벤트 구독 코드 작성
    }}

    public override void Unsubscribe()
    {{
        // 이벤트 구독 해제 코드 작성
    }}

    public override void Show()
    {{
        // 표시 관련 코드 작성
    }}

    public override void Hide()
    {{
        // 숨기기 관련 코드 작성
    }}

    public override void Reset()
    {{
        // 리셋 관련 코드 작성
    }}
}}
";
    }
}
