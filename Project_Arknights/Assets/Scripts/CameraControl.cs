using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    Quaternion normalRotation;
    Vector3 normalPosition;
    Quaternion selectRotation;
    Vector3 selectPositionOffset;

    public GameObject[] operatorArray;
    // Start is called before the first frame update
    void Start()
    {
        normalRotation = Quaternion.Euler(70f, 180f, 0f);
        normalPosition = new Vector3(-3.5f, 6, 4);
        selectRotation = Quaternion.Euler(80f, 175f, 0f);
        selectPositionOffset = new Vector3(0, 6, 1.5f);
        operatorArray = null;
    }

    // Update is called once per frame
    void Update()
    {
        operatorArray = GameObject.FindGameObjectsWithTag("Operator");
        if (operatorArray.Length == 0)
        {
            transform.position = normalPosition;
            transform.rotation = normalRotation;
            Time.timeScale = 1;
        }
        else
        {
            foreach (GameObject op in GameObject.FindGameObjectsWithTag("Operator"))
            {
                if ((!op.GetComponent<Character>().is_directed || !op.GetComponent<Character>().is_deployed) && !op.GetComponent<Character>().is_selected)
                {
                    transform.rotation = selectRotation;
                    Time.timeScale = 0.25f;
                }
                else if (op.GetComponent<Character>().is_selected)
                {
                    transform.rotation = selectRotation;
                    transform.position = op.transform.position + selectPositionOffset;
                    Time.timeScale = 0.25f;
                }
                else
                {
                    transform.position = normalPosition;
                    transform.rotation = normalRotation;
                    Time.timeScale = 1;
                }
            }
        }
    }
}
