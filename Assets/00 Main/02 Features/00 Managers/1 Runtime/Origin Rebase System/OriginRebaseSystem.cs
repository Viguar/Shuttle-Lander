using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginRebaseSystem : MonoBehaviour
{
    public Transform Player;
    public Transform WorldContainer;
    public float BoundingBoxSize = 1000f;

    private Rigidbody playerRigidbody;

    void Start()
    {
        if (Player != null)
        {
            playerRigidbody = Player.GetComponent<Rigidbody>();
        }
    }

    // Use FixedUpdate for physics-based updates.
    void FixedUpdate()
    {
        if (Player == null || WorldContainer == null)
            return;

        Vector3 offset = Vector3.zero;
        Vector3 playerPos = Player.position;

        // Check along X axis
        if (playerPos.x > BoundingBoxSize)
            offset.x = -2 * BoundingBoxSize;
        else if (playerPos.x < -BoundingBoxSize)
            offset.x = 2 * BoundingBoxSize;

        // Check along Y axis (if vertical rebasing is desired)
        if (playerPos.y > BoundingBoxSize)
            offset.y = -2 * BoundingBoxSize;
        else if (playerPos.y < -BoundingBoxSize)
            offset.y = 2 * BoundingBoxSize;

        // Check along Z axis
        if (playerPos.z > BoundingBoxSize)
            offset.z = -2 * BoundingBoxSize;
        else if (playerPos.z < -BoundingBoxSize)
            offset.z = 2 * BoundingBoxSize;

        // If the player is outside the bounding box, teleport and shift the world.
        if (offset != Vector3.zero)
        {
            // Use the Rigidbody to update position if available.
            if (playerRigidbody != null)
            {
                // Teleport the player and reset velocity to prevent momentum from carrying them out again.
                playerRigidbody.position += offset;
                //playerRigidbody.velocity = Vector3.zero;
            }
            else
            {
                // Fallback: update transform directly.
                Player.position += offset;
            }

            // Shift the entire world container to maintain relative positions.
            WorldContainer.position += offset;
        }
    }

    // Draws the bounding box in the Scene view for debugging.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // The bounding box is centered at the origin and spans twice the BoundingBoxSize on each axis.
        Vector3 cubeSize = new Vector3(BoundingBoxSize * 2, BoundingBoxSize * 2, BoundingBoxSize * 2);
        Gizmos.DrawWireCube(Vector3.zero, cubeSize);
    }
}
