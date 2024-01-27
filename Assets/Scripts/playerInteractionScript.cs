using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteractionScript : MonoBehaviour
{

    public string foodPickUpTag = "foodPickUpPoint";
    public string foodDropOffTag = "foodDropPoint";
    public KeyCode interactKey = KeyCode.E;

    bool canPickUpFood = false;
    bool FoodHasBeenPickedUp = false;
    bool canDeliverFood = false;

    public GameEvent onFoodDelivered;
    
    GameObject refrenceToCurrentFlagTrigger = null; //this will only work as long as there arnt multiple overlapping flag triggers

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactKey))
        {
            if (canPickUpFood)
                FoodHasBeenPickedUp = true;
        
            if(FoodHasBeenPickedUp&&canDeliverFood)
            {
                //this is so scuffed - such a horrible way of using a function in a diffrent script (if i have extra time do it through events)
                
                onFoodDelivered.Raise();
                FoodHasBeenPickedUp = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TriggerScript>().getHasFlag())
            refrenceToCurrentFlagTrigger = other.gameObject;
        
        if (other.gameObject.tag == foodPickUpTag)
        {
            canPickUpFood = true;
        }
        else if(other.gameObject.tag == foodDropOffTag)
        {
            canDeliverFood= true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (refrenceToCurrentFlagTrigger != null)
            refrenceToCurrentFlagTrigger = null;

        if(other.gameObject.tag == foodPickUpTag)
        {
            canPickUpFood=false;
        }    
        else if(other.gameObject.tag == foodDropOffTag)
        {
            canDeliverFood=false;
        }
    }
}
