using UnityEngine;

public class DoorPortal : MonoBehaviour
{
    public Vector2Int myRoomPos;       // �� ���� ���� �� ��ǥ
    public Vector2Int targetRoomPos;   // �̵��� �� ��ǥ
    public Vector3 entryOffset;        // Ÿ�� �� ���� ��ġ ������

    private DungeonGenerator dungeon;
    private MiniMapController miniMapController;

    private void Start()
    {
        dungeon = FindObjectOfType<DungeonGenerator>();
        miniMapController = FindObjectOfType<MiniMapController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || dungeon == null) return;

        // 1) �÷��̾� �ڷ���Ʈ
        Vector3 tp = dungeon.GetRoomWorldPos(targetRoomPos) + entryOffset;
        other.transform.position = tp;

        // 2) �̴ϸ� ������Ʈ
        miniMapController?.UpdatePlayerIcon(targetRoomPos);

        // 3) �� �� ���� ���� ȣ��
        Room targetRoom = dungeon.GetRoomScript(targetRoomPos);
        targetRoom?.OnPlayerEnter();

        // 4) �� ��(=origin room �� ��)��, 
        //    ���� Ŭ������� ���� ��(=myRoomPos)�� ���� ������ ���ְ�,
        //    �̹� Ŭ����� ���̸� �ѵӴϴ�.
        Room originRoom = dungeon.GetRoomScript(myRoomPos);
        if (originRoom != null && !originRoom.IsCleared)
        {
            gameObject.SetActive(false);
        }
        // else: �̹� Ŭ����� ���̴� ���� ����Ӵϴ�(�ѵӴϴ�).
    }
}
