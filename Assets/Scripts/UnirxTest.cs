using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
//using System;
//using System.Reactive.Linq;


public class UnirxTest : MonoBehaviour
{
    ReactiveCollection<float> colection = new ReactiveCollection<float>();
    int cont = 0;

    // Start is called before the first frame update
    void Start()
    {
        colection.ObserveAdd().Subscribe(
            x =>{
                print("entrou um novo! x:" + x);
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (cont % 2 == 0)
        {
            colection.Add(cont);
        }
        cont++;
    }
}
