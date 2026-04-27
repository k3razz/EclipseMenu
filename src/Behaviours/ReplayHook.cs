using UnityEngine;
using EclipseMenu;

namespace EclipseMenu
{
    public class ReplayHook : MonoBehaviour
    {
        void Update()
        {
            if (State.ShowReplay)
            {
                ReplayDraw.DrawWalkPaths();
            }
        }
    }
}