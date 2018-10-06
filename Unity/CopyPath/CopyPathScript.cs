using UnityEngine;
using System.Collections;
using UnityEditor;
public class CopyPathScript : MonoBehaviour
{
    [MenuItem("GameObject/MyMenu/CopyFullPath", false, 0)]
    static void InitFullPath()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length == 1)
        {
            string pathStr = string.Empty;
            GetPath(Selection.gameObjects[0].transform, ref pathStr);

            TextEditor te = new TextEditor();
            te.text = pathStr;
            te.SelectAll();
            te.Copy();
          
        }
        else
        {
          
            Debug.LogError("请只选择一个物体进行复制路径;");
        }
    }
    // GetCompomentByPath<Text>("gongfaInfo/ScrollRect/Viewport/Content/gongfaAdd/txtShengMing/txtAttri");
    [MenuItem("GameObject/MyMenu/CopyTextByFullPath", false, 0)]
    static void InitFullByCommentTextPath()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length == 1)
        {
            string pathStr = string.Empty;
            GetPath(Selection.gameObjects[0].transform, ref pathStr);

            TextEditor te = new TextEditor();
            pathStr = @"GetCompomentByPath<Text>(" + "\"" + pathStr + "\"" + ");";
            te.text = pathStr;
            te.SelectAll();
            te.Copy();

        }
        else
        {
            Debug.LogError("请只选择一个物体进行复制路径;");
        }
    }
    [MenuItem("GameObject/MyMenu/CopyImageByFullPath", false, 0)]
    static void InitFullByCommentImagePath()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length == 1)
        {
            string pathStr = string.Empty;
            GetPath(Selection.gameObjects[0].transform, ref pathStr);

            TextEditor te = new TextEditor();
            pathStr = @"GetCompomentByPath<Image>("+"\"" + pathStr + "\""+");";
            te.text = pathStr;
            te.SelectAll();
            te.Copy();

        }
        else
        {

            Debug.LogError("请只选择一个物体进行复制路径;");
        }
    }
    [MenuItem("GameObject/MyMenu/CopyParentPath", false, 0)]
    static void InitParent()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length == 1)
        {
            TextEditor te = new TextEditor();
            if (Selection.gameObjects[0].transform.parent == null)
            {
                Debug.LogError("无父物体;");
            }
            else
            {
                string pathStr = string.Empty;
                GetPath(Selection.gameObjects[0].transform.parent, ref pathStr);
                te.text =pathStr;
                te.SelectAll();
                te.Copy();
             
            }
        }
        else
        {
           
            Debug.LogError("请只选择一个物体进行复制路径;");
        }
    }
    [MenuItem("GameObject/MyMenu/CopyName", false, 0)]
    static void InitName()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length == 1)
        {
            string pathStr = Selection.gameObjects[0].name;
            TextEditor te = new TextEditor();
            te.text =pathStr;
            te.SelectAll();
            te.Copy();
           
        }
        else
        {
            Debug.ClearDeveloperConsole();
            Debug.LogError("请只选择一个物体进行复制路径;");
        }
    }
    static string GetPath(Transform tr, ref string str)
    {

        if (tr != null)
        {
            str = tr.name + str;
            tr = tr.parent;
            if (tr != null)
            {
                str = "/" + str;
            }
            GetPath(tr, ref str);
        }
        else
        {
            return str;
        }
        return str;
    }
    [MenuItem("GameObject/MyMenu/CreateBindPoint", false, 0)]
    static void CreateBindPoint()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length == 1)
        {
            Transform parent = Selection.gameObjects[0].transform;
            GameObject bindObj = new GameObject();
            bindObj.transform.SetParent(parent, false);
            bindObj.name = "bindpoint";


            GameObject head = new GameObject();
            head.transform.SetParent(bindObj.transform, false);
            head.name = "head";

            GameObject body = new GameObject();
            body.transform.SetParent(bindObj.transform, false);
            body.name = "body";

            GameObject foot = new GameObject();
            foot.transform.SetParent(bindObj.transform, false);
            foot.name = "foot";
        }
        else
        {

            Debug.LogError("请只选择一个物体操作");
        }
    }

}


