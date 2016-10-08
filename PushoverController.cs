using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushoverController : MonoBehaviour {
    public bool _activated = false;
    public GameObject master;
    public GameObject rotateCenter;
    public Transform previousParent;
    public float rotationTime = 0.1f;
    public float rayLength = 1f;
    public Color defaultColor;

    private bool moving = false;
    private Transform factoryTransform;

    private bool canMoveForward = true;
    private bool canMoveBackward = true;
    private bool canMoveLeft = true;
    private bool canMoveRight = true;

    private Animator anim;

    private Vector3 currentPos;
    private Vector3 lastPos;

    public List<GameObject> slaves;

    public bool activated
    {
        get
        {
            return _activated;
        }
        set
        {
            _activated = value;
            if (!anim)
            {
                return;
            }
            anim.SetBool("activated", _activated);
        }
    }

    // Use this for initialization
    void Start () {
        previousParent = transform.parent;
        anim = GetComponent<Animator>();
        slaves = new List<GameObject>();
        factoryTransform = transform;
    }

    public void ResetTransform()
    {
        //unParent();
        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
    }

    public void startLevel(Vector3 startPos)
    {
        // Move the player to the correct startPos
        slaves = new List<GameObject>();
        this.transform.parent.transform.position = startPos;
        ResetTransform();
        activated = true;
    }

    void Update ()
    {
        if (!activated)
        {
            return;
        }
        if (master)
        {
            return;
        }
        if (moving)
        {
            return;
        }
        else
        {
            transform.rotation = Quaternion.identity;
            rotateCenter.transform.position = roundUpVector(rotateCenter.transform.position);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 directionVector = new Vector3(0, -0.5f, 0.5f);
            string rotateAxis = "x";
            float rotateAngle = 90f;
            if (canMoveForward)
            {
                rollCharacter(rotateAxis, rotateAngle, directionVector);
                foreach (GameObject slaveObject in slaves)
                {
                    PushoverController controller = slaveObject.GetComponent<PushoverController>();
                    if (controller.canMoveForward)
                    {
                        controller.rollCharacter(rotateAxis, rotateAngle, directionVector);
                    }
                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 directionVector = new Vector3(0, -0.5f, -0.5f);
            string rotateAxis = "x";
            float rotateAngle = -90f;
            if (canMoveBackward)
            {
                rollCharacter(rotateAxis, rotateAngle, directionVector);
                foreach (GameObject slaveObject in slaves)
                {
                    PushoverController controller = slaveObject.GetComponent<PushoverController>();
                    if (controller.canMoveBackward)
                    {
                        controller.rollCharacter(rotateAxis, rotateAngle, directionVector);
                    }
                }
            }

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 directionVector = new Vector3(-0.5f, -0.5f, 0f);
            string rotateAxis = "z";
            float rotateAngle = 90f;
            if (canMoveLeft)
            {
                rollCharacter(rotateAxis, rotateAngle, directionVector);
                foreach (GameObject slaveObject in slaves)
                {
                    PushoverController controller = slaveObject.GetComponent<PushoverController>();
                    if (controller.canMoveLeft)
                    {
                        controller.rollCharacter(rotateAxis, rotateAngle, directionVector);
                    }
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 directionVector = new Vector3(0.5f, -0.5f, 0);
            string rotateAxis = "z";
            float rotateAngle = -90f;
            if (canMoveRight)
            {
                rollCharacter(rotateAxis, rotateAngle, directionVector);
                foreach (GameObject slaveObject in slaves)
                {
                    PushoverController controller = slaveObject.GetComponent<PushoverController>();
                    if (controller.canMoveRight)
                    {
                        controller.rollCharacter(rotateAxis, rotateAngle, directionVector);
                    }
                }
            }
        }
    }

    void rollCharacter (string rotateAxis, float rotateAngle, Vector3 directionVector){
        rotateCenter.transform.position = directionVector + transform.position;
        transform.parent = rotateCenter.transform;
        iTween.RotateTo(rotateCenter, iTween.Hash(rotateAxis, rotateAngle,
                                                  "time", rotationTime,
                                                  "onupdate", "checkPosition",
                                                  "onupdatetarget", gameObject,
                                                  "oncomplete", "unParent",
                                                  "oncompletetarget", gameObject));
        moving = true;
    }

    static Vector3 roundUpVector(Vector3 vector)
    {
        vector *= 2.0f;
        vector = new Vector3(Mathf.Round(vector.x),
                           Mathf.Round(vector.y),
                           Mathf.Round(vector.z));
        vector /= 2.0f;
        return vector;
    }

    void checkPosition()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!canMoveForward)
            {
                unParent();
            }
            if (master)
            {
                PushoverController controller = master.GetComponent<PushoverController>();
                if (!controller.canMoveForward)
                {
                    unParent();
                }
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (!canMoveBackward)
            {
                unParent();
            }
            if (master)
            {
                PushoverController controller = master.GetComponent<PushoverController>();
                if (!controller.canMoveBackward)
                {
                    unParent();
                }
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!canMoveLeft)
            {
                unParent();
            }
            if (master)
            {
                PushoverController controller = master.GetComponent<PushoverController>();
                if (!controller.canMoveLeft)
                {
                    unParent();
                }
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (!canMoveRight)
            {
                unParent();
            }
            if (master)
            {
                PushoverController controller = master.GetComponent<PushoverController>();
                if (!controller.canMoveRight)
                {
                    unParent();
                }
            }
        }
    }

    void unParent ()
    {
        transform.parent = previousParent;
        rotateCenter.transform.position = roundUpVector(rotateCenter.transform.position);
        rotateCenter.transform.rotation = Quaternion.identity;
        transform.rotation = Quaternion.identity;
        transform.position = roundUpVector(transform.position);
        iTween.Stop(rotateCenter);
        moving = false;
    }

    bool canMove(RaycastHit raycastHit, Vector3 direction)
    {
        RaycastHit colliderRaycastHit;
        string wallTag = "wall";
        if (raycastHit.collider.gameObject.tag == wallTag)
        {
            return false;
        }
        if (raycastHit.collider.gameObject.tag == "Player")
        {
            if (master)
            {
                return false;
            }
            PushoverController controller = raycastHit.collider.gameObject.GetComponent<PushoverController>();
            if(Physics.Raycast(raycastHit.collider.transform.position, direction, out colliderRaycastHit, rayLength))
            {
                if (controller.activated && !controller.canMove(colliderRaycastHit, direction))
                {
                    return false;
                }
            }
            else if (!controller.activated)
            {
                return false;
            }
        }
        return true;
    }

	void FixedUpdate () {
        List<RaycastHit> raycastHits = new List<RaycastHit>();
        RaycastHit forwardHit;
        RaycastHit backwardHit;
        RaycastHit leftHit;
        RaycastHit rightHit;
        canMoveForward = true;
        canMoveBackward = true;
        canMoveLeft = true;
        canMoveRight = true;
        if (Physics.Raycast(transform.position, rotateCenter.transform.forward, out forwardHit, rayLength))
        {
            canMoveForward = canMove(forwardHit, rotateCenter.transform.forward);
            raycastHits.Add(forwardHit);
        }
        if (Physics.Raycast(transform.position, -rotateCenter.transform.forward, out backwardHit, rayLength))
        {
            canMoveBackward = canMove(backwardHit, -rotateCenter.transform.forward);
            raycastHits.Add(backwardHit);
        }
        if (Physics.Raycast(transform.position, rotateCenter.transform.right, out rightHit, rayLength))
        {
            canMoveRight = canMove(rightHit, rotateCenter.transform.right);
            raycastHits.Add(rightHit);
        }
        if (Physics.Raycast(transform.position, -rotateCenter.transform.right, out leftHit, rayLength))
        {
            canMoveLeft = canMove(leftHit, -rotateCenter.transform.right);
            raycastHits.Add(leftHit);
        }
        if (master)
        {
            return;
        }
        if (!activated)
        {
            return;
        }
        foreach (RaycastHit hitInfo in raycastHits)
        {
            if (hitInfo.collider.gameObject.tag == "Player")
            {
                print(hitInfo.collider.gameObject);
                if (!slaves.Contains(hitInfo.collider.gameObject))
                {
                    PushoverController controller = hitInfo.collider.gameObject.GetComponent<PushoverController>();
                    if (!controller.activated)
                    {
                        slaves.Add(hitInfo.collider.gameObject);
                        controller.master = gameObject;
                        controller.activated = true;
                    }
                }
            }
        }
        List<GameObject> slavesToRemove = new List<GameObject>();
        foreach (GameObject slaveObject in slaves)
        {
            float dist = Vector3.Distance(slaveObject.transform.position, transform.position);
            if (dist > rayLength + 0.01)
            {
                PushoverController controller = slaveObject.GetComponent<PushoverController>();
                controller.activated = false;
                controller.master = null;
                slavesToRemove.Add(slaveObject);
            }
        }
        foreach (GameObject slaveToRemove in slavesToRemove)
        {
            slaves.Remove(slaveToRemove);
        }
    }
}
