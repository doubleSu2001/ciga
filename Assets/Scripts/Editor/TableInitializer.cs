using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary>
/// 数据表初始化
/// </summary>
public class TableInitializer : AssetPostprocessor
{
    static void GeneratePuzzleTable(string csvfile, string toSaveFile)
    {
        // TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(csvfile);
        // PuzzleTable gm = ScriptableObject.CreateInstance<PuzzleTable>();
        //Debug.Log(data.text);
        // gm.mItems = CSVSerializer.Deserialize<PuzzleTable.Meta>(data.text);
        // AssetDatabase.CreateAsset(gm, toSaveFile);
        // AssetDatabase.SaveAssets();
        
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        //Debug.Log("start");
        // 物品表
        // GeneratePuzzleTable("Assets/Table/PuzzleTable.csv", "Assets/Resources/Table/PuzzleTable.asset");

    }
}
#endif