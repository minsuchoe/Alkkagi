﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    public GameObject arrow;

    GameObject arrowPrefab;
    Vector3 first, last;
    RaycastHit hit;         //raycast로부터 정보를 얻기위해 사용되는 구조체

    void OnMouseDown()      //2차원 벡터
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))      //Mathf.Infinity : 양의 무한대
        {
            if (true)
            {
                first = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
                StartCoroutine(Fade());
                
                arrowPrefab = Instantiate(arrow, transform.position, Quaternion.identity);
            }
        }
    }
    public float alphaMin = 0.5f;
    IEnumerator Fade()
    {
        while (true)
        {
            for (float f = 1f; f >= alphaMin; f -= 0.01f)
            {
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                Color c = renderer.material.color;
                c.a = f;    //color값은 r,g,b,a로 구성되어 있음. a는 알파(alpha)값임.
                renderer.material.color = c;
                yield return null;
            }
            for (float f = alphaMin; f <= 1; f += 0.01f)
            {
                MeshRenderer renderer = GetComponent<MeshRenderer>();
                Color c = renderer.material.color;
                c.a = f;
                renderer.material.color = c;
                yield return null;
            }
        }
    }

    private void OnMouseDrag()
    {
        last = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        arrowPrefab.transform.LookAt(first - last);

        Vector3 tempVec = first - last;
        int count = (int) (Mathf.Abs(tempVec.magnitude) / 20);
       
        if (count > 5)
            count = 5;

        string s = string.Empty;

        for (int i = 0; i < count; ++i)
            s += ">";

        arrowPrefab.GetComponentInChildren<TextMesh>().text = s;


    }

    private void OnMouseUp()        //2차원 벡터
    {
        StopAllCoroutines();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Color c = renderer.material.color;
        c.a = 1.0f;
        renderer.material.color = c;

        last = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y);
        Vector3 tempVec = first - last;
        if (Mathf.Abs(tempVec.magnitude) < 100 )
            GetComponent<Rigidbody>().AddForce((first - last) * 100.0f);
        else
        {
            GetComponent<Rigidbody>().AddForce(tempVec.normalized * 10000f);
        }
        Destroy(arrowPrefab, 0.0f);
    }

    private void Update()
    {
        if (transform.position.y <= -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
