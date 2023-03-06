using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorManager : MonoBehaviour
{
    private Ray ra;
    private RaycastHit hit;
    public GameObject floorBlock; // the floor block hit by ray

    public int layerMask;

    public List<GameObject> deployed_operator = new List<GameObject>();
    public bool exist_selected;
    public GameObject selected_operator;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << 10;
        exist_selected = false;
        selected_operator = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SelectOperator();
    }

    void SelectOperator()
    {
        string operatorName = null;
        if (Input.GetMouseButton(1))
        {
            ra = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ra, out hit, 10000, layerMask))
            {
                if (hit.transform.gameObject.GetComponent<FloorBlockInfo>().character != null)
                {
                    operatorName = hit.transform.gameObject.GetComponent<FloorBlockInfo>().character.GetComponent<Character>().characterName;
                }
                else
                {
                    operatorName = null;
                }
            }

            if (exist_selected)
            {
                if (operatorName == selected_operator.gameObject.GetComponent<Character>().characterName)
                {
                    Debug.Log("ready to retreat:" + operatorName);
                    foreach (GameObject retreatOp in deployed_operator)
                    {
                        if (retreatOp.GetComponent<Character>().characterName == operatorName)
                        {
                            RetreatOperator(retreatOp, hit.transform.gameObject);
                            exist_selected = false;
                            selected_operator = null;
                            break;
                        }
                    }
                }
            }

            foreach (GameObject op in deployed_operator)
            {
                if (op.GetComponent<Character>().characterName == operatorName)
                {
                    op.GetComponent<Character>().is_selected = true;
                    exist_selected = true;
                    selected_operator = op;
                    break;
                }
                else
                {
                    op.GetComponent<Character>().is_selected = false;
                    exist_selected = false;
                    selected_operator = null;
                }
            }
        }
    }

    void RetreatOperator(GameObject op, GameObject floorBlock)
    {
        deployed_operator.Remove(op);
        op.GetComponent<Character>().ResetToButton();
        floorBlock.GetComponent<FloorBlockInfo>().character = null;
    }
}
