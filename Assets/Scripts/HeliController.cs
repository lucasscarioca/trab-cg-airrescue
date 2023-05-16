using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeliController : MonoBehaviour
{
  [Header("Helicopter Stats")]
  [Tooltip("How much the throttle ramps up or down.")]
  public float throttleIncrement = 0.1f;
  [Tooltip("Maximum engine thrust when at 100% throttle.")]
  public float maxThrust = 200f;
  [Tooltip("How responsive the helicopter is when rolling, pitching and yawing.")]
  public float responsiveness = 10f;

  [Tooltip("How much lift force the helicopter generates as it gains speed.")]
  public float lift = 135f;
  private float throttle;
  private float roll;
  private float pitch;
  private float yaw;

  private float responseModifier
  {
    get
    {
      return (rb.mass / 10f) * responsiveness;
    }
  }

  Rigidbody rb;
  [SerializeField] TextMeshProUGUI hud;
  [SerializeField] Transform rotor;

  private void Awake()
  {
    rb = GetComponent<Rigidbody>();
  }

  private void HandleInputs()
  {
    // Set rotation values from our axis inputs.
    roll = Input.GetAxis("Roll");
    pitch = Input.GetAxis("Pitch");
    yaw = Input.GetAxis("Yaw");

    // Handle throttle value being sure to clamp it between 0 and 100.
    if (Input.GetKey(KeyCode.Space)) throttle += throttleIncrement;
    else if (Input.GetKey(KeyCode.B) && (throttle >= 25 || transform.position.y <= 1)) throttle -= throttleIncrement;
    throttle = Mathf.Clamp(throttle, 0f, 100f);
  }

  private void Update()
  {
    HandleInputs();
    UpdateHUD();

    rotor.Rotate(Vector3.up * throttle);
  }

  private void FixedUpdate()
  {
    // Apply forces to our plane.
    rb.AddForce(transform.up * maxThrust * throttle);
    rb.AddTorque(transform.forward * pitch * responseModifier);
    rb.AddTorque(transform.right * yaw * responseModifier);
    rb.AddTorque(-transform.right * roll * responseModifier);

    rb.AddForce(Vector3.down * rb.velocity.magnitude * lift);
  }

  private void UpdateHUD()
  {
    hud.text = "Throttle " + throttle.ToString("F0") + "%\n";
    hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
    hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
  }
}
