using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//For Text Mesh Pro stuff
using TMPro;
public class RaceSystem : MonoBehaviour
{

    [SerializeField]
    private Camera playercam;
    [SerializeField]
    private List<Lapcheckpoint> racers;
    [SerializeField]
    private List<Lapcheckpoint> racersorgo;
    private bool racewon = false;
    [SerializeField]
    private GameObject playergui;
    [SerializeField]
    private GameObject racegui;

    [SerializeField]
    private TextMeshProUGUI countdowntext;

    [SerializeField]
    private List<TextMeshProUGUI> top5;
    // Use this for initialization
    void Start()
    {
        // StartCoroutine(waitforstart(5));
        Racerposition();

    }

    public void Waitstart(int timetowait)
    {
        StartCoroutine(Waitforstart(timetowait));
        StartCoroutine(Countdown(timetowait));
        GetComponent<Camera>().enabled = false;
        GetComponent<AudioListener>().enabled = false;
        playercam.enabled = true;
        playercam.gameObject.GetComponent<AudioListener>().enabled = true;
        playergui.SetActive(true);
        racegui.SetActive(true);
    }

    private IEnumerator Countdown(int time)
    {
        countdowntext.gameObject.SetActive(true);
        for (int i = 0; i < time; i++)
        {
            countdowntext.text = (time - i).ToString();
            countdowntext.GetComponent<Animator>().Play("Spin", 0);
            yield return new WaitForSeconds(1);
        }
        countdowntext.gameObject.SetActive(false);
        yield return 0;
    }

    private IEnumerator Waitforstart(int timetowait)
    {
        yield return new WaitForSeconds(timetowait);
        for (int i = 0; i < racersorgo.Capacity; i++)
        {
            if (racersorgo[i].GetComponent<AI>() == true)
            {
                racersorgo[i].GetComponent<AI>().canmove = true;
            }
            else if (racersorgo[i].GetComponent<CarDriving>() == true)
            {
                racersorgo[i].GetComponent<CarDriving>().canDrive = true;
                racersorgo[i].GetComponent<TimeSlowdown>().enabled = true;
            }
        }
        InvokeRepeating("Racerposition", 0, 0.5f);

        yield return 0;
    }

    public Transform Returnplayerahead(Lapcheckpoint me)
    {
        if (me != racers[0])
        {

            int tempnum = racers.IndexOf(me);
            return racers[tempnum - 1].transform;
        }
        else
        {
            return null;
        }
    }


    public int Returnposition(Lapcheckpoint me)
    {
        return racers.IndexOf(me);
    }

    private IEnumerator Checkingpositions()
    {


        for (int i = 0; i < 4; i++)
        {
            top5[i].text = racers[i].name;
            yield return new WaitForSeconds(0.05f);
        }



        yield return 0;
    }

    public int BetweenPlayers(Lapcheckpoint me, Lapcheckpoint other)
    {
        int total = Returnposition(me) - Returnposition(other);
        //5 last - 0 first = 5 bonus
        //0 - 5 = -5
        return total * 5;
    }

    void Update()
    {

    }

    private void Racerposition()
    {
        racers = racersorgo.OrderByDescending(go => go.totalracevalue).ToList();
        StartCoroutine(Checkingpositions());

    }
}
