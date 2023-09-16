using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Behavior
{
    class Scaling : ICustomBehaviour
    {
        private Transform transform;

        private bool busyScaling = false;
        private bool scalingUp = false;
        private float scaleSize;
        private float scaleTime;
        private float currentScale;
        private Vector3 originalScale;

        public bool IsActive
        {
            get
            {
                return busyScaling;
            }
        }

        public Scaling(Transform transform, float scaleSize = 0.2f, float scaleTime = 0.2f)
        {
            this.transform = transform;
            this.scaleSize = scaleSize;
            this.scaleTime = scaleTime;

            originalScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
        }

        public void SetOriginalScale(Vector3 originalScale)
        {
            this.originalScale = originalScale;
        }

        public void Update()
        {
            if (busyScaling)
            {
                currentScale += scalingUp ? Time.deltaTime : -Time.deltaTime;
                if (scalingUp && currentScale > scaleTime)
                    scalingUp = false;

                if (!scalingUp && currentScale <= 0)
                    Stop();

                this.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z) * (1 + (scaleSize * currentScale));
            }
        }

        public void Start()
        {
            busyScaling = true;
            scalingUp = true;
            currentScale = 0;
        }

        public void Stop()
        {
            busyScaling = false;
            this.transform.localScale = originalScale;
        }
    }
}
