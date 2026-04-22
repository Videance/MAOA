using UnityEngine;
using UnityEngine.InputSystem;

public class S_maoAnimadaInput : MonoBehaviour
{
    public InputActionProperty triggerV;
    public InputActionProperty gripV;

    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = triggerV.action.ReadValue<float>();
        float grip = gripV.action.ReadValue<float>();

        animator.SetFloat("Trigger", trigger);
        animator.SetFloat("Grip", grip);
    }
}
