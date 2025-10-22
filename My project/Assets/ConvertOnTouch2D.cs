using UnityEngine;

public class ConvertOnTouch2D : MonoBehaviour
{
    [Header("ʶ������õı�ǩ")]
    public string enemyTag = "Enemy";

    [Header("��ѡ���Ƿ�ѵ������Ҳ�ĳ���ҵ����")]
    public bool copyPlayerSpriteToEnemy = true;

    [Header("��ѡ���Ƿ��ڽӹܺ������Լ�Ҳ�ɿ��ƣ�=�������ܿ��ƣ�")]
    public bool keepSelfControllable = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(enemyTag)) return;

        // ��ֹ�ظ�ת��
        if (other.GetComponent<ConvertedMarker>() != null) return;

        // 1) ���õ������ϱ��͹��ŵ����õġ�����ƶ��ű���
        var enemyPlayerCtrl = other.GetComponent<Player>();
        if (enemyPlayerCtrl != null)
        {
            enemyPlayerCtrl.enabled = true;
        }
        else
        {
            Debug.LogWarning($"Ŀ�� {other.name} ��û�ҵ� YourPlayerController���ǵð������ҽű�Ԥ�ȹҵ� Enemy �ϣ��Ƚ��ã���");
        }

        // 2) �رյ��˵��Զ��ƶ��ű�
        var autoMover = other.GetComponent<Enemy>();
        if (autoMover != null) autoMover.enabled = false;

        // 3) ��ѡ������� sprite ͬ��Ϊ��ҵ�
        if (copyPlayerSpriteToEnemy)
        {
            var mySR = GetComponent<SpriteRenderer>();
            var hisSR = other.GetComponent<SpriteRenderer>();
            if (mySR && hisSR) hisSR.sprite = mySR.sprite;
        }

        // 4) ��ѡ��ֻ��ѿ���Ȩ����ȥ�������Լ�������
        if (!keepSelfControllable)
        {
            var selfCtrl = GetComponent<Player>();
            if (selfCtrl != null) selfCtrl.enabled = false;
        }

        // �����ǣ�������δ���
        other.gameObject.AddComponent<ConvertedMarker>();
    }
}

// ���ڱ�ǡ��Ѿ����ӹܡ��ĵ��ˣ������ظ�����
public class ConvertedMarker : MonoBehaviour { }
