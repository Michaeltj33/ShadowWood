using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PontosController : MonoBehaviour
{

    public static PontosController instance;
    public TextMeshProUGUI id;
    public TextMeshProUGUI[] ponto;

    public Color[] colors;
    public Image[] images;
    private int[] pontoint = { 0, 0 };//serve para mostrar valor de tora e ossos

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SetIconPainel(0);
    }

    public void Addpoint(int value, int pos)
    {
        pontoint[pos] += value;
        SetPonto(pontoint[pos].ToString(), pos);
    }

    public void Setpoint(int value, int pos)
    {
        pontoint[pos] = value;
        SetPonto(pontoint[pos].ToString(), pos);
    }

    public int Getpoint(int pos)
    {
        return pontoint[pos];
    }

    public int GetpointQtn()
    {
        return pontoint.Count();
    }

    public void SetId(string value)
    {
        id.text = value;
    }

    public void SetPonto(string value, int pos)
    {
        ponto[pos].text = value;
    }

    public int GetId()
    {
        return int.Parse(id.text);
    }

    public string GetIdString()
    {
        return id.text;
    }

    public int GetPonto(int pos)
    {
        return int.Parse(ponto[pos].text);
    }

    #region Icones Painel

    public void SetIconPainel(int pos)
    {
        for (int i = 0; i < images.Count(); i++)
        {
            if (i == pos)
            {
                if (images[i].color != colors[1])
                {
                    images[i].color = colors[1];
                }

            }
            else
            {
                if (images[i].color != colors[0])
                {
                    images[i].color = colors[0];
                }
            }
        }

    }

    #endregion
}
