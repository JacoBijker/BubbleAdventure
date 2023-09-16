using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Behavior
{
    class SlideOut : ICustomBehaviour
    {
        private GameObject attachedObject;
        private Vector3 originalPosition;
        private Vector3 switchPosition;
        private float yOffset;

        private float perfectDuration;
        private float duration;
        private float extent;
        private float startDelay;
        private float currentTime;

        private bool isActive = true;

        private SlideInPhases currentPhase = SlideInPhases.StartWait;

        public SlideOut(GameObject attachedObject, float yOffset)
        {
            this.attachedObject = attachedObject;
            this.yOffset = yOffset;

            perfectDuration = UnityEngine.Random.Range(0.1f, 0.3f);
            duration = UnityEngine.Random.Range(0.5f, 1.25f);
            startDelay = UnityEngine.Random.Range(0, 0.5f);
            extent = UnityEngine.Random.Range(1, 1.15f);

            originalPosition = this.attachedObject.transform.position;
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        public void Update()
        {
            if (!isActive)
                return;

            currentTime += Time.deltaTime;
            switch (currentPhase)
            {
                case SlideInPhases.StartWait:
                    if (currentTime > startDelay)
                    {                      
                        if (attachedObject == null)
                            Stop();
                        else
                        {
                            currentPhase = SlideInPhases.SlidePast;
                            currentTime = 0;
                        }
                    }
                    break;
                case SlideInPhases.SlidePast:
                    var percentage = currentTime / duration;
                    var yComponent = yOffset * percentage * extent;

                    attachedObject.transform.position = new Vector3(originalPosition.x, originalPosition.y - yOffset - yComponent, originalPosition.z);
                    if (currentTime > duration)
                    {
                        currentTime = 0;
                        currentPhase = SlideInPhases.SlidePerfect;
                        switchPosition = attachedObject.transform.position;
                    }
                    break;
                case SlideInPhases.SlidePerfect:                    
                    Stop();
                    break;
            }
        }

        public void Start()
        {
            isActive = true;
        }

        public void Stop()
        {
            currentPhase = SlideInPhases.SlideDone;
            isActive = false;
        }

    }
}
