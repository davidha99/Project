using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Player : Agent
{
    Rigidbody rbody;

    // Párametros torque del palo, incrementar en Unity si se requiere giros más rápidos
    public float maxAngleVelocity;
    
    // El jugador necesita saber en donde está la pelota
    public GameObject ball;

    // El jugador necesita saber si hay gol o no
    public GameObject referee;

    // Start es llamado en el primer frame del juego
    void Start()
    {
        //conectamos el rbody con el componente rigidbody del palo
        rbody = GetComponent<Rigidbody>();
        //asignamos la velocidad del giro
        rbody.maxAngularVelocity = maxAngleVelocity;
    }

    // Update es llamado durante cada frame del juego. Este ya no se ocupa
    // porque los agentes no se actualizan de la misma manera que otro
    // objeto de un videojuego
    // void Update()
    // {
    //     float h = Input.GetAxis("Horizontal") * 10000f * Time.deltaTime;
    //     float v = Input.GetAxis("Vertical");
    //     rbody.AddForce(0,0, v * 50f);
    //     rbody.AddTorque(transform.forward * h * 1000);
    // }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Posición x, y, z de la pelota
        sensor.AddObservation(ball.transform.localPosition);

        // Velocidad de la pelota
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(ball.GetComponent<Rigidbody>().velocity.z);

        foreach (Transform child in this.transform)
        {
            // Posición x, y, z de:
            //  1. Cada uno de los jugadores del palo
            //  2. Ambos topes del palo
            //  3. Tubo del palo
            //  4. Mango del palo
            sensor.AddObservation(child.localPosition);
        }

        // Ambas porterías
        Collider[] nets = referee.GetComponents<Collider>();
        foreach (Collider net in nets)
        {
            sensor.AddObservation(net);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // ¿Qué acciones puede hacer nuestro agente para resolver el problema?
        // 1. Mover el palo sobre el eje z
        // 2. Girar el palo sobre el eje z
        Vector3 move = Vector3.zero;
        Vector3 torque = Vector3.zero;
        move.z = actions.ContinuousActions[0];
        torque.z = actions.ContinuousActions[1];
        rbody.AddForce(move * 50f);
        rbody.AddTorque(torque * 1000);

        // ¿Cómo vamos a calificar a nuestro agente? Es decir, ¿cómo  sabemos
        // si está haciendo las cosas bien o mal?
        if (playerHitsBall()) {
            SetReward(0.1f);
        }

        if (agentScores()) {
            SetReward(0.5f);
        }

        if (rivalScores()) {
            SetReward(-1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var action = actionsOut.ContinuousActions;
        action[0] = Input.GetAxis("Vertical"); // move
        action[1] = Input.GetAxis("Horizontal") * 10000f * Time.deltaTime; // torque
    }

    private bool playerHitsBall() {
        return false;
    }

    private bool agentScores() {
        return false;
    }

    private bool rivalScores() {
        return false;
    }
}
