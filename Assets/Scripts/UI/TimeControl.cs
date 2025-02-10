using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    public Image imageTime;
    private float time;
    private float getTime;
    private float valueMax;
    private bool active;

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            SetFillAmount(time);
            if (getTime != Mathf.FloorToInt(time))
            {
                getTime = Mathf.FloorToInt(time);
            }

            if (time <= 0)
            {
                active = false;
            }
        }
    }

    public void SetFillAmount(float min)
    {
        imageTime.fillAmount = min / valueMax;
    }

    public bool GetActive()
    {
        return active;
    }

    public void SetActiveTime()
    {
        SetFillAmount(valueMax);
        time = valueMax;
        getTime = 0;
        active = true;
    }

    public void SetValueMax(float value)
    {
        valueMax = value;
    }

}
