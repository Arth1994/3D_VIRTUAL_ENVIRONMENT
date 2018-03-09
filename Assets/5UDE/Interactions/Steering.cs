using UnityEngine;
using System.Collections;
using System.Threading;

public class Steering : MonoBehaviour {

	// Enumerate the states of steering
	public enum SteeringState {
		NotSteering,
		SteeringForward,
		SteeringBackward,
        Jump,
        Fall
	};
	
    // Inspector parameters
    [Tooltip("The tracking device used to determine absolute direction for steering.")]
    public CommonTracker tracker;

    [Tooltip("The controller joystick used to determine relative direction (forward/backward) and speed.")]
	public CommonAxis joystick;

	[Tooltip("A button required to be pressed to activate steering.")]
	public CommonButton button;

    [Tooltip("A button required to be pressed to activate Jumping.")]
    public CommonButton jumpButton;

    [Tooltip("The space that is translated by this interaction. Usually set to the physical tracking space.")]
    public CommonSpace space;

    [Tooltip("The median speed for movement expressed in meters per second.")]
    public float speed = 1.0f;

	// Private interaction variables
	private SteeringState state;
    private int i;



    // Called at the end of the program initialization
    void Start () {

		// Set initial steering state to not steering
		state = SteeringState.NotSteering;
	}
		
    // FixedUpdate is not called every graphical frame but rather every physics frame
	void FixedUpdate () {

		// If state is not steering
		if (state == SteeringState.NotSteering) {
		
			// If the joystick is pressed forward and the button is pressed
			if (joystick.GetAxis ().y > 0.0f && button.GetPress ()) {

				// Change state to steering forward
				state = SteeringState.SteeringForward;
			}
				
			// If the joystick is pressed backward and the button is pressed
			else if (joystick.GetAxis ().y < 0.0f && button.GetPress ()) {

				// Change state to steering backward
				state = SteeringState.SteeringBackward;
			}


            // If the trigger button is pressed
            else if (jumpButton.GetPress())
            {

                // Change state to jump state
                state = SteeringState.Jump;
            }



            // Process current not steering state
            else {

				// Nothing to do for not steering
			}
		}

		// If state is steering forward
		else if (state == SteeringState.SteeringForward) {

			// If the button is not pressed
			if (!button.GetPress ()) {

				// Change state to not steering 
				state = SteeringState.NotSteering;
			}

			// If the joystick is pressed backward and the button is pressed
			else if (joystick.GetAxis ().y < 0.0f && button.GetPress ()) {

				// Change state to steering backward
				state = SteeringState.SteeringBackward;
			}

			// Process current steering forward state
			else {

                Vector3 direction = tracker.transform.forward;
                direction.y = 0.0f;

				// Translate the space based on the tracker's absolute forward direction and the joystick's forward value
				space.transform.position += joystick.GetAxis ().y * tracker.transform.forward * speed * Time.deltaTime;
			}
		}

		// If state is steering backward
		else if (state == SteeringState.SteeringBackward) {

			// If the button is not pressed
			if (!button.GetPress ()) {

				// Change state to not steering 
				state = SteeringState.NotSteering;
			}

			// If the joystick is pressed forward and the button is pressed
			else if (joystick.GetAxis ().y > 0.0f && button.GetPress ()) {

				// Change state to steering forward
				state = SteeringState.SteeringForward;
			}

			// Process current steering backward state
			else {

                Vector3 direction = tracker.transform.forward;
                direction.y = 0.0f;

                // Translate the space based on the tracker's absolute forward direction and the joystick's backward value
                space.transform.position += joystick.GetAxis ().y * tracker.transform.forward * speed * Time.deltaTime;
			}
		}


        // If it is a jump state
        else if (state == SteeringState.Jump)
        {

            // If the button is not pressed
            if (!jumpButton.GetPress())
            {

                // Change state to fall 
                state = SteeringState.Fall;
            }


            // Process current steering jump state
            else
            {

                Vector3 direction;
                direction = Vector3.up;

                // Translate the space to so that the user can jump
                while (i<10)
                {

                    space.transform.position += Vector3.up * Time.deltaTime;
                    if (i == 9)
                    {
                        Thread.Sleep(1000);

                    }

                    i++;

                            
                }
                
                    


                
                
            }
        }





    }
}