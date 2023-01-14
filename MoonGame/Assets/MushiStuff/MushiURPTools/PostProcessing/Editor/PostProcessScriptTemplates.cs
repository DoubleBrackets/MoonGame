using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MushiEditorTools.AssetCreationUtils;
using UnityEditor;

namespace MushiURPTools.PostProcessing
{
    public class PostProcessScriptTemplates
    {
        private static string path = "Assets/MushiStuff/MushiURPTools/PostProcessing/Editor";

        [MenuItem("Assets/Create/MushiStuff/MushiURPTools/PostProcessing/Custom Volume Component Script", false, 0)]
        public static void CreateVolumeComponentScript()
        {
            ScriptTemplateUtility.CreateScriptFromTemplate(
                $"{path}/VolumeComponentTemplate.txt",
                "CustomVolumeComponent.cs",
                "#FILENAME#"
            );
        }
    
        [MenuItem("Assets/Create/MushiStuff/MushiURPTools/PostProcessing/Simple Render Feature Script", false, 0)]
        public static void CreateRenderFeatureScript()
        {
            ScriptTemplateUtility.CreateScriptFromTemplate(
                $"{path}/PPRenderFeatureTemplate.txt",
                "CustomRenderFeature.cs",
                "#FILENAME#"
            );
        }
    
        [MenuItem("Assets/Create/MushiStuff/MushiURPTools/PostProcessing/Simple Render Pass Script", false, 0)]
        public static void CreateRenderPassScript()
        {
            ScriptTemplateUtility.CreateScriptFromTemplate(
                $"{path}/PPRenderPassTemplate.txt",
                "CustomRenderPass.cs",
                "#FILENAME#"
            );
        }
    }
}

