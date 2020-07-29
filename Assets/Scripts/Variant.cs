using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Variant : MonoBehaviour
{
    private Toggle toggle;
    private GameObject panel;
    private Button disableVariantBtn;
    private TextMeshProUGUI cwndTaxText;
    private TextMeshProUGUI currentStateText;

    void Start()
    {
        toggle = gameObject.GetComponentInChildren<Toggle>();
        panel = gameObject.GetComponentsInChildren<Image>()[2].gameObject; //pegando a imagem no indice 2 pq antes tem a imagem do background e a do check
        disableVariantBtn = gameObject.GetComponentInChildren<Button>();
        cwndTaxText = panel.GetComponentsInChildren<TextMeshProUGUI>()[1];
        currentStateText = panel.GetComponentsInChildren<TextMeshProUGUI>()[3];


        if (toggle.isOn)
        {
            Choose(true);
            
        }

        toggle.onValueChanged.AddListener(
            (value)=>{
                Choose(value);
            }    
        );

        disableVariantBtn.onClick.AddListener(
            () =>
            {
                toggle.isOn = false;
                Choose(false);

                StartCoroutine(turnOffPanel());
                StartCoroutine(turnOnToggle());
            }
        );

        StartCoroutine(turnOffPanel());

        
    }

    private void Choose(bool value)
    {
        GameObject.Find("Manager").GetComponent<Main>().ChangeTcpState(gameObject.name,value);
        if (value == true)
        {
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        } else
        {
            StartCoroutine(turnOnToggle());
            panel.SetActive(false);
        }
    }
    

    public void ChangeCWNDTax(string value)
    {
        cwndTaxText.text = value;
    }

    public void ChangeCurrentState(string newState)
    {
        currentStateText.text = newState;
    }

    IEnumerator turnOffPanel()
    {
        yield return new WaitForSeconds(0.01f);
        panel.SetActive(false);
    }

    IEnumerator turnOffToggle()
    {
        yield return new WaitForSeconds(0.01f);
        toggle.gameObject.SetActive(false);
    }

    IEnumerator turnOnToggle()
    {
        yield return new WaitForSeconds(0.01f);
        toggle.gameObject.SetActive(true);
    }
}
