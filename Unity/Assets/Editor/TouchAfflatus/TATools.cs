using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TouchAfflatus.com
{
    public class TATools : OdinMenuEditorWindow
    {
      
        private string name = "";
        [MenuItem("Tools/TouchAfflatus")]
        private static void Open()
        {
            var window = GetWindow<TATools>("个人工具箱");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            tree.Selection.SupportsMultiSelect = false;
            tree.Add("GeneralWindowCode", new GeneralWindowCode());
            tree.Add("Others",new OthersWindow());

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();
            
        }
    }

 
}
