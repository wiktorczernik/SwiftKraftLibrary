using SwiftKraft.Utils;
using UnityEngine;

namespace SwiftKraft.Gameplay.Animation
{
    public class AnimationReceiver : MonoBehaviour
    {
        [field: SerializeField]
        public Transform[] Roots { get; private set; } = new Transform[1];

        public void Copy(HierarchyCopier.Cache cache, int index)
        {
            if (cache == null || !Roots.InRange(index) || Roots[index] == null)
                return;

            HierarchyCopier.Copy(cache, Roots[index]);
        }

        public void BuildCache(ref HierarchyCopier.Cache cache, Transform source, int index)
        {
            if (!Roots.InRange(index) || Roots[index] == null)
                return;

            HierarchyCopier.BuildCache(ref cache, source, Roots[index]);
        }
    }
}
