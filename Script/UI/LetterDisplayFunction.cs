using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDisplayFunction : MonoBehaviour
{
    public void Back()
    {
        this.transform.parent.Find("LetterInterface").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
