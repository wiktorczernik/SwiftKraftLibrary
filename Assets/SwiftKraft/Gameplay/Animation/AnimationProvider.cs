using SwiftKraft.Utils;
using UnityEngine;

namespace SwiftKraft.Gameplay.Animation
{
    public class AnimationProvider : MonoBehaviour
    {
        [field: SerializeField]
        public AnimationReceiver Target { get; set; }
        [field: SerializeField]
        public Transform Root { get; private set; }
        [field: SerializeField]
        public int RootIndex { get; set; } = 0;

        [field: SerializeField]
        public bool Active { get; set; } = true;

        HierarchyCopier.Cache cache;

        private void Start()
        {
            if (Target == null)
                Target = GetComponentInParent<AnimationReceiver>();

            if (Target != null)
                Target.BuildCache(ref cache, Root, RootIndex);
        }

        private void LateUpdate()
        {
            if (!Active || Target == null || Root == null)
                return;

            if (cache == null)
                Target.BuildCache(ref cache, Root, RootIndex);
            Target.Copy(cache, RootIndex);
        }
    }
}
