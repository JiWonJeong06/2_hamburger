using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlot : MonoBehaviour, IDropHandler
{
    public TeamType teamType = TeamType.Player;
    public Vector3 spawnOffset = Vector3.zero;
    public UnitSpawner spawner;

    public void OnDrop(PointerEventData eventData)
    {
        UnitCard card = eventData.pointerDrag.GetComponent<UnitCard>();
        if (card != null)
        {
            Data data = System.Array.Find(CharJson.Instance.allCharacterData, c => c.id == card.characterId);
            if (data != null)
            {
                Vector3 worldPos = transform.position + spawnOffset;
                spawner.SpawnUnit(worldPos, data, teamType);
            }
        }
    }
}
