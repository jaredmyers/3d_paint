using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
   [SerializeField]
   Transform hoursPivot = default, minutesPivot = default, secondsPivot = default;
   const float hoursToDegrees = -30f, minutesToDegrees = -6f, secondsToDegrees = -6f, degreesPerDayCycle = 15f;

    private TimeSpan timeContinuous;
   private DateTime timeDiscrete;
   public Transform dayCycle;
   [Range(0, 23)]
   
    private float hours, minutes, hoursTemp, AMPMcycle;
    private bool isAM;
    public Text textAMPM;
    private bool functionRan = false;
    public Text noon;
    private bool realTime = true;
    private bool AMPMflip = false;
    
    
    void Awake()
    {
        //Connecting sun to time of day
        timeDiscrete = DateTime.Now;
        Debug.Log(timeDiscrete.TimeOfDay.Hours);
        dayCycle.localRotation = Quaternion.Euler((timeDiscrete.TimeOfDay.Hours - 12) * degreesPerDayCycle, 0f, 0f);
        hours = (float)timeContinuous.TotalHours;
        hoursTemp = (float)timeContinuous.TotalHours;
        AMPMcycle = (float)timeContinuous.TotalHours;
    }



    void Update()
    {
        timeContinuous = DateTime.Now.TimeOfDay;
       
        TimeSpan time = DateTime.Now.TimeOfDay;

        minutesPivot.localRotation = Quaternion.Euler(0f, 0f, minutesToDegrees * (float)time.TotalMinutes);
        secondsPivot.localRotation = Quaternion.Euler(0f, 0f, secondsToDegrees * (float)time.TotalSeconds);

        if (realTime)
        {
            hoursPivot.localRotation = Quaternion.Euler(0f, 0f, hoursToDegrees * (float)time.TotalHours);
        }
        else
        {
            hoursPivot.localRotation = Quaternion.Euler(0f, 0f, hoursToDegrees * ((float)hours - 12));
        }

        if (AMPMflip == false)
        {
            if (timeDiscrete.TimeOfDay.Hours < 12)
            {
                isAM = true;
                textAMPM.text = "AM";
            }
            else
            {
                isAM = false;
                textAMPM.text = "PM";
            }
        }

        CheckNoon(realTime);
       

    }

    public void UpdateTime(float clickHourRotation)
    {
        Debug.Log((int)timeContinuous.TotalHours);
      
        dayCycle.localRotation = Quaternion.Euler((clickHourRotation-12) * degreesPerDayCycle, 0f, 0f);
        hours = clickHourRotation;
        realTime = false;

    }

    public void UpdateAMPM()
    {
       
      
        realTime = true;
        
        if (AMPMflip)
        {
           AMPMflip = false;
         
        }
        else if (AMPMflip == false)
        {
            AMPMflip = true;
        }
       

        if (isAM)
        {
            isAM = false;
            textAMPM.text = "PM";
            hoursTemp += 12f;
        }
        else
        {
            isAM = true;
            textAMPM.text = "AM";
            hoursTemp += 12f;
        }
        updateSunAMPM(hoursTemp);

    }

    public float getHours()
    {
        return hours;
    }

    public void updateSunAMPM(float updatedHours)
    {
        dayCycle.localRotation = Quaternion.Euler(((timeDiscrete.TimeOfDay.Hours - 12) + updatedHours) * degreesPerDayCycle, 0f, 0f);

    }

    public void CheckNoon(bool realTime)
    {

        if (!realTime)
        {
            noon.text = "   Noon";
        }
        else
        {
            if (AMPMflip)
            {
                noon.text = "Real Time Flipped ";
            }
            else
            {
                noon.text = "Real Time ";
            }
                
        }

    }

}
