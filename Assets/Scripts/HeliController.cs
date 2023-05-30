using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeliController : MonoBehaviour
{
  private Rigidbody _rigidbody;
  [SerializeField] TextMeshProUGUI hud;
  [SerializeField] Transform rotor;

  [SerializeField] private float _responsiveness = 500f;
  [SerializeField] private float _throttleAmt = 25f;
  private float _throttle;

  private float _roll;
  private float _pitch;
  private float _yaw;

  private void Awake() {
    _rigidbody = GetComponent<Rigidbody>();
  }

  private void Update() {
    HandleInputs();
    UpdateHUD();

    rotor.Rotate(Vector3.up * _throttle);
  }

  private void FixedUpdate() {
    _rigidbody.AddForce(transform.up * _throttle, ForceMode.Impulse);

    _rigidbody.AddTorque(-transform.right * _pitch * _responsiveness);
    _rigidbody.AddTorque(-transform.forward * _roll * _responsiveness);
    _rigidbody.AddTorque(transform.up * _yaw * _responsiveness);
  }

  private void HandleInputs() {
    _roll = Input.GetAxis("Pitch");
    _pitch = Input.GetAxis("Roll");
    _yaw = Input.GetAxis("Yaw");

    if (Input.GetKey(KeyCode.Space)) {
      _throttle += Time.deltaTime * _throttleAmt;
    } else if (Input.GetKey(KeyCode.B)) {
      _throttle -= Time.deltaTime * _throttleAmt;
    }

    _throttle = Mathf.Clamp(_throttle, 0f, 100f);
  }

  private void UpdateHUD()
  {
    hud.text = "Throttle " + _throttle.ToString("F0") + "%\n";
    hud.text += "Airspeed: " + (_rigidbody.velocity.magnitude * 3.6f).ToString("F0") + "km/h\n";
    hud.text += "Altitude: " + transform.position.y.ToString("F0") + " m";
  }
}
