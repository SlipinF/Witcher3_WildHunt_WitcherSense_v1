using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QlueLogic : MonoBehaviour
{

    bool isSerching;
   
   float floatToSet;
    float _Distance;

   
   Transform ReferenceToPlayer;


    [SerializeField]
    GameObject SoundsCircle;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", 0);
        FindObjectOfType<CharacterMovement>().OnSensingEvent += DisplayQlue;
        FindObjectOfType<CharacterMovement>().OnSensingFinishedEvent += HideQlue;
        ReferenceToPlayer = FindObjectOfType<CharacterMovement>().transform;
    }


    private void Update()
    {
        if(isSerching)
        {
            if(floatToSet <= 0.03f && CalculateRange() <= FindObjectOfType<CharacterMovement>().range)
            {
                floatToSet += 0.001f;
                gameObject.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", floatToSet);
                if (SoundsCircle != null)
                SoundsCircle.SetActive(true);
            }

            if (_Distance <= 0.3f)
            {
                _Distance += 0.01f;
                gameObject.GetComponent<Renderer>().material.SetFloat("_Distance", _Distance);
                if(SoundsCircle != null)
                SoundsCircle.SetActive(true);
            }

        }



        if (isSerching == false || CalculateRange() > FindObjectOfType<CharacterMovement>().range) 
        {
            if (floatToSet >= 0f)
            {
                floatToSet -= 0.001f;
                gameObject.GetComponent<Renderer>().material.SetFloat("_FirstOutlineWidth", floatToSet);
                if(SoundsCircle!=null)
                SoundsCircle.SetActive(false);
            }
            if(_Distance >= 0)
            {
                _Distance -= 0.01f;
                gameObject.GetComponent<Renderer>().material.SetFloat("_Distance", _Distance);
                if(SoundsCircle != null)
                SoundsCircle.SetActive(false);
            }        
        }

        if(SoundsCircle != null)
        {
          SoundsCircle.transform.LookAt(ReferenceToPlayer);
        }
    }

    void DisplayQlue()
    {
       isSerching = true;
    }
    void HideQlue()
    {
        isSerching = false;
    }

    float CalculateRange()
    {
        float distance = Vector3.Distance(gameObject.transform.position, FindObjectOfType<CharacterMovement>().transform.position);

        return distance;
    }
}
