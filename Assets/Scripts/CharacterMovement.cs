using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PostProcessing;

  public class CharacterMovement : MonoBehaviour
  {         
    public event Action OnSensingEvent;
    
        public event Action OnSensingFinishedEvent;
        public float range = 100f;    
        [SerializeField]
        Animator animator;

        [SerializeField]
        vThirdPersonCamera camera;

        [SerializeField]
        PostProcessingProfile ppProfile;
        bool SetZoom;
        public float changeValue;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                animator.SetBool("Walking", true);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                animator.SetBool("Walking", false);
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                OnSensingEvent?.Invoke();
                SetZoom = true;
                ppProfile.motionBlur.enabled = true;
                ppProfile.chromaticAberration.enabled = true;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                OnSensingFinishedEvent?.Invoke();

                SetZoom = false;
                ppProfile.motionBlur.enabled = false;
                ppProfile.chromaticAberration.enabled = false;
            }

            if(SetZoom)
            {
            
                if (camera.rightOffset <= 0.18f)
                camera.rightOffset += 0.012f;
                if (camera.defaultDistance >= 1f)
                    camera.defaultDistance -= 0.1f;
                if (camera.height <= 1.6f)
                    camera.height += 0.03f;;
                if (ppProfile.vignette.settings.intensity <= 0.2f)
                     ChangeVignetteAtRuntime(0.2f);
            }
            else
{
               
               if (camera.rightOffset >= 0f)
                    camera.rightOffset -= 0.01f;
                if (camera.defaultDistance <= 2.5f)
                    camera.defaultDistance += 0.1f;
                if (camera.height >= 1.4f)
                    camera.height -= 0.03f;

                if (ppProfile.vignette.settings.intensity > 0.1f)
                            ChangeVignetteAtRuntime(-0.1f);            
            }
            void ChangeVignetteAtRuntime(float val)
            {
                VignetteModel.Settings vignetteSettings = ppProfile.vignette.settings;
                vignetteSettings.intensity += Time.deltaTime * val;
                ppProfile.vignette.settings = vignetteSettings;
            }
        }
    }


