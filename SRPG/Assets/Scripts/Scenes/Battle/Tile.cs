using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum State
    {
        // �� ���
        Empty,

        // ���� �ִ� ���
        Full,

        // ��ֹ�
        Hurdle,
    }
    public State state;

    public int F { get { return G + H; } private set { } }
    //���� Ÿ�Ͽ����� �Ÿ�
    //ī��Ʈ�� ������Ų��
    public int G;
    //�޸���ƽ �Ÿ� => �� Ÿ�Ͽ��� ��ǥ Ÿ�ϱ��� ��ֹ��� ������ �Ÿ�
    //�Ÿ��� x, z�� Ÿ���� ũ��� ������ ���� ��
    public int H;

    public bool moveSelect = false;
    public bool attackSelect = false;

    public Vector2 Pos { get { return transform.position; } private set { } }
    public Vector2Int intPos { get; private set; }
    public List<Tile> _enemyRoute = new List<Tile>();
    public List<Tile> _playerRoute = new List<Tile>();
    public string dir;
    public Character currentCharacter;

    SpriteRenderer SR;
    private Sprite moveMat;
    private Sprite attackMat;
    private Sprite HealMat;
    private Sprite TempMat;
    private Sprite NormalMat;

    // Start is called before the first frame update
    private void Awake()
    {
        intPos = new Vector2Int(Mathf.RoundToInt(Pos.x * 10.0f), Mathf.RoundToInt(Pos.y * 10.0f));
        SR = gameObject.GetorAddComponent<SpriteRenderer>();
        TempMat = Managers.resource.Load<Sprite>("TileYellow", "Sprites/Tile/");
        HealMat = Managers.resource.Load<Sprite>("TileGreen", "Sprites/Tile/");
        attackMat = Managers.resource.Load<Sprite>("TileRed", "Sprites/Tile/");
        moveMat = Managers.resource.Load<Sprite>("TileBlue", "Sprites/Tile/");
        NormalMat = Managers.resource.Load<Sprite>("TileWhite", "Sprites/Tile/");
    }

    private void Start()
    {
        Managers.scene.map._tiles.Add(intPos, this);
    }

    public void MoveSelectMat()
    {
        SR.sprite = moveMat;
    }

    public void AttackSelectMat()
    {
        SR.sprite = attackMat;
    }

    public void HealSelectMat()
    {
        SR.sprite = HealMat;
    }

    public void TempSelectMat()
    {
        SR.sprite = TempMat;
    }

    public void UnSelected()
    {
        SR.sprite = NormalMat;
    }

    public void Clear()
    {
        if(state == State.Hurdle)
        {
            return;
        }

        UnSelected();
        moveSelect = false;
        attackSelect = false;
        _enemyRoute.Clear();
        _playerRoute.Clear();
        currentCharacter = null;
    }
}
