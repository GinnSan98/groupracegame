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
        racerposition();

    }

    public void waitstart(int timetowait)
    {
        StartCoroutine(waitforstart(timetowait));
        StartCoroutine(countdown(timetowait));
        GetComponent<Camera>().enabled = false;
        GetComponent<AudioListener>().enabled = false;
        playercam.enabled = true;
        playercam.gameObject.GetComponent<AudioListener>().enabled = true;
        playergui.SetActive(true);
        racegui.SetActive(true);
    }

    private IEnumerator countdown(int time)
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

    private IEnumerator waitforstart(int timetowait)
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
                racersorgo[i].GetComponent<CarDriving>().candrive = true;
                racersorgo[i].GetComponent<TimeSlowdown>().enabled = true;
            }
        }
        InvokeRepeating("racerposition", 0, 0.5f);

        yield return 0;
    }

    public Transform returnplayerahead(Lapcheckpoint me)
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


    public int returnposition(Lapcheckpoint me)
    {
        return racers.IndexOf(me);
    }
    private IEnumerator checkingpositions()
    {


        for (int i = 0; i < 4; i++)
        {
            top5[i].text = racers[i].name;
            yield return new WaitForSeconds(0.05f);
        }



        yield return 0;
    }

    void Update()
    {

    }

    private void racerposition()
    {
        racers = racersorgo.OrderByDescending(go => go.totalracevalue).ToList();
        StartCoroutine(checkingpositions());

    }
}
