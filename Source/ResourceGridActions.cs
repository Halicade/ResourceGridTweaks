using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using Verse;

namespace ResourceGridTweaks;

public class ResourceGridActions
{
    [DebugAction(category: "Map", name: "Clear resource grid (rect)", allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
    private static void ClearResourceGrid() {

        DebugToolsGeneral.GenericRectTool("clear", rect => {
            foreach (IntVec3 cellLoc in rect) {
                Find.CurrentMap.deepResourceGrid.SetAt(cellLoc, null, 0);
            }
        });
    }

    [DebugAction(category: "Map", name: "Refresh deep resource (rect)", allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
    private static void RefreshResourceGrid() {

        DebugToolsGeneral.GenericRectTool("refresh", rect => {
            foreach (IntVec3 cellLoc in rect) {

                ThingDef itemToRefresh = Find.CurrentMap.deepResourceGrid.ThingDefAt(cellLoc);
                if (itemToRefresh == null) {
                    continue;
                }
                if (itemToRefresh.deepCommonality == 0) {
                    Find.CurrentMap.deepResourceGrid.SetAt(cellLoc, null, 0);
                    continue;
                }

                Find.CurrentMap.deepResourceGrid.SetAt(cellLoc, itemToRefresh, itemToRefresh.deepCountPerCell);
            }
        });

    }

    [DebugAction("Map", "Replace deep resource (rect)", allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
    private static List<DebugActionNode> ReplaceDeepResource() {
        List<DebugActionNode> list = [];
        foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where(resource => resource.deepCommonality > 0)) {
            ThingDef itemReplacing = item;
            list.Add(new DebugActionNode(itemReplacing.defName)
            {
                action = () => {
                    DebugToolsGeneral.GenericRectTool(itemReplacing.defName, rect => {
                        foreach (IntVec3 cellLoc in rect) {

                            if (Find.CurrentMap.deepResourceGrid.ThingDefAt(cellLoc) == null) {
                                continue;
                            }
                            Find.CurrentMap.deepResourceGrid.SetAt(cellLoc, itemReplacing, Find.CurrentMap.deepResourceGrid.CountAt(cellLoc));

                        }
                    });
                }
            });
        }
        return list;
    }

    [DebugAction("Map", "Generate deep resource (rect)", allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 100)]
    private static List<DebugActionNode> GenerateDeepResource() {
        List<DebugActionNode> list = [];
        foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs.Where(resource => resource.deepCommonality > 0)) {
            ThingDef localDef = item;
            list.Add(new DebugActionNode(localDef.defName)
            {
                action = () => {
                    DebugToolsGeneral.GenericRectTool(localDef.defName, rect => {
                        foreach (IntVec3 cellLoc in rect) {
                            Find.CurrentMap.deepResourceGrid.SetAt(cellLoc, localDef, localDef.deepCountPerCell);
                        }
                    });
                }
            });
        }
        return list;
    }



}