using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP; // player의 체력을 표시하는 text
    [SerializeField]
    private TextMeshProUGUI textPlayerGold; // player의 골드를 표시하는 text
    [SerializeField]
    private TextMeshProUGUI textWave; // 현재 웨이브와 총 웨이브를 표시하는 text
    [SerializeField]
    private TextMeshProUGUI textEnemyCount; // 현재 적 개수와 총 개수를 표시하는 text
    [SerializeField]
    private PlayerHP playerHP; // player 체력
    [SerializeField]
    private PlayerGold playerGold; // player 골드
    [SerializeField]
    private WaveSystem waveSystem; // 웨이브 정보를 가지고 있는 오브젝트
    [SerializeField]
    private EnemySpawner enemySpawner; // 적 정보를 가지고 있는 변수

    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount.ToString() + '/' + enemySpawner.MaxEnemyCount.ToString();
    }
}
