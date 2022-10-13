using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTemp : MonoBehaviour {
    public float maxSpeed = 5f;
    
    [SerializeField]
    [Tooltip("Current power")]
    public PowerInfo cur_power;

    
    [HideInInspector] //This means that even though the variable is public, it will not show up in the Inspector.
    public Vector2 estimatedVelocity; // Used by SniperMovement.cs 
	void Update () // If your movement is dealing with forces/rigidbodies, you should handle it in fixedUpdate. All physics calculations happen there.
    {
        float deltaX = Input.GetAxis("Horizontal"); //To rebind axes and buttons, go to Edit > Project Settings > Input
        float deltaY = Input.GetAxis("Vertical"); //Look at the unity script reference for Input to see what else can be changed.
        Vector2 delta = new Vector2(deltaX, deltaY);

        if (delta.magnitude > 1) // We only want to normalize this if we need to. This means that if the player is using a controller with a joystick, they can move at slower speeds if they'd prefer.
        {
            delta.Normalize(); //This prevents the character from moving faster when both directions are pressed. Comment out this line and see the effects for yourself!
        }
        delta = delta * maxSpeed * Time.deltaTime; // Time.deltaTime is how much time has elapsed since the last frame and allows us to convert distance/frame to distance/time (or speed).
        transform.Translate(new Vector3(delta.x, delta.y, 0));  // Use this function instead of setting "transform.position" directly

        estimatedVelocity = delta/Time.deltaTime;   // We need this for the sniper enemy type. 
        // ^ In a bigger game, you want to keep all the relevant code as modular and separated as possible. Try to find ways around lines like this in your game.
    }

    public void changePower(PowerInfo power)
    {
        cur_power = power;
    }
}
