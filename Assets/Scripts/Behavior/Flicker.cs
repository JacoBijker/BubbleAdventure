using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Behavior
{
    class Flicker : ICustomBehaviour
    {
        private SpriteRenderer renderer;
        private bool isFlashing;
        private float flashTicker;
        private float flashInterval = 0.5f;
        private bool Upwards;
        public Flicker(SpriteRenderer spriteRenderer)
        {
            renderer = spriteRenderer;
        }

        public Flicker(GameObject objectToAttachTo)
        {
            renderer = objectToAttachTo.GetComponentsInChildren<SpriteRenderer>()[0] as SpriteRenderer;
        }

        public bool IsActive
        {
            get
            {
                return isFlashing;
            }
        }

        public void Update()
        {
            if (isFlashing)
            {
                flashTicker += Upwards ? Time.deltaTime : -Time.deltaTime;
                float opacity = 1 - (flashTicker / flashInterval);
                renderer.color = new Color(1f, 1f, 1f, opacity);

                if (flashTicker >= flashInterval)
                    Upwards = false;
                if (flashTicker <= 0)
                    Upwards = true;
            }
        }

        public void Start()
        {
            isFlashing = true;
        }

        public void Stop()
        {
            isFlashing = false;
            renderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
