using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace WebGLSupport
{
    public class Postprocessor
    {
        const string MenuPath = "Assets/WebGLSupport/OverwriteFullscreenButton";

        private const string DefaultFullscreenFunc = "unityInstance.SetFullscreen(1);";
        private const string FullscreenNode = "unity-container";

        private static bool IsEnable => PlayerPrefs.GetInt(MenuPath, 1) == 1;

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.WebGL) return;
            if (!IsEnable) return;

            var path = Path.Combine(pathToBuiltProject, "index.html");
            if (!File.Exists(path)) return;

            var html = File.ReadAllText(path);

            // check node is exist
            if (html.Contains(FullscreenNode))
            {
                html = html.Replace(DefaultFullscreenFunc, $"document.makeFullscreen('{FullscreenNode}');");
                File.WriteAllText(path, html);
            }
        }

        [MenuItem(MenuPath)]
        public static void OverwriteDefaultFullscreenButton()
        {
            var flag = !Menu.GetChecked(MenuPath);
            Menu.SetChecked(MenuPath, flag);
            PlayerPrefs.SetInt(MenuPath, flag ? 1 : 0);
        }

        [MenuItem(MenuPath, validate = true)]
        private static bool OverwriteDefaultFullscreenButtonValidator()
        {
            Menu.SetChecked(MenuPath, IsEnable);
            return true;
        }
    }
}
