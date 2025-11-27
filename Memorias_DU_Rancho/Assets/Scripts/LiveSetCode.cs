public int maxHealth = 10;
public int currentHealth = 7;

public Image[] Live;
public Sprite LiveFull;
public Sprite LiveEmpty;

void UpdateHeartsUI()
{
    for (int i = 0; i < Live.Length; i++)
    {
        if (i < currentHealth)
            Live[i].sprite = LiveFull;
        else
            Live[i].sprite = LiveEmpty;
    }
}