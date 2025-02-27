using UnityEngine;
using System.Collections;

public class CustomBubbles : MonoBehaviour
{   
    // [Tooltip("The object to be spawned")]
    public GameObject prefab = null;
    // [Tooltip("The transform where the object is spawned")]
    public Transform spawnTransform = null;
    // [Header("jiggle settings")]
    // [Tooltip("Time for bubbles to scale in and settle")]
    public float jiggleDuration = 0.3f;
    // [Tooltip("How big is the bulle on its peak")]
    public float overShootScale = 1.2f;
   // [Toolpit(Curve controlling the scaling motion from 0 to overshoot)]
    public AnimationCurve jiggleCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    Vector3 finalScale = new Vector3(10f, 10f, 10f);

    public void Spawn(){
        //create bubble
        GameObject newObject = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);
        //set random rotation
        newObject.transform.rotation = Random.rotation;
        //set randomised laund speed
        float launchSpeed = Random.Range(0.5f, 1.5f);
        //creates Vector3 to represent foward movement
        Vector3 force =spawnTransform.forward * launchSpeed;
        newObject.GetComponent<Rigidbody>().AddForce(force);
        newObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)));

        //reset scale to 0 so it can pop in
        newObject.transform.localScale = Vector3.zero;
        //start the popup coroutine
        StartCoroutine(JiggleEffect(newObject.transform));
    }
    private IEnumerator JiggleEffect(Transform bubbleTransform) {
        Vector3 startScale = Vector3.one;
        float elapsedTime = 0f;

        while (elapsedTime < jiggleDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / jiggleDuration;
            
            // Evaluate your animation curve (0..1) -> typically goes from 0 to 1
            float curveValue = jiggleCurve.Evaluate(t);
            
            // Lerp from 0 scale to overshootScale
            float scaleFactor = Mathf.Lerp(0f, overShootScale, curveValue);
            
            bubbleTransform.localScale = finalScale * scaleFactor;
            yield return null;
        }
        // Ensure the final scale is set
        bubbleTransform.localScale = finalScale;
    }
}
