using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    [Header("계속하기 버튼 (MainLobby Canvas > ContinueButton)")]

    public Button moveButton;
    public TextMeshProUGUI reloadText;
    public Button saveExitButton;
    public Button continueButton;

    private Player player;
    private DungeonGenerator dungeon;
    private Portal currentPortal;

    private void Awake()
    {
    
        if (moveButton == null) moveButton = GameObject.Find("MoveButton")?.GetComponent<Button>();
        if (reloadText == null) reloadText = GameObject.Find("ReloadText")?.GetComponent<TextMeshProUGUI>();
        if (saveExitButton == null) saveExitButton = GameObject.Find("SaveExitButton")?.GetComponent<Button>();
        if (continueButton == null) continueButton = GameObject.Find("ContinueButton")?.GetComponent<Button>();

      
        if (moveButton != null) moveButton.gameObject.SetActive(false);
        if (reloadText != null) reloadText.gameObject.SetActive(false);
        if (saveExitButton != null) saveExitButton.gameObject.SetActive(true);                    
        if (continueButton != null) continueButton.gameObject.SetActive(SaveManager.HasSave);      

        if (moveButton != null) moveButton.onClick.AddListener(OnMoveButtonClicked);
        if (saveExitButton != null) saveExitButton.onClick.AddListener(OnSaveAndExit);
        if (continueButton != null) continueButton.onClick.AddListener(OnContinue);

        continueButton.gameObject.SetActive(SaveManager.HasSave);
    }

    private void Start()
    {

        player = FindObjectOfType<Player>();
        dungeon = FindObjectOfType<DungeonGenerator>();

        if (player == null) Debug.LogError("[UIManager] Start(): Player를 찾을 수 없습니다!");
        if (dungeon == null) Debug.LogError("[UIManager] Start(): DungeonGenerator를 찾을 수 없습니다!");
    }


    public void ShowReloadText() => reloadText?.gameObject.SetActive(true);
    public void HideReloadText() => reloadText?.gameObject.SetActive(false);

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


    public void OnSaveAndExit()
    {

        player = player ?? FindObjectOfType<Player>();
        dungeon = dungeon ?? FindObjectOfType<DungeonGenerator>();

        if (player == null || dungeon == null)
        {
            Debug.LogError("[UIManager] 저장 불가: Player/DungeonGenerator 할당 안 됨");
            return;
        }

    
        var d = new SaveData();
        d.selectedWeaponIndex = SelectionData.Instance.SelectedWeaponIndex;
        d.selectedSkillIndex = SelectionData.Instance.SelectedSkillIndex;

        d.level = player.Level;
        d.exp = player.Exp;
        d.maxHp = player.MaxHp;
        d.currentHp = player.CurrentHp;
        d.maxAmmo = player.MaxAmmo;
        d.currentAmmo = player.CurrentAmmo;
        d.savedBulletSpeed = player.BulletSpeed;   
        d.savedFireRate = player.FireRate;

        d.roomPositions = dungeon.RoomPositions
                              .ConvertAll(p => new SerializableVector2Int(p.x, p.y));
        d.currentRoomPos = new SerializableVector2Int(
                              dungeon.CurrentRoomPos.x,
                              dungeon.CurrentRoomPos.y);
        d.clearedRooms = dungeon.ClearedRooms
                              .ConvertAll(p => new SerializableVector2Int(p.x, p.y));

     
        SaveManager.SaveGame(d);

        
        SceneManager.LoadScene("MainLobby");
    }

    // 이어하기
    public void OnContinue()
    {
        if (!SaveManager.HasSave) return;
        SceneManager.LoadScene("TopViewMap_1");
    }
}
