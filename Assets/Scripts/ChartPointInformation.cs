using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartPointInformation : MonoBehaviour
{

    [SerializeField]private GameObject information_tooltip;

    private void OnMouseOver()
    {
        information_tooltip.SetActive(true);
    }

    private void OnMouseExit()
    {
        information_tooltip.SetActive(false);
    }
}
