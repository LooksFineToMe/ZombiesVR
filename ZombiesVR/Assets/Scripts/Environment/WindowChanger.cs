using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowChanger : MonoBehaviour
{
    public Material[] materials;
    public Renderer material;
    public float timer;
    public int ChangeWindowTime;
    private void Start()
    {
        material = GetComponent<Renderer>();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= ChangeWindowTime)
        {
            ChangeMaterial();
        }
    }
    public void ChangeMaterial()    
    {
        int randomNumber = Random.Range(0, materials.Length);
        material.material = materials[randomNumber];
        ChangeWindowTime = Random.Range(30, 120);
        timer = 0;
    }

}
