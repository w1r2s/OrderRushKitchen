#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class AlignVisualChildrenToRootGroundTool
{
    [MenuItem("Tools/Kitchen/Align Selected Visual Renderers Bottom To Root Y")]
    private static void AlignSelectedVisualRenderersBottomToRootY()
    {
        Transform root = Selection.activeTransform;

        if (root == null)
        {
            Debug.LogWarning("Select a visual prefab root first.");
            return;
        }

        Renderer[] renderers = root.GetComponentsInChildren<Renderer>(true);

        if (renderers.Length == 0)
        {
            Debug.LogWarning($"No renderers found under '{root.name}'.");
            return;
        }

        Bounds bounds = renderers[0].bounds;

        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        float targetMinY = root.position.y;
        float deltaY = targetMinY - bounds.min.y;

        Transform[] movableRoots = GetTopLevelRendererRoots(root, renderers);

        Undo.RecordObjects(movableRoots, "Align Visual Renderer Children To Root Y");

        foreach (Transform movableRoot in movableRoots)
        {
            movableRoot.position += Vector3.up * deltaY;
        }

        Debug.Log(
            $"Aligned '{root.name}'. " +
            $"Old MinY: {bounds.min.y:F3}, Target MinY: {targetMinY:F3}, DeltaY: {deltaY:F3}, Moved Roots: {movableRoots.Length}"
        );
    }

    private static Transform[] GetTopLevelRendererRoots(Transform root, Renderer[] renderers)
    {
        var result = new List<Transform>();

        foreach (Renderer renderer in renderers)
        {
            Transform candidate = renderer.transform;

            while (candidate.parent != null && candidate.parent != root)
            {
                candidate = candidate.parent;
            }

            if (candidate == root)
                continue;

            if (!result.Contains(candidate))
            {
                result.Add(candidate);
            }
        }

        return result.ToArray();
    }

    [MenuItem("Tools/Kitchen/Align Selected Visual Renderers Bottom To Root Y", true)]
    private static bool ValidateAlignSelectedVisualRenderersBottomToRootY()
    {
        return Selection.activeTransform != null;
    }
}
#endif
