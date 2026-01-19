using System.Collections.Generic;
using UnityEngine;


namespace SwiftKraft.Utils
{
    public class HierarchyCopier : MonoBehaviour
    {
        [SerializeField] private Transform sourceRoot;
        [SerializeField] private Transform targetRoot;

        private Cache cache;

        [ContextMenu("Copy Local Transforms")]
        public void Copy()
        {
            if (cache == null)
                Build();

            Copy(cache);
        }

        [ContextMenu("Build Cache")]
        public void Build() => BuildCache(ref cache, sourceRoot, targetRoot);

        /// <summary>
        /// Source to target copying, target gets changed.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="useWorldPos"></param>
        public static void Copy(Cache cache, bool useWorldPos = false)
        {
            if (cache == null)
            {
                Debug.LogError("Cache is null.");
                return;
            }

            CopyCached(cache);
        }

        //private static void CopyRecursive(Transform source, Transform target, bool useWorldPos = false)
        //{
        //    if (useWorldPos)
        //        target.SetPositionAndRotation(source.position, source.rotation);
        //    else
        //        target.SetLocalPositionAndRotation(source.localPosition, source.localRotation);
        //    target.localScale = source.localScale;

        //    int count = Mathf.Min(source.childCount, target.childCount);
        //    for (int i = 0; i < count; i++)
        //        CopyRecursive(source.GetChild(i), target.GetChild(i));
        //}

        public class Cache
        {
            public readonly List<(Transform source, Transform target)> Pairs = new();

            public void Build(IList<Transform> source, IList<Transform> target)
            {
                Pairs.Clear();
                int min = Mathf.Min(source.Count, target.Count);
                for (int i = 0; i < min; i++)
                    Pairs.Add((source[i], target[i]));
            }
        }

        private static void CopyCached(Cache cache)
        {
            for (int i = 0; i < cache.Pairs.Count; i++)
            {
                var src = cache.Pairs[i].source;
                var tgt = cache.Pairs[i].target;

                tgt.SetLocalPositionAndRotation(src.localPosition, src.localRotation);
                tgt.localScale = src.localScale;
            }
        }

        public static void BuildCache(ref Cache cache, Transform sourceRoot, Transform targetRoot)
        {
            var srcList = new List<Transform>();
            var tgtList = new List<Transform>();

            CollectActiveTransforms(sourceRoot, srcList);
            CollectActiveTransforms(targetRoot, tgtList);

            cache ??= new Cache();
            cache.Build(srcList, tgtList);
        }

        private static void CollectActiveTransforms(Transform root, List<Transform> list)
        {
            list.Add(root);

            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (!child.gameObject.activeSelf)
                    continue;

                CollectActiveTransforms(child, list);
            }
        }
    }
}
