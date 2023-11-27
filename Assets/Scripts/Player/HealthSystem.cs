using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public static Vector2 lastCheckpointPos;
    
    private Animator animator;
    private PlayerMovement playerMovement;
    [SerializeField] private float respawnTime = 3;
    private float timer = 0;
    PlayerManager playerManager;
    private void Awake()
    {
        if (playerManager == null) playerManager = PlayerManager.getInstance();
        playerMovement = GetComponent<PlayerMovement>();

        EventSystem.SaveEventSystem.OnSaveGame += SaveGame;
        EventSystem.SaveEventSystem.OnLoadGame += LoadGame;

    }
    
    private void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        if (playerManager.values.ContainsKey("health"))
        {
            health = (int)playerManager.values["health"];
        }
    }
    private void OnDestroy()
    {
       playerManager.values["health"] = health;
       EventSystem.SaveEventSystem.OnSaveGame -= SaveGame;
       EventSystem.SaveEventSystem.OnLoadGame -= LoadGame;
    }
    private void Update()
    {
        if (health <= 0)
        {
            timer += Time.deltaTime;
            if(timer > respawnTime)
            {
                Respawn();
                timer = 0;
            }
        }
    }

    private void SaveGame(SaveData data) 
    { 
        data.Health = health;
        data.MaxHealth = maxHealth;
    }

    private void LoadGame(SaveData data) 
    { 
        maxHealth = data.MaxHealth;
        health = data.Health;
    }
    
  //wywo�ywane z innych skrypt�w gdy player ma dosta� jaki� dmg
    public void takeDamage(int dmg)
    {
        health -= dmg;
        //je�eli umiera (hp<0)
        if(health <= 0)
        {
            playerMovement.enabled = false;
            playerMovement.rb.velocity = Vector3.zero;
            animator.SetTrigger("DeadHit");
        }
        else
            animator.SetTrigger("Hit");
    }

    //jakby�my kiedy� heal chcieli zrobi� 
    public void heal(int heal)
    {
        health += heal;
    }
    public void Respawn()
    {
        animator.SetTrigger("Dead");

        gameObject.transform.position = lastCheckpointPos;
        health = maxHealth;
        playerMovement.enabled = true;
    }

    public void Respawn(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Respawn();
    }
}
