using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject ball;
    private const float zPos = -10;

    private void Update() => ball = GameManager.Instance.player;

    void LateUpdate() => transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, zPos);
}
