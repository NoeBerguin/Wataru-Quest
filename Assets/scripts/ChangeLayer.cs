using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class ChangeLayer : MonoBehaviour
{
    private Transform character; // Le personnage que vous voulez changer de layer
    public Tilemap tilemap; // La tilemap à utiliser pour détecter si le personnage est devant ou derrière un objet

    public int frontLayer = 2; // Le layer à utiliser lorsque le personnage est devant l'objet
    public int backLayer = 1; // Le layer à utiliser lorsque le personnage est derrière l'objet

    public float overlapRadius = 0.5f; // Le rayon du cercle utilisé pour détecter les objets de la tilemap

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        character = gameObject.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3Int characterCellPosition = tilemap.WorldToCell(character.position);

        // Parcours des cellules dans le cercle de rayon overlapRadius autour du personnage
        for (int x = -Mathf.CeilToInt(overlapRadius); x <= Mathf.CeilToInt(overlapRadius); x++)
        {
            for (int y = -Mathf.CeilToInt(overlapRadius); y <= Mathf.CeilToInt(overlapRadius); y++)
            {
                Vector3Int tilePosition = characterCellPosition + new Vector3Int(x, y, 0);
                if (Vector3.Distance(tilemap.CellToWorld(tilePosition), character.position) <= overlapRadius)
                {
                    TileBase tile = tilemap.GetTile(tilePosition);
                    if (tile != null && tilemap.GetSprite(tilePosition).bounds.max.y > character.position.y)
                    {
                        _spriteRenderer.sortingOrder = frontLayer;

                    }
                    else
                    {
                        _spriteRenderer.sortingOrder = backLayer;
                    }
                    return;
                }
            }
        }

        _spriteRenderer.sortingOrder = backLayer;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, overlapRadius);
    }
}

