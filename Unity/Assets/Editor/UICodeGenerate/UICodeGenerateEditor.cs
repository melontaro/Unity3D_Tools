using System;
using System.Collections.Generic;
using System.IO;
using ETModel;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class UICodeGeneratetor: EditorWindow
{
    /// <summary>
    /// 使用方法:
    /// 把需要生成的UI组件拉如RC里面, 命名规则是: 类型_名字 如: Text_HeroLevel
    /// 然后根据需要填写namespace和ui继承对象, 输出路径等等 (根据自己需求修改)
    /// </summary>

    private string nameSpace = "ETHotfix";
    private string outputPath = "";
    private string scriptName = "";
    private string uiBaseComponentName = "UIBaseComponent";
    private string[] usingArray = { "ETModel", "UnityEngine", "UnityEngine.UI" }; // 根据自己需要添加

    [MenuItem("CG工具/生成UI代码")]
    public static void ShowWindow()
    {
        var window = GetWindow<UICodeGeneratetor>();
        window.minSize = new Vector2(800, 300);
        window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 200, 300);
    }

    public void OnGUI()
    {
        nameSpace = EditorGUILayout.TextField("NameSpace:", nameSpace);
        outputPath = EditorGUILayout.TextField("代码输出路径:", outputPath);
        scriptName = EditorGUILayout.TextField("脚本命名:", scriptName);
        uiBaseComponentName = EditorGUILayout.TextField("UI继承类名字:", uiBaseComponentName);
        GameObject go = Selection.activeGameObject;

        GUILayout.Label(" ");
        GUILayout.Label("请从Hierarchy里选择需要生成的物体!");
        GUILayout.Label(" ");

        if (go != null)
        {
            GUILayout.Label("当前选中的物体: " + Selection.activeGameObject.name);
            GUILayout.Label(" ");

            if (GUILayout.Button("生成RC持有的组件代码"))
            {
                if (scriptName == "")
                {
                    Log.Debug("请填写脚本名字!");
                    return;
                }

                if (outputPath == "")
                {
                    Log.Debug("请填写输出路径!");
                    return;
                }

                ReferenceCollector rc = go.GetComponent<ReferenceCollector>();
                Dictionary<string, Object> dict = rc.GetAll();

                Dictionary<string, string> resultDict = new Dictionary<string, string>();

                foreach (var obj in dict)
                {
                    string[] key = obj.Key.Split('_');

                    if(key.Length >= 3)
                    {
                        Log.Debug(obj.Key + " 分割线超出2个");
                    }
                    
                    if (key.Length < 2)
                    {
                        Log.Debug("注意: " + obj.Key + " 分割不足2个");
                    }
                    else
                    {
                        if (resultDict.ContainsKey(key[1]))
                        {
                            Log.Error("错误 - 发现重名: " + obj.Key);
                            break;
                        }

                        resultDict.Add(key[1], key[0]);
                    }
                }

                if (resultDict.Count > 0)
                {
                    // 覆盖清空文本
                    using (new FileStream(Path.Combine(this.outputPath, this.scriptName + ".cs"), FileMode.Create))
                    {
                    }
                    
                    using (StreamWriter sw = new StreamWriter(Path.Combine(this.outputPath, this.scriptName + ".cs")))
                    {
                        for (int i = 0; i < usingArray.Length; i++)
                        {
                            sw.WriteLine("using " + usingArray[i] + ";");
                        }

                        sw.WriteLine("// Code Generate By Tool : " + DateTime.Now);
                        sw.WriteLine("\nnamespace " + this.nameSpace);
                        sw.WriteLine("{");

                        //

                        sw.WriteLine("\t[ObjectSystem]");
                        sw.WriteLine("\tpublic class " + this.scriptName + "System : AwakeSystem<" + this.scriptName + ">");
                        sw.WriteLine("\t{");
                        sw.WriteLine("\t\tpublic override void Awake(" + this.scriptName + " self)");
                        sw.WriteLine("\t\t{");
                        sw.WriteLine("\t\t\tself.Awake();");
                        sw.WriteLine("\t\t}");
                        sw.WriteLine("\t}");
                        
                        sw.WriteLine("\n\tpublic class " + this.scriptName + " : " + this.uiBaseComponentName);
                        sw.WriteLine("\t{");

                        sw.WriteLine("\t\t#region Property");

                        // 属性
                        foreach (var result in resultDict)
                        {
                            sw.WriteLine("\t\tprivate " + result.Value + " " + result.Value + "_" + result.Key + ";");
                        }

                        sw.WriteLine("\t\t#endregion");
                        
                        // Awake
                        sw.WriteLine("\n\t\tpublic void Awake()");
                        sw.WriteLine("\t\t{");

                        sw.WriteLine("\t\t\tReferenceCollector rc = this.GetParent<UI_Z>().GameObject.GetComponent<ReferenceCollector>();");

                        // 获取组件
                        foreach (var result in resultDict)
                        {
                            // 这里根据自己需要的写法去获得RC身上的组件
                            string _name = result.Value + "_" + result.Key;
                            if (result.Value == "GameObject")
                            {
                                sw.WriteLine("\t\t\tthis." + _name + " = rc.Get<GameObject>(" + '"' + _name + '"' + ");");
                            }
                            else
                            {
                                sw.WriteLine("\t\t\tthis." + _name + " = UIHelper.Get<" + result.Value + ">(rc, " + ('"' + _name + '"') + ");");
                            }
                        }

                        // 绑定按钮事件
                        sw.WriteLine("\n\t\t\t// 绑定按钮点击回调");
                        foreach (var result in resultDict)
                        {
                            if (result.Value == "Button")
                            {
                                string _name = result.Value + "_" + result.Key;
                                sw.WriteLine("\t\t\tthis." + _name + ".onClick.Add(On" + _name + "Click);");
                            }
                        }
                        
                        sw.WriteLine("\t\t}");
                        
                        // 生成按钮绑定的对应方法
                        foreach (var result in resultDict)
                        {
                            if (result.Value == "Button")
                            {
                                string _name = result.Value + "_" + result.Key;
                                sw.WriteLine("\n\t\tprivate void On" + _name + "Click()");
                                sw.WriteLine("\t\t{");
                                sw.WriteLine("\t\t}");
                            }
                        }

                        sw.WriteLine("\t}");

                        //

                        sw.WriteLine("}");
                    }

                    Log.Debug("Job Done!");
                }
            }
        }
    }

    private void OnSelectionChange()
    {
        // 改变后自动刷新
        Repaint();
    }
}
