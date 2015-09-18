namespace ConstantsGenerator {
    using UnityEngine;
    using UnityEditor;
    using System.Collections.Generic;
    using Rewired;

    public static class RewiredConstantsGenerator {

        private static void WriteCategorySummary (ConstantsWriter writer, InputCategory ic) {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Descriptive Name: {0}", ic.descriptiveName);
            writer.WriteLine("/// ID: {0}", ic.id);
            writer.WriteLine("/// </summary>");
        }

        private static void WriteActionSummary (ConstantsWriter writer, InputAction action) {
            writer.WriteLine("/// <summary>");
            writer.WriteLine("/// Descriptive Name: {0}", action.descriptiveName);
            writer.WriteLine("/// Type: {0}", action.type);
            if (action.type == InputActionType.Axis) {
                writer.WriteLine("/// Positive Axis: {0}", action.positiveDescriptiveName);
                writer.WriteLine("/// Negative Axis: {0}", action.negativeDescriptiveName);
            }
            writer.WriteLine("/// </summary>");
        }

        private static InputManager InputManager {
            get {
                return (Object.FindObjectOfType<LaunchRewiredOnAwake>() != null) ?
                    Object.FindObjectOfType<LaunchRewiredOnAwake>().InputManager :
                    Object.FindObjectOfType<InputManager>();
            }
        }

        private static Dictionary<InputCategory, List<InputAction>> CategoriesToActions {
            get {

                var inputManager = InputManager;
                var actions = inputManager.userData.GetActions_Copy();
                var categoryToActions = new Dictionary<InputCategory, List<InputAction>>();
                foreach (var action in actions) {
                    var category = inputManager.userData.GetActionCategoryById(action.categoryId);
                    if (!categoryToActions.ContainsKey(category)) {
                        categoryToActions[category] = new List<InputAction>();
                    }
                    categoryToActions[category].Add(action);
                }
                return categoryToActions;
            }
        }

        private static Dictionary<int, string> PlayerIdsToNames {
            get {
                var inputManager = InputManager;
                int[] playerIds = inputManager.userData.GetPlayerIds();
                var idsToNames = new Dictionary<int, string>();
                int localPlayerId = 0;
                const int systemId = 9999999;
                foreach (var rewiredId in playerIds) {
                    string name = inputManager.userData.GetPlayerNameById(rewiredId);
                    idsToNames.Add(rewiredId == systemId? systemId : localPlayerId++, name);
                }
                return idsToNames;
            }
        }

        const string _name = "RewiredConstants";
        [MenuItem("Edit/Generate " + _name + ".cs")]
        public static void Generate () {
            using (var writer = new ConstantsWriter(_name)) {
                writer.WriteLine("namespace " + _name + " {"); // open namespace
                writer.Indent();

                writer.WriteLine("public static class Players {"); // open players
                writer.Indent();
                foreach (var playerIdToName in PlayerIdsToNames) {
                    int playerId = playerIdToName.Key;
                    string playerName = playerIdToName.Value;
                    writer.WriteLine("public const int {0} = {1};",
                                     ConstantsWriter.MakeSafeForCode(playerName),
                                     playerId);
                }
                writer.UnIndent();
                writer.WriteLine("}"); // close players

                writer.WriteLine();

                writer.WriteLine("public static class Actions {"); // open actions
                writer.Indent();
                foreach (var categoryToActions in CategoriesToActions) {
                    var category = categoryToActions.Key;
                    var actions = categoryToActions.Value;
                    writer.WriteLine();
                    WriteCategorySummary(writer, category);
                    writer.WriteLine("public static class " + category.name + " {"); // open category
                    writer.Indent();
                    foreach (var action in actions) {
                        writer.WriteLine();
                        WriteActionSummary(writer, action);
                        writer.WriteLine("public const int {0} = {1};",
                                         ConstantsWriter.MakeSafeForCode(action.name),
                                         action.id);
                    }
                    writer.UnIndent();
                    writer.WriteLine("}"); // close category
                    writer.UnIndent();
                    writer.WriteLine("}"); // close actions
                }
                writer.UnIndent();
                writer.Write("}"); // close namespace
            }
        }
    }
}