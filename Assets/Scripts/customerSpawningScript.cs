using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum customer_hairType
{
    None,
    hairA,
    hairB,
    hairC,
    hairD,
    hairE
}

public enum customer_hairColour
{
    None,
    blonde,
    brown,
    black
}

public enum customer_beardType
{
    None,
    beardA,
    beardB,
    beardC,
    beardD
}

public enum customer_hat
{
    None,
    cap1,
    cap2,
    cap3
}

public enum customer_chain
{
    None,
    chain1,
    chain2,
    chain3
}

public enum customer_completeSuit
{
    None,   //if this is None do the rest of the cloths
    banker,
    farmer,
    fireman,
    mechanic,
    nurse,
    securityGuard,
    seller,
    worker
}

public enum customer_top
{
    None,
    jacket,
    pullover,
    shirt,
    tShirt,
    tankTop
}

public enum customer_bottom
{
    None,
    shortPants,
    trousers
}

public enum customer_shoes
{
    None,
    shoes1,
    shoes2,
    shoes3
}


public struct customerAtributes
{
    customerAtributes
        ( 
        bool hasHair, customer_hairType hairTypeIs, customer_hairColour hairColourIs,
        bool hasBeard, customer_beardType beardTypeIs, customer_hat hatTypeIs, customer_chain chainTypeIs,
        bool hasCompleteSuit, customer_completeSuit completeSuitIs, 
        customer_top topIs, customer_bottom bottemIs, customer_shoes shoesIs
        )
    {

        Hair = hasHair;
        hairType = hairTypeIs;
        hairColour = hairColourIs;
        beard = hasBeard;
        beardType = beardTypeIs;
        hatType = hatTypeIs;
        chainType = chainTypeIs;
        CompleteSuit = hasCompleteSuit;
        completeSuitType = completeSuitIs;
        topType = topIs;
        bottomType = bottemIs;
        shoeType = shoesIs;
    }

    bool Hair;
    customer_hairType hairType; 
    customer_hairColour hairColour;
    bool beard;
    customer_beardType beardType;
    customer_hat hatType;
    customer_chain chainType;
    bool CompleteSuit;
    customer_completeSuit completeSuitType;
    customer_top topType;
    customer_bottom bottomType;
    customer_shoes shoeType;
}

public class customerSpawningScript : MonoBehaviour
{
    public void spawnNewCustomer(customerAtributes thisCustomerAttributes, GameObject customerPrefab, GameObject spawnPoint)
    {
        //could pass it a struct for what it wants to order
        GameObject justSpawned = Instantiate(customerPrefab, spawnPoint.transform);
    }
}
