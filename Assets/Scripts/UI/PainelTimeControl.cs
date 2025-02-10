using System.Linq;
using UnityEngine;

public class PainelTimeControl : MonoBehaviour
{
    public static PainelTimeControl instance;
    public TimeControl[] timeControls;

    private void Awake()
    {
        instance = this;
        timeControls[0].SetValueMax(1);
        timeControls[1].SetValueMax(5);
        timeControls[2].SetValueMax(4);
        timeControls[3].SetValueMax(2);
        InicialFillAmout();
    }

    public void InicialFillAmout()
    {
        for (int i = 0; i < timeControls.Count(); i++)
        {
            timeControls[i].SetFillAmount(0);
        }
    }
}
