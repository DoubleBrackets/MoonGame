using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace MushiEditorTools.AssetCreationUtils
{
    public class ScriptTemplateUtility 
    {
    /// <summary>
    /// Create a new script using a template text file.
    /// </summary>
    /// <param name="templatePath">path to template text file</param>
    /// <param name="defaultPath"></param>
    /// <param name="nameSymbol">pattern to replace with the script name (includes any specified postfix/prefix)</param>
    /// <param name="rawNameSymbol">pattern to replace with the script name (excludes postfix/prefix)</param>
    /// <param name="appendPostfix">postfix to append to end of name (if user provided name does not contain)</param>
    /// <param name="appendPrefix">prefix to append to end of name (if user provided name does not contain)</param>
    public static void CreateScriptFromTemplate(
        string templatePath, 
        string defaultPath, 
        string nameSymbol = default,
        string rawNameSymbol = "", 
        string appendPostfix = "", 
        string appendPrefix = "")
    {
        // Set up end name action 
        var endAction = ScriptableObject.CreateInstance<CreateScriptTemplateAction>();
        endAction.nameSymbol = nameSymbol;
        endAction.rawNameSymbol = rawNameSymbol;
        endAction.appendPostFix = appendPostfix;
        endAction.appendPreFix = appendPrefix;
        // Begin name editing
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            endAction,
            defaultPath,
            (Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
            templatePath);
    }

    private class CreateScriptTemplateAction : EndNameEditAction
    {
        public string nameSymbol;
        public string rawNameSymbol;
        public string appendPostFix;
        public string appendPreFix;
        
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            // Text from template file
            string text = File.ReadAllText(resourceFile);

            // Name formatting
            string rawFileName = Path.GetFileName(pathName).Trim();
            string newFileName = rawFileName;
                
            // Append specified postfix (before extension)
            var splitFileName = newFileName.Split(".");
            if (!splitFileName[0].EndsWith(appendPostFix))
            {
                newFileName = newFileName.Replace(".", appendPostFix + ".");
            }

            // Append specified prefix
            if (!newFileName.StartsWith(appendPreFix))
            {
                newFileName = appendPreFix + newFileName;
            }
            
            // Update path name
            pathName = pathName.Replace(rawFileName, newFileName);

            // Swap out replacement symbol for formatted script name
            if (nameSymbol != "")
            {
                string fileNameWithoutExtension = newFileName.Split(".")[0];
                text = text.Replace(nameSymbol, fileNameWithoutExtension);
            }
            
            // Swap out raw replacement symbol for raw script name (with prefix/postfix trimmed)
            if (rawNameSymbol != "")
            {
                string fileNameWithoutExtension = rawFileName.Split(".")[0];
                
                // Trim out prefix/postfix
                fileNameWithoutExtension = TrimStart(fileNameWithoutExtension, appendPreFix);
                fileNameWithoutExtension = TrimEnd(fileNameWithoutExtension, appendPostFix);
                
                text = text.Replace(rawNameSymbol, fileNameWithoutExtension);
            }

            // Write processed text into the file
            string fullPath = Path.GetFullPath(pathName);
            var encoding = new UTF8Encoding(true);
            File.WriteAllText(fullPath, text, encoding);
            AssetDatabase.ImportAsset(pathName);
            ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object)));
        }

        private string TrimEnd(string toTrim, string trimString)
        {
            if (toTrim.EndsWith(trimString))
            {
                return toTrim.Substring(0, toTrim.Length - trimString.Length);
            }

            return toTrim;
        }
        
        private string TrimStart(string toTrim, string trimString)
        {
            if (toTrim.StartsWith(trimString))
            {
                return toTrim.Substring(trimString.Length, toTrim.Length - trimString.Length);
            }

            return toTrim;
        }

        public override void Cancelled(int instanceId, string pathName, string resourceFile)
        {
            base.Cancelled(instanceId, pathName, resourceFile);
        }
    }
}
}

