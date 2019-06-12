using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Take an input of -1 to 1 and pan horizontally.
/// Make the images move not the camera.
/// </summary>
public class PanHorizontal : MonoBehaviour
{
    /// <summary>
    /// Controller that is giving us the input mapped from -1 to 1
    /// </summary>
    public PlayerPanCamController camController;

    /// <summary>
    /// List of all screens.
    /// </summary>
    public List<Transform> screenList = new List<Transform>();

    //Array which holds the position of the screen. Index matches screenList length.
    int[] screenOrder;

    //If we save it then it doesn't have to be changed/loaded often.
    int center_index;

    //The in game world width of a screen. Note: 7.5f is what the iphone seems to be( about 750 pixels across).
    public float panel_length = 7.5f;

    //The reference for velocity with smoothdamp
    Vector3 ref_velocity, targetDiff;

    //ARray of target positions for the screens
    Vector3[] targetPosition;

    //How much smoothdamping
    public float smoothing = 0.5f;

    //Enum state for the panning and verifying the order
    public enum PanState { Interactable, VerifyingOrder };
    public PanState state = PanState.Interactable;

    //Enum state for whether ot not we are moving or settling
    public enum CarouselState { Moving, InteractEnd, Settling, Still};
    public CarouselState car_state = CarouselState.Moving;

    private void Start()
    {
        //Initialize array to same length.
        screenOrder = new int[screenList.Count];
       for(int i = 0; i < screenOrder.Length; i++)
        {
            screenOrder[i] = i;
            
            //Set the last one to be -1, so it will be positioned on the left of the first one.
            if(i == screenOrder.Length - 1)
            {
                screenOrder[i] = -1;
            }
        }

        //Initialize array to same length
        targetPosition = new Vector3[screenList.Count];

        //Init center index
        center_index = 0;
    }

    private void Update()
    {
        //Player input, mapped to -1 to 1. y is the horizontal because it is the y rotation that we expect.
        Vector2 input = camController.cur_rot;

        if(state == PanState.Interactable)
        {
            //When there is horizontal input, adjust
            if(car_state == CarouselState.Moving)
            {
                //Move all panels based on input
                foreach (Transform trans in screenList)
                {
                    trans.position = new Vector3(trans.position.x + input.y, trans.position.y, trans.position.z);
                }

                //If input is zero move to interactEnd
                if(input.y == 0)
                {
                    car_state = CarouselState.InteractEnd;
                }
            }
            else if (car_state == CarouselState.InteractEnd)//When there is zero horizontal input, calculate the closest position that would center the center index
            {
                Transform centerTransform = screenList[center_index];
                targetDiff = new Vector3(
                    centerTransform.position.x,
                    0,
                    0);

                for(int i = 0; i < targetPosition.Length; i++)
                {
                    targetPosition[i] = screenList[i].transform.position - targetDiff;
                }

                car_state = CarouselState.Settling;
            }
            else if(car_state == CarouselState.Settling)
            {
                //Exit settling state if we get input
                if(input.y != 0)
                {
                    car_state = CarouselState.Moving;
                }

                for(int i = 0; i < screenList.Count; i++)
                {
                        screenList[i].position = Vector3.SmoothDamp(
                            screenList[i].position,
                            targetPosition[i],
                            ref ref_velocity,
                            smoothing);
                }

               if (Vector3.Distance(screenList[center_index].position, new Vector3(0, screenList[center_index].position.y, screenList[center_index].position.z)) <= 0.1f)
                {
                    car_state = CarouselState.Still;
                }
            }
            else if(car_state == CarouselState.Still)
            {
                if(input.y != 0)
                {
                    car_state = CarouselState.Moving;
                }
            }

            //Adjust screen order based off position of the center panel.
            if (screenList[center_index].transform.position.x >= panel_length)
            {
                IncrementIndexes();

                state = PanState.VerifyingOrder;
            }
            else if (screenList[center_index].transform.position.x <= -panel_length)
            {
                DecrementIndexes();

                state = PanState.VerifyingOrder;
            }
        }
        else if(state == PanState.VerifyingOrder)
        {
            //Adjust panel positions so it works out.
            for(int i = 0; i < screenList.Count; i++)
            {
                screenList[i].transform.position = new Vector3(screenOrder[i] * panel_length, screenList[i].transform.position.y, screenList[i].transform.position.z);
            }


        
            state = PanState.Interactable;
        }
    }

    /// <summary>
    /// Returns the index of the center panel. Throws exception otherwise.
    /// </summary>
    /// <returns></returns>
    int GetCenterIndex()
    {
        for(int i = 0; i < screenOrder.Length; i++)
        {
            if(screenOrder[i] == 0)
            {
                return i;
            }
        }
        throw new System.Exception("Error, there isn't a panel with the order value: 0. No center panel.");
    }

    /// <summary>
    /// Gets the largest index.
    /// </summary>
    /// <returns></returns>
    int GetLargestIndex()
    {
        int value = -1;
        for (int i = 0; i < screenOrder.Length; i++)
        {
            if (screenOrder[i] > value)
            {
                value = screenOrder[i];
            }
        }
        return value;
    }

    /// <summary>
    /// Helper function that increments the screen order indexes. Maintains it so there is only one negative number, which is the panel to the left of center.
    /// </summary>
    private void IncrementIndexes()
    {
        int zero_index = -1;
        int max_index = -1;
        for(int i = 0; i < screenOrder.Length; i++)
        {
            //Increment all the positions.
            screenOrder[i]++;

            //If the new position is zero, this is our new zero index.
            if (screenOrder[i] == 0)
            {
                zero_index = i;
            }

            else if(screenOrder[i] >= screenOrder.Length - 1)
            {
                max_index = i;
            }
        }

        //Set the max value
        screenOrder[max_index] = -1;

        //Modify center index
        center_index = zero_index;
    }

    /// <summary>
    /// Helper function that decrements the screen order indexes. Maintains it so there is only one negative number, which is the panel to the left of center.
    /// </summary>
    private void DecrementIndexes()
    {
        int negative_index = -1;
        int zero_index = -1;
        for (int i = 0; i < screenOrder.Length; i++)
        {
            screenOrder[i]--;

            //If the new position is negative 2, save it as the negative index.
            if (screenOrder[i] == -2)
            {
                negative_index = i;
            }

            //If the new position is zero, this is our new zero index.
            else if (screenOrder[i] == 0)
            {
                zero_index = i;
            }

        }

        //Set the negative index to the max value.
        screenOrder[negative_index] = screenOrder.Length - 2;

        //Modify center index
        center_index = zero_index;
    }
}
