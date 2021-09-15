using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickPositionManager_01ScreenToWorldPoint : MonoBehaviour
{
    
    
    public GameObject fancy;
    public float distance = 15f;
    public float distanceChange = 1f;
    private int shape = 0;
    private GameObject primitive;
    private float red = 1f, green = 1f, blue = 1f, timeToDestroy = 3f;
    private bool timeDestroy = true;
    private bool isAnimTypeRandom, isAnimSpeedRandom;
    public Text mousePosition;
    private Vector3 lastClickPosition = Vector3.zero;
    public Text blueAmount, greenAmount, redAmount, timerAmount, animAmount;
    private float size = 1;

    public GameObject paintedObject00, paintedObject01, paintedObject02;
    private Color paintedObjectColor, paintedObjectEmission;

    private float emissionStrength = 0.5f;
    private float opacityStrength = 0.5f;
    private int animationState = 0;
    private float animationSpeed = 1f;

    public Dropdown animDropDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    //0 = left, 1 = right
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeAnimationState(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeAnimationState(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeAnimationState(2);


        if (Input.GetMouseButtonDown(0)) // left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.layer == 11) //dat clockface
                {
                    
                    hit.transform.parent.GetComponent<Clock>().UpdateTime(hit.transform.GetChild(0).localEulerAngles.y);
                    Debug.Log(hit.transform.GetChild(0).localEulerAngles.y);
                   
                }  
            }

        }


        //left click erases/destroys selected primitives
        if (Input.GetMouseButton(1) && (EventSystem.current.currentSelectedGameObject == null)) // left hold
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject.layer == 12) //painted object
                {

                    Destroy(hit.transform.gameObject);
                }
                
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastClickPosition = Vector3.zero;
        }


        //right click or hold, creates primitives
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            distance += distanceChange;

            
            //create vector to store mouse position, set to default non real value
            float posX = -1f;
            float posY = -1f;
            float posZ = -1f;
            Vector3 clickPosition = new Vector3(posX, posY, posZ);

            //Vector3 clickPosition = -Vector3.one;


            //method1: x and y from Input.mousePosition with a z determined by distance
            clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0f, 0f, distance));

            switch (shape)
            {
                case 0:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    primitive = Instantiate(paintedObject00, clickPosition, Quaternion.identity);
                    break;

                case 1:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    primitive = Instantiate(paintedObject01, clickPosition, Quaternion.identity);
                    break;

                case 2:
                    //primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    primitive = Instantiate(paintedObject02, clickPosition, Quaternion.identity);
                    break;

            }


            //print pos and spawn sphere
            Debug.Log(clickPosition);

            //spawn default object
           // GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            //primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f), Random.Range(0.1f, 1f), Random.Range(0.1f, 1f));

            if (lastClickPosition == Vector3.zero) primitive.transform.localScale = new Vector3(Random.Range(0.1f, 1f) * size, Random.Range(0.1f, 1f)*size, Random.Range(0.1f, 1f)*size);
            else
            {
                float x = Mathf.Clamp(Random.Range(1f, 2f) * Mathf.Abs(lastClickPosition.x - clickPosition.x), .1f, size*5f);
                float y = Mathf.Clamp(Random.Range(1f, 2f) * Mathf.Abs(lastClickPosition.y - clickPosition.y), .2f, size*5f);
                float z = (x + y) / 2f;
                //float z = Random.Range(0.1f, 0.3f);
                primitive.transform.localScale = new Vector3(x, y, z);
                

            }

            if(primitive.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStrength);
                primitive.GetComponent<Renderer>().material.color = paintedObjectColor;
                paintedObjectEmission = new Color(paintedObjectColor.r * emissionStrength, paintedObjectColor.g * emissionStrength, paintedObjectColor.b * emissionStrength);
                primitive.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
            }

            foreach(Transform child in primitive.transform)
            {
                paintedObjectColor = new Color(Random.Range(0.0f, red), Random.Range(0.0f, green), Random.Range(0.0f, blue), opacityStrength);
                child.gameObject.GetComponent<Renderer>().material.color = paintedObjectColor;
                paintedObjectEmission = new Color(paintedObjectColor.r * emissionStrength, paintedObjectColor.g * emissionStrength, paintedObjectColor.b * emissionStrength);
                child.gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
            }

            if (primitive.GetComponent<Animator>() != null)
            {
                if (isAnimTypeRandom) animationState = (int)Random.Range(0f, 2.99f);
                primitive.GetComponent<Animator>().SetInteger("state", animationState);

                if (isAnimSpeedRandom) animationSpeed = Random.Range(0f, 2f);
                primitive.GetComponent<Animator>().speed = animationSpeed;


            }


            //primitive.transform.position = clickPosition;
            //primitive.GetComponent<Renderer>().material.color = new Vector4(Random.Range(0f, red), Random.Range(0f, green), Random.Range(0f, blue), 0.5f);
            primitive.transform.parent = this.transform;

            //primitive.transform.position = clickPosition;
            //instantiate the gameobject prefab - object, location, rotation
            //GameObject tempGO = Instantiate(fancy, clickPosition, Quaternion.identity);

            if (timeDestroy)
            {
                Destroy(primitive, timeToDestroy);
            }

            lastClickPosition = clickPosition;

        }
        mousePosition.text = "Mouse Position x: " + Input.mousePosition.x.ToString("F0") + ", y: " + Input.mousePosition.y.ToString("F0");
    }

    //accessor methods

    public void ChangeDistance(float change)
    {
        distance = change;

    }

    public void ChangeDistanceDelta(float change)
    {
        distanceChange = change;
    }

    public void ChangeShape(int tempShape)
    {
        shape = tempShape;
    }

    public void ChangeRed(float tempRed)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.r = tempRed;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);

            }
            else
            {

                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.gameObject.GetComponent<Renderer>() != null)
                    {
                        paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                        paintedObjectColor.r = tempRed;
                        //paintedObjectColor = new Color(tempRed, paintedObjectColor.g, paintedObjectColor.b, paintedObjectColor.a);
                        grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                    }

                }
            }
        }

       red = tempRed;
       redAmount.text = (red * 100f).ToString("F0");
    }

    public void ChangeBlue(float tempBlue)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.b = tempBlue;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);

            }
            else
            {

                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.gameObject.GetComponent<Renderer>() != null)
                    {
                        paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                        paintedObjectColor.b = tempBlue ;
                        grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                    }

                }
            }
        }

        blue = tempBlue;
        blueAmount.text = (blue * 100f).ToString("F0");
    }

    public void ChangeGreen(float tempGreen)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.g = tempGreen;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);

            }
            else
            {

                foreach (Transform grandchild in child.transform)
                {
                    if (grandchild.gameObject.GetComponent<Renderer>() != null)
                    {
                        paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                        paintedObjectColor.g = tempGreen;
                        grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                    }

                }
            }
        }

        green = tempGreen;
        greenAmount.text = (green * 100f).ToString("F0");
    }

    public void ToggleTimeDestroy(bool timer)
    {

        timeDestroy = timer;
    }

    public void DestroyObjects()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ChangeTimeDestroy(float temp)
    {
        timeToDestroy = temp;
        timerAmount.text = timeToDestroy.ToString("F0") + " Sec";
    }

    public void ChangeEmission(float tempEmission)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectEmission = new Color(paintedObjectColor.r * tempEmission, paintedObjectColor.g * tempEmission, paintedObjectColor.b * tempEmission);
                child.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);

            }
            else
            {
                foreach(Transform grandchild in child)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectEmission = new Color(paintedObjectColor.r * tempEmission, paintedObjectColor.g * tempEmission, paintedObjectColor.b * tempEmission);
                    grandchild.GetComponent<Renderer>().material.SetColor("_EmissionColor", paintedObjectEmission);
                }
            }

        }

        emissionStrength = tempEmission;
        
    }
    public void ChangeSize(float temp)
    {
        foreach (Transform child in transform)
        {
            child.localScale = child.localScale * temp / size;
        }
        size = temp;
    }

    public void changeOpacity(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Renderer>() != null)
            {
                paintedObjectColor = child.GetComponent<Renderer>().material.GetColor("_Color");
                paintedObjectColor.a = temp;
                child.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);

            }
            else
            {
                foreach(Transform grandchild in child)
                {
                    paintedObjectColor = grandchild.GetComponent<Renderer>().material.GetColor("_Color");
                    paintedObjectColor.a = temp;
                    grandchild.GetComponent<Renderer>().material.SetColor("_Color", paintedObjectColor);
                }
            }

        }

        opacityStrength = temp;
    }

    public void ChangeAnimationState(int temp)
    {
        animationState = temp;
        animDropDown.value = animationState;

        foreach (Transform child in transform)
        {
            if(child.gameObject.GetComponent<Animator>() != null)
            {
                child.gameObject.GetComponent<Animator>().SetInteger("state", animationState);
            }

        }


    }

    public void ChangeAnimationSpeed(float temp)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<Animator>() != null)
            {
                child.gameObject.GetComponent<Animator>().speed = temp;
            }
        }
        animationSpeed = temp;
        animAmount.text = animationSpeed.ToString("F1") + " Sec";
    }

    public void ChangeAnimationTypeRandom(bool temp)
    {
        isAnimTypeRandom = temp;
    }

    public void ChangeAnimationSpeedRandom(bool temp)
    {
        isAnimSpeedRandom = temp;
    }

}
