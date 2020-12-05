using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Auto_Sort : MonoBehaviour
{
    //포오오오ㅗ오오오오실의 오토솔트!!!

    [SerializeField] private int sortingBase = 5000;
    [SerializeField] private float offset_Times = 1;
    [SerializeField] private float offset = 1;
    [SerializeField] private bool runOnlyOnce = false;
    [SerializeField] private bool TileRenderer = false;
    [SerializeField] private bool ParticleRenderer = false;

    private SpriteRenderer renderer_sprite;
    private LineRenderer renderer_tile;
    private ParticleSystemRenderer renderer_particle;

    private void Awake()
    {
        if (ParticleRenderer)
            renderer_particle = GetComponent<ParticleSystemRenderer>();
        else if (TileRenderer)
            renderer_tile = GetComponent<LineRenderer>();
        else
            renderer_sprite = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        if (ParticleRenderer)
            renderer_particle.sortingOrder = (int)(sortingBase - ((transform.position.y * offset_Times) - offset));
        else if (TileRenderer)
            renderer_tile.sortingOrder = (int)(sortingBase - ((transform.position.y * offset_Times) - offset));
        else
            renderer_sprite.sortingOrder = (int)(sortingBase - ((transform.position.y * offset_Times) - offset));
        if (runOnlyOnce)
            Destroy(this);
    }
}
