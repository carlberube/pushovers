using UnityEngine;
using System.Collections;

public class BackgroundCube : MonoBehaviour {
    private float originalHeight;

	// Use this for initialization
	void Start () {
        originalHeight = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector3 newYpos = new Vector3(0, Random.Range(-1, 1), 0);
        transform.Translate(newYpos * Time.deltaTime);
    }
}
