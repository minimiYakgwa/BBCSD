using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP; // player�� ü���� ǥ���ϴ� text
    [SerializeField]
    private TextMeshProUGUI textPlayerGold; // player�� ��带 ǥ���ϴ� text
    [SerializeField]
    private TextMeshProUGUI textWave; // ���� ���̺�� �� ���̺긦 ǥ���ϴ� text
    [SerializeField]
    private TextMeshProUGUI textEnemyCount; // ���� �� ������ �� ������ ǥ���ϴ� text
    [SerializeField]
    private PlayerHP playerHP; // player ü��
    [SerializeField]
    private PlayerGold playerGold; // player ���
    [SerializeField]
    private WaveSystem waveSystem; // ���̺� ������ ������ �ִ� ������Ʈ
    [SerializeField]
    private EnemySpawner enemySpawner; // �� ������ ������ �ִ� ����

    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount.ToString() + '/' + enemySpawner.MaxEnemyCount.ToString();
    }
}
