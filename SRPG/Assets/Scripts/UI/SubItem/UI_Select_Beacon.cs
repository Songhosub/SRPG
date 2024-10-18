using UnityEngine;
using UnityEngine.UI;

public class UI_Select_Beacon : UIScene
{
    public Tile tile;
    public Image image;

    public override void Init()
    {
        base.Init();

        image = Util.FindChild<Image>(gameObject, "Image");

        if (tile == null)
        {
            if (transform.parent.TryGetComponent<Tile>(out Tile t))
            {
                tile = t;
            }
        }
    }

    public void Selected()
    {
        image.color = Color.gray;
    }

    public void UnSelected()
    {
        image.color = Color.white;
    }
}
