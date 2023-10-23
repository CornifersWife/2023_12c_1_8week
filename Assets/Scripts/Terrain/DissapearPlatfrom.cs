using System;
using System.Collections;
using UnityEngine;


public class DissapearPlatfrom : MonoBehaviour {
    
    private SpriteRenderer _sp;
    private Collider2D _c2d;

    public bool _ready = true;
    
    [SerializeField] private float _warningTime = 2.0f;
    [SerializeField] private float _respawnTime = 2.0f;
    
    private void Awake() {
        _sp = GetComponent<SpriteRenderer>();
        _c2d = GetComponent<Collider2D>();
    }

    private void Start() {
        _sp.color = Color.green;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("OnTriggerEnter2D called");
        if (_ready) {
            _ready = false;
            StartCoroutine(TriggerCollapse());
        }
    }


    private IEnumerator TriggerCollapse() {
        
        //warning
        _sp.color = Color.yellow;
        yield return new WaitForSeconds(_warningTime);
        
        //collapse
        _c2d.enabled = false;
        _sp.color = Color.red;
        yield return new WaitForSeconds(_respawnTime);

        //respawn
        _c2d.enabled = true;
        _sp.color = Color.green;
        _ready = true;
    }
    

 
}
