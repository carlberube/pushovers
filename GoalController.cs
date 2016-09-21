using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {
    private Vector3 originalPos;
    public bool activated = false;

    // Use this for initialization
    void Start () {
        originalPos = transform.position;
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider other) {
        activated = true;
        if (transform.position.y != 0)
        {
            iTween.MoveTo(gameObject, originalPos+new Vector3(0, -0.1f, 0), 0.2f);
        }
	}

    void OnTriggerExit (Collider other)
    {
        activated = false;
        if (transform.position.y == 0)
        {
            iTween.MoveTo(gameObject, originalPos, 0.2f);
        }

    }

}
