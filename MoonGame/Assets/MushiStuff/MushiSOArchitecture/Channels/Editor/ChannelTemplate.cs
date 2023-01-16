using MushiEditorTools.AssetCreationUtils;
using UnityEditor;
using UnityEngine;

public class ChannelTemplate
{
    private static string path = "Assets/MushiStuff/MushiSOArchitecture/Channels/Editor";

    [MenuItem("Assets/Create/MushiStuff/MushiSOArchitecture/Event Channel Script", false, 0)]
    public static void CreateEventChannelScript()
    {
        ScriptTemplateUtility.CreateScriptFromTemplate(
            $"{path}/EventChannelScriptTemplate.txt",
            "EventChannelSO.cs",
            "#FILENAME#",
            "#RAWFILENAME#",
            "EventChannelSO"
        );
    }
    
    [MenuItem("Assets/Create/MushiStuff/MushiSOArchitecture/Func Channel Script", false, 0)]
    public static void CreateFuncChannelScript()
    {
        ScriptTemplateUtility.CreateScriptFromTemplate(
            $"{path}/FuncChannelScriptTemplate.txt",
            "ChannelSO.cs",
            "#FILENAME#",
            "#RAWFILENAME#",
            "FuncChannelSO"
        );
    }
}
