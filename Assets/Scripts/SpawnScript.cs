using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject ball;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private MapGeneratorScript mapGen;

    public void Spawn()
    {
        var newBall = Instantiate(ball, transform.position, transform.rotation);

        rb = newBall.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        sprite = newBall.GetComponent<SpriteRenderer>();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        StartCoroutine(FadeIn());
    }

    public void MoveSpawn()
    {
        var newSpawn = new Vector3(transform.position.x, transform.position.y+MapGeneratorScript.MAP_SIZE, transform.position.z);
        mapGen.AddToSpawns(newSpawn);
        transform.position = newSpawn;
    }

    private void Start()
    {
        Spawn();
        mapGen = FindObjectOfType<MapGeneratorScript>();
    }

    private IEnumerator FadeIn()
    {
        float counter = 0;
        float duration = 0.5f;
        

        //Get current color
        Color spriteColor = sprite.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            //Fade from 0 to 1
            float alpha = Mathf.Lerp(0, 1, counter / duration);

            //Change alpha only
            sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            //Wait for a frame
            yield return null;
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.MovePosition(rb.position + new Vector2(0, 0.0001f));
    }
}
