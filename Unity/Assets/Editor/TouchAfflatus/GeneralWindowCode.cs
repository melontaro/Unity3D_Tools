using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace TouchAfflatus.com
{
    internal class GeneralWindowCode
    {
        public GeneralWindowCode()
        {
        }
        //Window_xxx
        [SceneObjectsOnly] 
        [HideLabel]
        [Title("选择场景中的WindowUI")]
        public GameObject WindowGameObject;

        [ChildGameObjectsOnly(IncludeSelf = false)]
        public GameObject[] ChildGameObjects;


        [ChildGameObjectsOnly(IncludeSelf = false)]
        public GameObject ChildGameObjectItem;

        [PropertySpace(SpaceBefore = 30)]
        [HideLabel]
        [Title("选择要生成到的文件夹")]
        [FolderPath] 
        public String GeneralToFolderPath;

        public bool isCreateNewFolder = true;

        [HideLabel]
        [Title("是否生成PropsFile")]
        [PropertySpace(SpaceBefore = 30)]

        public bool isGeneralPropsFile=true;



        public String FileName;

        [PropertySpace(SpaceBefore = 30)]
        [Sirenix.OdinInspector.Button("生成UI代码",ButtonSizes.Large, ButtonStyle.FoldoutButton),GUIColor(0.3f, 0.8f, 1)]
        void GeneralCode()
        {
            if(this.WindowGameObject==null)return;
            if(String.IsNullOrEmpty(this.GeneralToFolderPath))return;
            string fullCodePath = System.Environment.CurrentDirectory + "/" + this.GeneralToFolderPath;
            if (this.isCreateNewFolder)
            {
                fullCodePath = fullCodePath + "/Window_" + this.FileName;
                if (!Directory.Exists(fullCodePath))
                {
                    Directory.CreateDirectory(fullCodePath);
                }

                String window_filePath = fullCodePath + "/Window_" + this.FileName + ".cs";
                File.Create(window_filePath);

                if (this.isGeneralPropsFile)
                {
                    String props_filePath = fullCodePath + "/Props_" + this.FileName + ".cs";
                    File.Create(props_filePath);
                }
               
            }
            

        }



    }
}