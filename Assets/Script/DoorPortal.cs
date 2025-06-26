using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    [HideInInspector] public Vector2Int myRoomPos;
    [HideInInspector] public Vector2Int targetRoomPos;
    [HideInInspector] public Vector3 entryOffset;

    private DungeonGenerator dungeon;
    private MiniMapController miniMapController;

    void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
        miniMapController = FindObjectOfType<MiniMapController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || dungeon == null) return;

      
        Vector3 wp = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = wp;

      
        dungeon.CurrentRoomPos = targetRoomPos;

      
        miniMapController?.UpdatePlayerIcon(targetRoomPos);

       
        Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        Room originRoom = dungeon.GetRoomScript(myRoomPos);
        if (originRoom != null && !originRoom.cleared)
            gameObject.SetActive(false);
    }
}
