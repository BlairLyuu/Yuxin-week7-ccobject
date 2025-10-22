using UnityEngine;

public class ConvertOnTouch2D : MonoBehaviour
{
    [Header("识别敌人用的标签")]
    public string enemyTag = "Enemy";

    [Header("可选：是否把敌人外观也改成玩家的外观")]
    public bool copyPlayerSpriteToEnemy = true;

    [Header("可选：是否在接管后保留我自己也可控制（=两个都能控制）")]
    public bool keepSelfControllable = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(enemyTag)) return;

        // 防止重复转换
        if (other.GetComponent<ConvertedMarker>() != null) return;

        // 1) 启用敌人身上本就挂着但禁用的“玩家移动脚本”
        var enemyPlayerCtrl = other.GetComponent<Player>();
        if (enemyPlayerCtrl != null)
        {
            enemyPlayerCtrl.enabled = true;
        }
        else
        {
            Debug.LogWarning($"目标 {other.name} 上没找到 YourPlayerController，记得把你的玩家脚本预先挂到 Enemy 上（先禁用）。");
        }

        // 2) 关闭敌人的自动移动脚本
        var autoMover = other.GetComponent<Enemy>();
        if (autoMover != null) autoMover.enabled = false;

        // 3) 可选：把外观 sprite 同步为玩家的
        if (copyPlayerSpriteToEnemy)
        {
            var mySR = GetComponent<SpriteRenderer>();
            var hisSR = other.GetComponent<SpriteRenderer>();
            if (mySR && hisSR) hisSR.sprite = mySR.sprite;
        }

        // 4) 可选：只想把控制权交出去，不想自己继续动
        if (!keepSelfControllable)
        {
            var selfCtrl = GetComponent<Player>();
            if (selfCtrl != null) selfCtrl.enabled = false;
        }

        // 打个标记，避免二次处理
        other.gameObject.AddComponent<ConvertedMarker>();
    }
}

// 用于标记“已经被接管”的敌人，避免重复触发
public class ConvertedMarker : MonoBehaviour { }
