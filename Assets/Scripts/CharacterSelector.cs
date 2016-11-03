using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterSelector : MonoBehaviour {

    bool selecting = false;

    public bool AllowSelect
    {
        get
        {
            return this.selecting;
        }

        set
        {
            this.selecting = value;
            this.UpdateStates();
        }
    }

    public string Current
    {
        get
        {
            return this.charObjects[this.index].name;
        }
    }

    public GameObject[] charObjects;
    int index = 0;

    void Reset()
    {
        this.charObjects = this.GetComponentsInChildren<Rigidbody2D>().Select((v)=> { return v.gameObject; }).ToArray();
    }

    void Awake()
    {
        index = Random.Range(0, this.charObjects.Length);
    }

    void OnEnable()
    {
        this.UpdateStates();
    }

    void OnDisable()
    {
        this.UpdateStates();
    }

    void UpdateStates()
    {
        for (int ii = 0; ii < this.charObjects.Length; ii++)
        {
            var selected = ii == this.index && this.selecting;
            var renderer = this.charObjects[ii].GetComponent<SpriteRenderer>();
            renderer.material.color = selected ? Color.white : new Color(0.3f, 0.3f, 0.3f);
        }
    }


	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            index = (int)Mathf.Repeat(index - 1, this.charObjects.Length);
            this.UpdateStates();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            index = (int)Mathf.Repeat(index + 1, this.charObjects.Length);
            this.UpdateStates();
        }

    }
}
