using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Standalone
{
    public class GUIScaler
    {
        private float originalWidth = 1024f;
        private float originalHeight = 600f;
        private Vector3 scale;
        private Matrix4x4 original;

        public GUIScaler()
        {
            scale = new Vector3(Screen.width / originalWidth, Screen.height / originalHeight, 1);
        }

        internal void Set(Matrix4x4 matrix4x4)
        {
            original = matrix4x4;
        }

        internal Matrix4x4 GetScaled()
        {
            return Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
        }

        internal Matrix4x4 GetOriginal()
        {
            return original;
        }
    }
}
