using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EditorTool
{
    [MenuItem("CustomTool/gotoMain")]
    public static void GotoMain()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/0.Main.unity");
    }
    [MenuItem("CustomTool/gotoUIDesign")]
    public static void GotoUIdesign()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Scenes/1.UIDesign.unity");
    }

    
    /// <summary>
    /// Project/Config => Resources/Config
    /// </summary>
    [MenuItem("CustomTool/Config/ConfigToResources")]
    public static void ConfigToResources()
    {
        /*
         * 找到目标路径 和 原路径
         * 清空目标路径
         * 把原路径内的所有文件 复制到目标路径 并添加扩展名
         * 强制刷新
         */


        var srcPath = Application.dataPath + "/../Config";
        var dstPath = Application.dataPath + "/Resources/Config";

        srcPath.CreateDirIfNotExists();
        dstPath.CreateDirIfNotExists();
        
        //清空目标路径
        Directory.Delete(dstPath, true);
        Directory.CreateDirectory(dstPath);

        foreach (var filePath in Directory.GetFiles(srcPath + "/"))
        {
            var fileName = filePath.Substring(filePath.LastIndexOf('/') + 1);
            File.Copy(filePath, dstPath + "/" + fileName + ".bytes", true);
        }

        AssetDatabase.Refresh();
        Debug.Log("表导入成功");
    }
}