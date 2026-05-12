#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class HoldPointPlacementTool
{
    private const string HoldPointName = "HoldPoint";
    private const float VerticalOffset = 0.05f;

    [MenuItem("Tools/Kitchen/Create Or Move HoldPoint Above Selected Visual")]
    private static void CreateOrMoveHoldPointAboveSelectedVisual()
    {
        Transform selected = Selection.activeTransform;

        if (selected == null)
        {
            Debug.LogWarning("Select a counter root or visual root first.");
            return;
        }

        Renderer[] renderers = selected.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogWarning($"No renderers found under '{selected.name}'.");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Transform holdPoint = selected.Find(HoldPointName);

        if (holdPoint == null)
        {
            GameObject holdPointObject = new GameObject(HoldPointName);
            Undo.RegisterCreatedObjectUndo(holdPointObject, "Create HoldPoint");

            holdPoint = holdPointObject.transform;
            Undo.SetTransformParent(holdPoint, selected, "Parent HoldPoint");
        }
        else
        {
            Undo.RecordObject(holdPoint, "Move HoldPoint");
        }

        Vector3 targetWorldPosition = new Vector3(
            bounds.center.x,
            bounds.max.y + VerticalOffset,
            bounds.center.z
        );

        holdPoint.position = targetWorldPosition;

        Debug.Log(
            $"Placed HoldPoint for '{selected.name}' at world {targetWorldPosition}. " +
            $"Bounds MaxY: {bounds.max.y:F3}, Offset: {VerticalOffset:F3}"
        );

        Selection.activeTransform = holdPoint;
    }

    [MenuItem("Tools/Kitchen/Create Or Move HoldPoint Above Selected Visual", true)]
    private static bool ValidateCreateOrMoveHoldPointAboveSelectedVisual()
    {
        return Selection.activeTransform != null;
    }
}
#endif
