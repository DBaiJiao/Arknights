using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonButton : MonoBehaviour
{
    public GameObject character;
    public void OnButtonDown()
    {
        character.SetActive(true);
        character.gameObject.GetComponent<Character>().is_dragging = true;
        character.transform.GetChild(1).gameObject.SetActive(false);
        character.transform.GetChild(2).gameObject.SetActive(false);
        character.transform.GetChild(4).gameObject.SetActive(false);
    }
    public void OnButtonUp()
    {
        character.GetComponent<Character>().is_dragging = false;
        character.GetComponent<Character>().is_deployed = true;
        character.transform.GetChild(4).gameObject.SetActive(true);
    }
}
