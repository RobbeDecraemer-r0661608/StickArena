﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float sprintSpeed;

    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode sprint;

    public Transform root;
    public Transform stick;

    private PlayerState currentstate;
    private PlayerState nextstate;

    private void Start()
    {
        currentstate = nextstate = new PlayerState() { timestamp = Time.time, pos = root.position, cam = Vector3.zero };
    }

    private void Update()
    {
        float interpolateFactor = (Time.time - currentstate.timestamp) / Time.fixedDeltaTime;
        root.position = Vector3.LerpUnclamped(currentstate.pos, nextstate.pos, interpolateFactor);
        stick.rotation = Quaternion.LerpUnclamped(Quaternion.Euler(0f, 0f, currentstate.rot), Quaternion.Euler(0f, 0f, nextstate.rot), interpolateFactor);
        currentstate.timestamp = Time.time;
        currentstate.pos = root.position;
        currentstate.rot = stick.rotation.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        float currentSpeed = speed;
        currentstate = nextstate.copy;
        Vector2 pos = nextstate.pos;
        Vector2 movement = Vector2.zero;
        
        if (Input.GetKey(forward)) movement.y++;
        if (Input.GetKey(backward)) movement.y--;
        if (Input.GetKey(right)) movement.x++;
        if (Input.GetKey(left)) movement.x--;
        if (Input.GetKey(sprint)) currentSpeed = sprintSpeed;

        pos += Vector2.ClampMagnitude(movement, 1f) * currentSpeed * Time.fixedDeltaTime;
        nextstate.pos = pos;
        nextstate.timestamp = Time.fixedTime + Time.fixedDeltaTime;

        Vector2 mouse = Input.mousePosition;
        float mouseX = mouse.x - Screen.width / 2f;
        float mouseY = mouse.y - Screen.height / 2f;
        nextstate.rot = Mathf.Atan2(mouseY, mouseX) * Mathf.Rad2Deg - 90f;
    }
}