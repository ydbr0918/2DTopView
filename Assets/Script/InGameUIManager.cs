using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("계속하기 버튼 (MainLobby Canvas > ContinueButton)")]
    // 만약 인스펙터에 할당 안 하셨다면 Awake()에서 자동으로 찾아줍니다.
    public Button moveButton;
    public TextMeshProUGUI reloadText;
    public Button saveExitButton;
    public Button continueButton;

    private Player player;
    private DungeonGenerator dungeon;
    private Portal currentPortal;

    private void Awake()
    {
        // ── ① 씬에 떠 있는 오브젝트들을 이름으로 찾아서 할당 ──
        if (moveButton == null) moveButton = GameObject.Find("MoveButton")?.GetComponent<Button>();
        if (reloadText == null) reloadText = GameObject.Find("ReloadText")?.GetComponent<TextMeshProUGUI>();
        if (saveExitButton == null) saveExitButton = GameObject.Find("SaveExitButton")?.GetComponent<Button>();
        if (continueButton == null) continueButton = GameObject.Find("ContinueButton")?.GetComponent<Button>();

        // ── ② Null 체크 후 UI 초기 상태 설정 ──
        if (moveButton != null) moveButton.gameObject.SetActive(false);
        if (reloadText != null) reloadText.gameObject.SetActive(false);
        if (saveExitButton != null) saveExitButton.gameObject.SetActive(true);                     // 항상 보여주기
        if (continueButton != null) continueButton.gameObject.SetActive(SaveManager.HasSave);       // 저장이 있을 때만

        // ── ③ 안전하게 클릭 리스너 연결 ──
        if (moveButton != null) moveButton.onClick.AddListener(OnMoveButtonClicked);
        if (saveExitButton != null) saveExitButton.onClick.AddListener(OnSaveAndExit);
        if (continueButton != null) continueButton.onClick.AddListener(OnContinue);

        continueButton.gameObject.SetActive(SaveManager.HasSave);
    }

    private void Start()
    {
        // 이 시점에야 Player/DungeonGenerator가 씬에 올라와 있습니다
        player = FindObjectOfType<Player>();
        dungeon = FindObjectOfType<DungeonGenerator>();

        if (player == null) Debug.LogError("[UIManager] Start(): Player를 찾을 수 없습니다!");
        if (dungeon == null) Debug.LogError("[UIManager] Start(): DungeonGenerator를 찾을 수 없습니다!");
    }

    // ────────────────────────────────────────────────────

    // 재장전 팝업
    public void ShowReloadText() => reloadText?.gameObject.SetActive(true);
    public void HideReloadText() => reloadText?.gameObject.SetActive(false);

    // 방 이동 버튼
    public void ShowMoveButton(Portal portal)
    {
        currentPortal = portal;
        moveButton?.gameObject.SetActive(true);
    }
    public void HideMoveButton()
    {
        currentPortal = null;
        moveButton?.gameObject.SetActive(false);
    }
    private void OnMoveButtonClicked() => currentPortal?.MoveToNextScene();

    // ────────────────────────────────────────────────────

    // 저장 후 나가기
    public void OnSaveAndExit()
    {
        // 만약 Start() 시 할당이 실패했다면 한 번 더 찾아 봅니다
        player = player ?? FindObjectOfType<Player>();
        dungeon = dungeon ?? FindObjectOfType<DungeonGenerator>();

        if (player == null || dungeon == null)
        {
            Debug.LogError("[UIManager] 저장 불가: Player/DungeonGenerator 할당 안 됨");
            return;
        }

        // 1) SaveData 채우기
        var d = new SaveData();
        d.selectedWeaponIndex = SelectionData.Instance.SelectedWeaponIndex;
        d.selectedSkillIndex = SelectionData.Instance.SelectedSkillIndex;

        d.level = player.Level;
        d.exp = player.Exp;
        d.maxHp = player.MaxHp;
        d.currentHp = player.CurrentHp;
        d.maxAmmo = player.MaxAmmo;
        d.currentAmmo = player.CurrentAmmo;
        d.savedBulletSpeed = player.BulletSpeed;   // 플레이어가 들고 있는 현재 런타임 총알 속도
        d.savedFireRate = player.FireRate;

        d.roomPositions = dungeon.RoomPositions
                              .ConvertAll(p => new SerializableVector2Int(p.x, p.y));
        d.currentRoomPos = new SerializableVector2Int(
                              dungeon.CurrentRoomPos.x,
                              dungeon.CurrentRoomPos.y);
        d.clearedRooms = dungeon.ClearedRooms
                              .ConvertAll(p => new SerializableVector2Int(p.x, p.y));

        // 2) 저장
        SaveManager.SaveGame(d);

        // 3) 메인 로비로
        SceneManager.LoadScene("MainLobby");
    }

    // 이어하기
    public void OnContinue()
    {
        if (!SaveManager.HasSave) return;
        SceneManager.LoadScene("TopViewMap_1");
    }
}
