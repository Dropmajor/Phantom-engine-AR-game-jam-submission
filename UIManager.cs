using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public Button confirmButton;
    public static UIManager instance;
    public bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        confirmButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(clicked)
        StartCoroutine(switchClick());
    }

    IEnumerator switchClick()
    {
        yield return null;
        clicked = false;
    }

    public void confirm(Action<Vector3> callback, Vector3 dest)
    {
        confirmButton.onClick.AddListener(() =>
        {
            callback(dest);
            confirmButton.gameObject.SetActive(false);
            confirmButton.onClick.RemoveAllListeners();
        });
        confirmButton.gameObject.SetActive(true);
    }

}
