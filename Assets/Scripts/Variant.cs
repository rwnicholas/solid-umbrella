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
            Choose();
            
        }

        toggle.onValueChanged.AddListener(
            (value)=>{
                Choose();
            }    
        );

        disableVariantBtn.onClick.AddListener(
            () =>
            {
                toggle.isOn = false;

                if (gameObject.name.Equals("tahoe".ToLower()))
                {
                    Tahoe(false);
                    print("entrou***********************0");
                } else if (gameObject.name.Equals("reno".ToLower()))
                {
                    Reno(false);
                } else if (gameObject.name.Equals("cubic".ToLower())) {
                    Cubic(false);
                }

                StartCoroutine(turnOffPanel());
                StartCoroutine(turnOnToggle());
            }
        );

        StartCoroutine(turnOffPanel());

        
    }

    private void Choose()
    {
        if (gameObject.name.Equals("tahoe".ToLower()))
        {
            Tahoe(true);
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        }
        else if (gameObject.name.Equals("reno".ToLower()))
        {
            Reno(true);
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        } else if (gameObject.name.Equals("cubic".ToLower())) {
            Cubic(true);
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        }
    }

    public void Tahoe(bool value)
    {
        GameObject.Find("Manager").GetComponent<Main>().ChangeTahoe(value);
        
    }

    public void Reno(bool value)
    {
        GameObject.Find("Manager").GetComponent<Main>().ChangeReno(value);
        
    }

    public void Cubic(bool value) {
        GameObject.Find("Manager").GetComponent<Main>().ChangeCubic(value);
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
