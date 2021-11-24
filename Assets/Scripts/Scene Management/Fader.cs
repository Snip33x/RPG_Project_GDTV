using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1) // alpha is not 1
            {
                canvasGroup.alpha += Time.deltaTime / time;
                // moving alpha toward 1
                yield return null; //run on the next frame
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0) // alpha is not 1
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                // moving alpha toward 1
                yield return null; //run on the next frame
            }
        }

        IEnumerator FadeOutIn() //nested Coroutine
        {
            yield return FadeOut(3f);  //every statement is executed after the previous has ended
            print("Faded out");
            yield return FadeIn(2f);
            print("Faded in");
        }
    }

}

