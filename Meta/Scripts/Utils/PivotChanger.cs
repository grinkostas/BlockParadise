using UnityEngine;

namespace GameCore.Meta.Scripts.Utils
{
    public static class PivotChanger
    {
        private static readonly string pivotObjectName = "_TempPivot";

        public static Transform GetMeshCenterPivot(this Transform target) => target.GetMeshCenterPivot(Vector3.zero);
        public static Transform GetMeshCenterPivot(this Transform target, Vector3 pivotOffset)
        {
            if (TryGetMeshCenter(target.gameObject, out Vector3 center) == false)
                return target;
            if (target.parent.gameObject.name == pivotObjectName)
                return target.parent;
            var gameObject = new GameObject(pivotObjectName);
            var transform = gameObject.transform;
            transform.SetParent(target.parent, false);
            transform.localPosition = center + pivotOffset;
            target.SetParent(transform, true);
            return transform;
        }
        
        public static bool TryGetMeshCenter(GameObject meshObject, out Vector3 center)
        {
            center = Vector3.zero;
            if (meshObject.TryGetComponent(out MeshFilter meshFilter) == false)
                return false;
            
            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            Vector3 meshCenter = Vector3.zero;

            for (int i = 0; i < vertices.Length; i++)
                meshCenter += vertices[i];

            meshCenter /= vertices.Length;
            center = meshObject.transform.TransformPoint(meshCenter);
            return true;
        }
    }
}