using System.Collections;
using UnityEngine;

public class ActiveDelay : MonoBehaviour
{
    public GameObject go;
    private void Start()
    {
        StartCoroutine(losedelay());
    }

    IEnumerator losedelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(true);
    }
}