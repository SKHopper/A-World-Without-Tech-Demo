using StarterAssets;
using System.Collections;
using UnityEngine;



public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    CharacterController charControl;
    FirstPersonController firstPersonControl;
    Rigidbody rigidBody;

    //movement
    [SerializeField] float dashInputSeconds, dashSpeed, dashHeight, dashLength, dashCooldown;
    Vector2 lastMoveDirection = Vector2.zero;
    float lastMoveTime = 0f;
    bool justDashed = true;
    bool canDash = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charControl = GetComponent<CharacterController>();
        firstPersonControl = GetComponent<FirstPersonController>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //set move values for FPController to apply over time
    IEnumerator DashLaunch(Vector2 direction)
    {
        Debug.Log("Dashing");
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
