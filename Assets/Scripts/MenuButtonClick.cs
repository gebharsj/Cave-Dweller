using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuButtonClick : MonoBehaviour {

    public Image radial;
    public float lerpSpeed;

    bool isDjikstra;
    bool onButton;

    //RaycastHit2D hit;

    // Use this for initialization
    void Start () {
        
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        //hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.zero, 5000f);

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        
        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider == null)
            {
                onButton = false;
                radial.fillAmount = 0;
                StopCoroutines();
            }
            else //if(hit.collider.name == "Button")
            {
                onButton = true;
                CallCoroutine(hit.collider.gameObject);
            }
        }
        else
        {
            onButton = false;
            radial.fillAmount = 0;
            StopCoroutines();
        }

        Debug.DrawRay(ray.origin, ray.direction * 1000000);
    }

    void CallCoroutine(GameObject button)
    {
        StartCoroutine(Djikstra(button));
    }

    void StopCoroutines()
    {
        isDjikstra = false;
        StopAllCoroutines();
    }

    bool FillRadial()
    {        
        radial.fillAmount = Mathf.Lerp(radial.fillAmount, 1.25f, Time.deltaTime * lerpSpeed);
        
        if(!onButton)
        {
            StopCoroutines();
        }

        if (radial.fillAmount >= 1)
        {
            return true;
        }        
        
        return false;        
    }

    IEnumerator Djikstra(GameObject button)
    {        
        if (!isDjikstra)
        {
            isDjikstra = true;
            yield return new WaitUntil(FillRadial);
            ExecuteEvents.Execute(button, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            isDjikstra = false;
        }        
    }
}