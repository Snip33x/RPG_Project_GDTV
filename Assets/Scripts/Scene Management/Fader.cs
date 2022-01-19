using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade =null;

        private void Awake()  //we should set refrences in awake, and use them in start, so we won't get null refrences
        {
            canvasGroup = GetComponent<CanvasGroup>();

        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }
        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }
       
        public Coroutine Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }


        private IEnumerator FadeRoutine(float target, float time) //removing bug, causing fader to never fadeout if we changed scene fast
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) // alpha is not 1
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
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

