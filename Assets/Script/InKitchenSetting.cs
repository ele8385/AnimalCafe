using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InKitchenSetting : MonoBehaviour
{
    public OrderPapersManager orderPapersManager;
    public AnimalManager animalManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void InCounter()
    {
        animalManager.OpenMiniBalloon(true);
        gameObject.SetActive(false);

    }

    public void InKitchen()
    {
        gameObject.SetActive(true);
        animalManager.OpenMiniBalloon(false);
        StartCoroutine("InKitchenCo");
    }

    //sec초 뒤에 func함수 실행
    IEnumerator InKitchenCo()
    {
        orderPapersManager.ViewOrderPapers();
        yield return new WaitForSeconds(0.5f);
    }
}
