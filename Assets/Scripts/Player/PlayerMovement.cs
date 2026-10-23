using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerMovement : MonoBehaviour, IPlayerMovement, ISliderUser
{
    CharacterController charControl;
    FirstPersonController firstPersonControl;
    Rigidbody rigidBody;
    ISlider dashSlider;

    //movement
    [SerializeField] float dashInputSeconds, dashSpeed, dashHeight, dashLength, dashCooldown;
    Vector2 lastMoveDirection = Vector2.zero;
    float lastMoveTime;
    bool justDashed = true;
    bool canDash = true;
    float lastDashTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Setup(ISlider newDashSlider)
    {
        charControl = GetComponent<CharacterController>();
        firstPersonControl = GetComponent<FirstPersonController>();
        rigidBody = GetComponent<Rigidbody>();

        dashSlider = newDashSlider;
        dashSlider.Setup(dashCooldown, 0);
        dashSlider.Toggle(false);
    }

    public string Name() { return "DashSlider"; }

    // Update is called once per frame
    void Update()
    {
        //ramp down cooldown slider if post-dash
        if (!canDash)
        {
            dashSlider.UpdateBar(
                dashCooldown - (Time.time - lastDashTime)
            );
        }
    }

    void StartDashSlider()
    {
        lastDashTime = Time.time;
        dashSlider.Toggle(true);
        dashSlider.UpdateBar(dashCooldown);
    }

    void EndDashSlider()
    {
        dashSlider.Toggle(false);
    }

    //set move values for FPController to apply over time
    IEnumerator DashLaunch(Vector2 direction)
    {
        StartDashSlider();

        canDash = false;
        direction *= dashSpeed;
        //rotated and multiplied dash direction
        firstPersonControl.SetDash(
            true,
            Quaternion.LookRotation(transform.forward, Vector3.up) * new Vector3(direction.x, dashHeight, direction.y)
        );

        yield return new WaitForSeconds(dashLength);
        firstPersonControl.SetDash(false, Vector3.zero);

        yield return new WaitForSeconds(Mathf.Clamp(dashCooldown - dashLength, 0, dashCooldown));
        canDash = true;

        EndDashSlider();
    }

    void RecordFirstDashInput(Vector2 direction)
    {
        lastMoveDirection = direction;
        lastMoveTime = Time.time;
    }

    public void Dash(Vector2 value)
    {
        //first dash input
        if (justDashed)
        {
            justDashed = false;
            RecordFirstDashInput(value);
        } 
        //second dash input
        else if (
               value == lastMoveDirection 
            && Time.time - lastMoveTime <= dashInputSeconds 
            && !firstPersonControl.GetIsDashing()
            && canDash
        )
        {
            StartCoroutine(DashLaunch(value));
            justDashed = true;
        }
        //second input fail
        else
        {
            RecordFirstDashInput(value);
        }
    }
}
