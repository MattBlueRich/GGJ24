using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerInteractionScript : MonoBehaviour
{

    public string foodPickUp1Tag = "foodPartOne";
    public string foodPickUp2Tag = "foodPartTwo";
    public string foodPickUp3Tag = "foodPartThree";
    public string foodDropOffTag = "foodDropPoint";
    public KeyCode interactKey = KeyCode.E;

    bool canPickUpFood = false;
    int nextStep = 1;

    bool foodHasBeenFinished = false;
    bool canDeliverFood = false;

    int currentFoodStep = 0;
    int triggerFoodStage = 0;
    bool stageHasBeenPickedUp = false;

    public GameEvent onFoodDelivered;
    
    GameObject refrenceToCurrentFlagTrigger = null; //this will only work as long as there arnt multiple overlapping flag triggers

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nextStep = currentFoodStep + 1;

        if(!foodHasBeenFinished && currentFoodStep == 3)
        {
            foodHasBeenFinished=true;
            currentFoodStep=0;
        }

        if(Input.GetKeyDown(interactKey))
        {
            if (canPickUpFood)
            {
                if(nextStep == triggerFoodStage && !stageHasBeenPickedUp)
                {
                    currentFoodStep = nextStep;
                    stageHasBeenPickedUp=true;
                }
            }
        
            if(foodHasBeenFinished&&canDeliverFood)
            {                
                onFoodDelivered.Raise();
                foodHasBeenFinished = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TriggerScript>().getHasFlag())
            refrenceToCurrentFlagTrigger = other.gameObject;
        
        if (other.gameObject.tag == foodPickUp1Tag)
        {
            canPickUpFood = true;
            triggerFoodStage = 1;
        }
        else if (other.gameObject.tag == foodPickUp2Tag)
        {
            canPickUpFood= true;
            triggerFoodStage = 2;
        }
        else if(other.gameObject.tag== foodPickUp3Tag)
        {
            canPickUpFood = true;
            triggerFoodStage = 3;
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

        if(other.gameObject.tag == foodPickUp1Tag)
        {
            canPickUpFood=false;
            triggerFoodStage = 0;
            stageHasBeenPickedUp = false;
        }    
        else if (other.gameObject.tag == foodPickUp2Tag)
        {
            canPickUpFood=false;
            triggerFoodStage = 0;
            stageHasBeenPickedUp = false;
        }
        else if(other.gameObject.tag == foodPickUp3Tag)
        {
            canPickUpFood = false;
            triggerFoodStage = 0;
            stageHasBeenPickedUp = false;
        }
        else if(other.gameObject.tag == foodDropOffTag)
        {
            canDeliverFood=false;
        }
    }
}
