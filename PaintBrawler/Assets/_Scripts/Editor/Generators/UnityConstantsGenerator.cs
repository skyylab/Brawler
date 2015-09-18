namespace ConstantsGenerator {
    using UnityEditor;
    using System.IO;
    using System.Collections.Generic;

    public static class UnityConstantsGenerator {
        static Dictionary<int, string> SceneIdsToNames {
            get {
                var idsToNames = new Dictionary<int, string>();
                var scenes = EditorBuildSettings.scenes;
                for (int sceneId = 0; sceneId < scenes.Length; sceneId++) {
                    var scene = scenes[sceneId];
                    var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                    idsToNames.Add(sceneId, sceneName);
                }
                return idsToNames;
            }
        }

        const string _name = "UnityConstants";

        [MenuItem("Edit/Generate " + _name + ".cs")]
        public static void Generate () {
            using (var writer = new ConstantsWriter(_name)) {
                writer.WriteLine("namespace " + _name + " {"); // open namespace
                writer.Indent();

                writer.WriteLine("public static class Levels {"); // open levels
                writer.Indent();
                foreach (var sceneIdToName in SceneIdsToNames) {
                    var id = sceneIdToName.Key;
                    var name = sceneIdToName.Value;
                    writer.WriteLine("public const int {0} = {1};",
                                              ConstantsWriter.MakeSafeForCode(name),
                                              id);
                }
                writer.UnIndent();
                writer.WriteLine("}"); // close levels

                writer.WriteLine();

                writer.WriteLine("public enum LevelsE {"); // open levels enum
                writer.Indent();
                foreach (var sceneIdToName in SceneIdsToNames) {
                    var id = sceneIdToName.Key;
                    var name = sceneIdToName.Value;
                    writer.WriteLine("{0} = {1},",
                                              ConstantsWriter.MakeSafeForCode(name),
                                              id);
                }
                writer.UnIndent();
                writer.WriteLine("};"); // close levels enum

                writer.UnIndent();
                writer.Write("}"); // close namespace
            }
        }
    }
}