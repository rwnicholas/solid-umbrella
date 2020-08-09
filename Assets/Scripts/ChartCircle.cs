using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartCircle : MonoBehaviour
{
    [SerializeField]private float cwnd;
    [SerializeField]private string eventMode;

    public void AddAtributesValues(float cwndValue, string eventMode)
    {
        this.cwnd = cwndValue;
        this.eventMode = eventMode;
    }

    private void OnMouseDown()
    {
        GameObject.Find("Manager").GetComponent<Main>().circleInformationPrefab
            .GetComponentsInChildren<TMPro.TextMeshProUGUI>()[1].text = this.cwnd.ToString();
        GameObject.Find("Manager").GetComponent<Main>().circleInformationPrefab
            .GetComponentsInChildren<TMPro.TextMeshProUGUI>()[3].text = this.eventMode;
    }

    private void OnMouseEnter()
    {
        gameObject.transform.localScale = new Vector3(2, 2, 0);
    }
    

    private void OnMouseExit()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 0);
    }
}
