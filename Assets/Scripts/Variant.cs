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

                if (gameObject.name.Equals("tcp1".ToLower()))
                {
                    Tcp1(false);
                    print("entrou***********************0");
                } else if (gameObject.name.Equals("tcp2".ToLower()))
                {
                    Tcp2(false);
                } else if (gameObject.name.Equals("tcp3".ToLower())) {
                    Tcp3(false);
                }

                StartCoroutine(turnOffPanel());
                StartCoroutine(turnOnToggle());
            }
        );

        StartCoroutine(turnOffPanel());

        
    }

    private void Choose()
    {
        if (gameObject.name.Equals("tcp1".ToLower()))
        {
            Tcp1(true);
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        }
        else if (gameObject.name.Equals("tcp2".ToLower()))
        {
            Tcp2(true);
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        } else if (gameObject.name.Equals("tcp3".ToLower())) {
            Tcp3(true);
            StartCoroutine(turnOffToggle());
            panel.SetActive(true);
        }
    }

    public void Tcp1(bool value)
    {
        GameObject.Find("Manager").GetComponent<Main>().ChangeTcp1(value);
        
    }

    public void Tcp2(bool value)
    {
        GameObject.Find("Manager").GetComponent<Main>().ChangeTcp2(value);
        
    }

    public void Tcp3(bool value) {
        GameObject.Find("Manager").GetComponent<Main>().ChangeTcp3(value);
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
