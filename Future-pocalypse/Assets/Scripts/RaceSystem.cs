using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//For Text Mesh Pro stuff
using TMPro;
public class RaceSystem : MonoBehaviour {

    [SerializeField]
    private List<Lapcheckpoint> racers;
    [SerializeField]
    private List<Lapcheckpoint> racersorgo;
    private bool racewon = false;

    [SerializeField]
    private List<TextMeshProUGUI> top5;
	// Use this for initialization
	void Start ()
    {
       
        InvokeRepeating("racerposition", 0, 2f);
	}

    public Transform returnplayerahead(Lapcheckpoint me)
    {
        if (me != racers[0])
        {
            
            int tempnum = racers.IndexOf(me);

            print(tempnum);
            return racers[tempnum - 1].transform;
        }
        else
        {
            return null;
        }
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

	void Update ()
    {
		
	}

    private void racerposition()
    {
        racers = racersorgo.OrderByDescending(go => go.totalracevalue).ToList();
        StartCoroutine(checkingpositions());

    }
}
